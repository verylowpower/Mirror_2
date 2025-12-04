using UnityEngine;
using UnityEngine.AI;

public class RangeEnemy : Enemy
{
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float stopDistance = 2f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private GameObject bulletPF;
    //[SerializeField] private Transform bulletHolder;

    private Transform player;
    private float lastAttackTime;

    // protected override void Awake()
    // {
    //     if (bulletHolder == null)
    //     {
    //         GameObject holder = GameObject.Find("BulletHolder");
    //         if (holder != null)
    //             bulletHolder = holder.transform;
    //         else
    //         {
    //             holder = new GameObject("BulletHolder");
    //             bulletHolder = holder.transform;
    //         }
    //     }
    // }

    protected override void Start()
    {
        base.Start();

        if (PlayerController.instance != null)
            player = PlayerController.instance.transform;
    }

    private void Update()
    {
        if (player == null || agent == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        FlipTowards(player.position);

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else if (distance < stopDistance)
        {
            agent.isStopped = false;
            Vector2 dir = (transform.position - player.position).normalized;
            Vector3 backPos = transform.position + (Vector3)dir * 1.5f;
            agent.SetDestination(backPos);
        }
        else
        {
            agent.isStopped = true;
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        lastAttackTime = Time.time;
        Shoot();
    }

    private void Shoot()
    {
        Vector2 dir = (player.position - transform.position).normalized;

        GameObject b = Instantiate(bulletPF, transform.position, Quaternion.identity);

        // if (bulletHolder != null)
        //     b.transform.SetParent(bulletHolder);

        Bullet bullet = b.GetComponent<Bullet>();
        if (bullet != null)
            bullet.SetDirection(dir);

        Destroy(b, 5f);
    }

    private void FlipTowards(Vector3 target)
    {
        if (spriteRender == null) return;

        spriteRender.flipX = (target.x < transform.position.x);
    }
}
