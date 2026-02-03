using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    [Header("NPC ID")]
    public string iD;
    NPCDialog dialog;
    bool isPlayerNearby;

    void Awake()
    {
        dialog = GetComponent<NPCDialog>();
    }

    void Update()
    {
        if (!isPlayerNearby) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialog.Interact();
            NPCQuestCounter();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = false;
    }

    public void NPCQuestCounter()
    {
        QuestManager.instance.NotifyEvent(
           QuestType.TalkToNPC,
           iD,
           1);
    }
}
