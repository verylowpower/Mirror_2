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

    Rigidbody2D rb;
    Animator animator;
    Collider2D col;

    bool isExploding = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    public void Initialize(
        int dmg,
        Vector2 direction,
        bool enemyBullet,
        float bulletSpeed,
        List<IBulletBuff> buffs)
    {
        damage = dmg;
        dir = direction.normalized;
        isEnemyBullet = enemyBullet;
        speed = bulletSpeed;
        bulletBuffs = buffs;

        rb.linearVelocity = dir * speed;

        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExploding) return;

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

                Explode();
            }
            else if (collision.CompareTag("Enemy") || collision.CompareTag("Wall"))
            {
                Explode();
            }
        }
        else
        {
            var player = collision.GetComponent<PlayerDamageReceiver>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Explode();
            }
        }
    }

    void Explode()
    {
        isExploding = true;

        rb.linearVelocity = Vector2.zero;
        col.enabled = false;

        if (animator != null)
            animator.SetTrigger("Impact");

        Destroy(gameObject, 0.4f);
    }
}
