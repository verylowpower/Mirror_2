using UnityEngine;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack instance;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletHolder;

    [Header("Bullet Stats")]
    [SerializeField] public int bulletDamage = 10;
    [SerializeField] public float bulletSpeed;

    [Header("Fire Rate")]
    public float fireRate = 5f;
    private float nextShootTime = 0f;

    [Header("Bullet Buffs")]
    public bool hasIceBuff = false;
    public float slow;
    public float slowTime;
    public bool hasBurnBuff = false;
    public bool hasLightningBuff = false;

    [Header("Special Bullet Behavior")]
    public bool hasSpreadShot = false;
    public int pierceCount = 0;


    private Camera cam;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cam = Camera.main;

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
        if (Input.GetMouseButton(0) && Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + (1f / fireRate);
            Shoot();
        }
    }

    private void Shoot()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector2 baseDir = (mouseWorld - transform.position).normalized;

        // Tạo list buff
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


}
