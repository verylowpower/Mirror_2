using UnityEngine;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    private float speed;
    private Vector2 dir;
    private int damage;
    private bool isEnemyBullet = false;
    public int pierceRemaining = 0;

    private List<IBulletBuff> bulletBuffs;

    public void Initialize(int dmg, Vector2 direction, bool enemyBullet, float bulletSpeed, List<IBulletBuff> buffs)
    {
        damage = dmg;
        dir = direction.normalized;
        isEnemyBullet = enemyBullet;
        speed = bulletSpeed;

        bulletBuffs = buffs;

        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isEnemyBullet)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ChangeHealth(damage);

                foreach (var buff in bulletBuffs)
                    buff.Apply(enemy);

                if (pierceRemaining > 0)
                {
                    pierceRemaining--;
                    return;
                }

                Destroy(gameObject);
            }
        }
        else
        {
            var player = collision.GetComponent<PlayerDamageReceiver>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

}
