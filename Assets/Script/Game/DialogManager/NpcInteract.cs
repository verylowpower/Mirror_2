using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    NPCDialog dialog;

    void Awake()
    {
        dialog = GetComponent<NPCDialog>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialog.Interact();
        }
    }
}
