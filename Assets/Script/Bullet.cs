using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private Vector2 dir;
    private int damage;
    private bool isEnemyBullet = false;

    public void Initialize(int dmg, Vector2 direction, bool enemyBullet, float bulletSpeed)
    {
        damage = dmg;
        dir = direction.normalized;
        isEnemyBullet = enemyBullet;
        speed = bulletSpeed;

        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnemyBullet)
        {
            var player = collision.GetComponent<PlayerDamageReceiver>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            var enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ChangeHealth(damage);
                Destroy(gameObject);
            }
        }
    }
}
