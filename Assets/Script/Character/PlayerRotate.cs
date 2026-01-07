// using UnityEngine;

// public class PlayerRotate : MonoBehaviour
// {
//     void Update()
//     {
//         Vector3 mouseScreenPos = Input.mousePosition;
//         mouseScreenPos.z = 12f;

//         Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

//         Vector2 direction = mouseWorldPos - transform.position;
//         float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

//         if (angle > 90f || angle < -90f)
//         {
//             transform.localScale = new Vector3(1, -1, 1);
//             angle += 180f;
//         }
//         else
//         {
//             transform.localScale = new Vector3(1, 1, 1);
//         }

//         transform.rotation = Quaternion.Euler(0, 0, angle);
//     }
// }
