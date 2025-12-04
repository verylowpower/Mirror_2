using UnityEngine;

public class PlayerDamageReceiver : MonoBehaviour
{
    [SerializeField] private float hurtBoxRadius = 0.3f;
    [SerializeField] private LayerMask enemyMask;

    private PlayerHealth health;

    private void Awake()
    {
        health = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, hurtBoxRadius, enemyMask);
        if (hit == null) return;

        Enemy enemy = hit.GetComponent<Enemy>();

        if (enemy != null)
        {
            health.TakeDamage(enemy.Damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hurtBoxRadius);
    }
}
