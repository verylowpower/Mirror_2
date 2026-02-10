using UnityEngine;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack instance;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletHolder;
    public bool IsAttacking { get; private set; }

    [Header("Bullet Stats")]
    public int bulletDamage = 10;
    public float bulletSpeed;

    [Header("Fire Rate")]
    public float fireRate = 5f;
    private float nextShootTime = 0f;

    [Header("Bullet Buffs")]
    public bool hasIceBuff;
    public float slow;
    public float slowTime;
    public bool hasBurnBuff;
    public bool hasLightningBuff;

    [Header("Special Bullet Behavior")]
    public bool hasSpreadShot;
    public int pierceCount;

    [Header("Melee")]
    [SerializeField] private Animator animator;
    [SerializeField] private float meleeCooldown = 0.6f;
    [SerializeField] private int meleeDamage = 15;
    [SerializeField] private float meleeRange = 1.2f;
    [SerializeField] private LayerMask enemyLayer;

    private float nextMeleeTime;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        if (bulletHolder == null)
        {
            GameObject holder = GameObject.Find("PlayerBullets");
            if (holder == null)
                holder = new GameObject("PlayerBullets");
            bulletHolder = holder.transform;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryMelee();
        }
        if (Input.GetMouseButton(1))
        {
            IsAttacking = true;
            if (Time.time >= nextShootTime)
            {
                nextShootTime = Time.time + (1f / fireRate);
                Shoot();
            }
        }
        else
        {
            IsAttacking = false;
        }

    }

    public void Shoot()
    {
        Vector2 baseDir;
        if (PlayerController.instance.usingGamepad && PlayerController.instance.aimInput.sqrMagnitude > 0.1f)
        {
            baseDir = PlayerController.instance.aimInput.normalized;
        }
        else
        {
            Camera cam = Camera.main;
            if (cam == null) return;
            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;
            baseDir = (mouseWorld - transform.position).normalized;
        }
        List<IBulletBuff> buffs = new();
        bool allowIce = hasIceBuff && !hasBurnBuff;
        bool allowBurn = hasBurnBuff && !hasIceBuff;

        if (allowIce) buffs.Add(new IceBulletBuff(slow, slowTime));
        if (allowBurn) buffs.Add(new BurnBulletBuff(3, 1f));
        if (hasLightningBuff) buffs.Add(new LightningBulletBuff(2, 3));
        if (hasSpreadShot)
        {
            float angle = 15f;
            ShootBullet(Quaternion.Euler(0, 0, -angle) * baseDir, buffs);
            ShootBullet(baseDir, buffs);
            ShootBullet(Quaternion.Euler(0, 0, angle) * baseDir, buffs);
        }
        else
        {
            ShootBullet(baseDir, buffs);
        }
    }


    private void ShootBullet(Vector2 dir, List<IBulletBuff> buffs)
    {
        GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity, bulletHolder);
        Bullet bullet = b.GetComponent<Bullet>();
        bullet.pierceRemaining = pierceCount;
        bullet.Initialize(bulletDamage, dir, false, bulletSpeed, buffs);
    }

    void TryMelee()
    {
        if (Time.time < nextMeleeTime) return;
        nextMeleeTime = Time.time + meleeCooldown;
        animator.SetTrigger("Melee");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, meleeRange);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
                enemy.ChangeHealth(meleeDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }

}
