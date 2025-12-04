using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletHolder;
    [SerializeField] private float bulletSpeed = 10f;

    private Camera cam;

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
        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    private void Shoot()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector2 dir = (mouseWorld - transform.position).normalized;

        GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity, bulletHolder);

        Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * bulletSpeed;

        Destroy(b, 5f);
    }
}
