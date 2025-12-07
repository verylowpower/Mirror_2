using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : Enemy
{
    [SerializeField] private float attackRange = 0.8f;
    [SerializeField] private float attackCooldown = 1f;

    private float lastAttackTime = 0f;
    private Transform player;

    protected override void Awake()
    {
        base.Awake();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    protected override void Start()
    {
        base.Start();
        player = PlayerController.instance.transform;
    }

    private void Update()
    {
        if (player == null || agent == null) return;

        agent.SetDestination(player.position);

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        lastAttackTime = Time.time;

        PlayerDamageReceiver receiver = player.GetComponent<PlayerDamageReceiver>();
        if (receiver != null)
        {
            receiver.TakeDamage(damage);
        }
    }
}
