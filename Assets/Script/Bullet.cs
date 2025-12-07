using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 dir;

    private int damage;
    private bool isEnemyBullet = false;

    public void Initialize(int dmg, Vector2 direction, bool enemyBullet)
    {
        damage = dmg;
        dir = direction.normalized;
        isEnemyBullet = enemyBullet;

        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnemyBullet)
        {
            // Enemy -> Player
            PlayerDamageReceiver player = collision.GetComponent<PlayerDamageReceiver>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            // Player -> Enemy
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ChangeHealth(damage);
                Destroy(gameObject);
            }
        }
    }
}
