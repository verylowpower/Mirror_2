using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    private enum BossState
    {
        Chase,
        RangeAttack,
        MeleeAttack,
        Dash,
        MultiShot
    }

    [Header("Ranges")]
    [SerializeField] private float detectRange = 8f;
    [SerializeField] private float meleeRange = 1f;
    [SerializeField] private float rangeAttackMin = 2f;
    [SerializeField] private float rangeAttackMax = 5f;

    [Header("Attack")]
    [SerializeField] private float meleeCooldown = 1f;
    [SerializeField] private float rangeCooldown = 1.2f;
    [SerializeField] private GameObject bulletPF;
    [SerializeField] private Transform firePoint1;
    [SerializeField] private Transform firePoint2;

    [Header("Dash")]
    [SerializeField] private float dashDistance = 4f;
    [SerializeField] private float dashCooldown = 4f;
    [SerializeField] private float dashSpeed = 15f;
    private Vector3 dashTarget;


    [Header("Multi Shot")]
    [SerializeField] private int multiShotCount = 12;
    [SerializeField] private float multiShotCooldown = 6f;

    private float lastDashTime;
    private bool isDashing = false;
    private float lastMultiShotTime;

    private BossState currentState;
    private Transform player;
    Animator animator;
    private float lastMeleeTime;
    private float lastRangeTime;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    protected override void Start()
    {
        base.Start();
        player = PlayerController.instance.transform;
        currentState = BossState.Chase;
    }

    private void Update()
    {
        if (player == null || agent == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        FlipTowards(player.position);

        if (currentState == BossState.Chase)
        {
            if (Time.time >= lastMultiShotTime + multiShotCooldown)
            {
                ChangeState(BossState.MultiShot);
                return;
            }

            if (Time.time >= lastDashTime + dashCooldown)
            {
                ChangeState(BossState.Dash);
                return;
            }
        }


        switch (currentState)
        {
            case BossState.Chase:
                HandleChase(distance);
                break;

            case BossState.RangeAttack:
                HandleRangeAttack(distance);
                break;

            case BossState.MeleeAttack:
                HandleMeleeAttack(distance);
                break;

            case BossState.Dash:
                HandleDash();
                break;

            case BossState.MultiShot:
                HandleMultiShot();
                break;
        }
    }

    private void HandleChase(float distance)
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        if (distance <= meleeRange)
            ChangeState(BossState.MeleeAttack);
        else if (distance <= rangeAttackMax && distance >= rangeAttackMin)
            ChangeState(BossState.RangeAttack);
    }

    private void HandleRangeAttack(float distance)
    {
        if (distance < rangeAttackMin)
        {
            Vector2 dir = (transform.position - player.position).normalized;
            agent.isStopped = false;
            agent.SetDestination(transform.position + (Vector3)dir * 2f);
            return;
        }

        if (distance > rangeAttackMax)
        {
            ChangeState(BossState.Chase);
            return;
        }

        agent.isStopped = true;
        TryRangeAttack();
    }

    private void HandleMeleeAttack(float distance)
    {
        agent.isStopped = true;

        if (distance > meleeRange + 0.3f)
        {
            ChangeState(BossState.Chase);
            return;
        }

        TryMeleeAttack();
    }

    private void TryMeleeAttack()
    {
        if (Time.time < lastMeleeTime + meleeCooldown) return;

        lastMeleeTime = Time.time;

        PlayerDamageReceiver receiver = player.GetComponent<PlayerDamageReceiver>();
        if (receiver != null)
            receiver.TakeDamage(Damage);
    }

    private void TryRangeAttack()
    {
        if (Time.time < lastRangeTime + rangeCooldown) return;

        lastRangeTime = Time.time;
        Shoot();
    }

    private void Shoot()
    {
        Vector2 dir = (player.position - firePoint1.position).normalized;

        GameObject b = Instantiate(bulletPF, firePoint1.position, Quaternion.identity);
        Bullet bullet = b.GetComponent<Bullet>();
        bullet.Initialize(Damage, dir, true, bulletSpeed, null);
    }

    private void HandleDash()
    {
        if (!isDashing)
        {
            isDashing = true;
            lastDashTime = Time.time;
            animator.SetBool("IsDashing", true);

            agent.isStopped = true;
            agent.ResetPath();

            Vector2 dir = (player.position - transform.position).normalized;
            dashTarget = transform.position + (Vector3)dir * dashDistance;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            dashTarget,
            dashSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, dashTarget) < 0.05f)
        {
            isDashing = false;
            animator.SetBool("IsDashing", false);
            ChangeState(BossState.Chase);
        }
    }


    private void HandleMultiShot()
    {
        agent.isStopped = true;

        if (Time.time < lastMultiShotTime + multiShotCooldown)
        {
            ChangeState(BossState.Chase);
            return;
        }

        lastMultiShotTime = Time.time;

        float angleStep = 360f / multiShotCount;

        for (int i = 0; i < multiShotCount; i++)
        {
            float angle = angleStep * i;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

            GameObject b = Instantiate(bulletPF, firePoint1.position, Quaternion.identity);
            GameObject c = Instantiate(bulletPF, firePoint2.position, Quaternion.identity);
            Bullet bullet = b.GetComponent<Bullet>();
            Bullet bullet2 = c.GetComponent<Bullet>();
            bullet.Initialize(Damage, dir, true, bulletSpeed, null);
            bullet2.Initialize(Damage, dir, true, bulletSpeed, null);
        }

        ChangeState(BossState.Chase);
    }


    private void ChangeState(BossState newState)
    {
        currentState = newState;
    }

    private void FlipTowards(Vector3 target)
    {
        if (spriteRender == null) return;
        spriteRender.flipX = (target.x < transform.position.x);
    }
}
