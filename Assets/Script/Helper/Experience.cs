using UnityEngine;

public class Experience : MonoBehaviour
{
    public int expAmount = 1;
    public float moveSpeed = 5f;

    private Transform character;

    void Start()
    {
        character = PlayerController.instance?.transform;
    }

    void Update()
    {
        if (character == null) return;

        float distance = Vector2.Distance(transform.position, character.position);

        if (distance < PlayerExperience.instance.collectRadius)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                character.position,
                moveSpeed * Time.deltaTime
            );
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerExperience.instance.AddExp(expAmount);
            Destroy(gameObject);
        }
    }
}
