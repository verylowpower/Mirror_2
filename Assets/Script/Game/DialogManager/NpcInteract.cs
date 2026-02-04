using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    [Header("NPC ID")]
    public string iD;

    [Header("Optional UI")]
    public SkillTreeUI skillTreeUI;

    NPCDialog dialog;
    bool isPlayerNearby;
    bool skillUIOpen;

    void Awake()
    {
        dialog = GetComponent<NPCDialog>();
    }

    void Update()
    {
        if (!isPlayerNearby) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"Press Space on NPC: {iD}, skillTreeUI = {skillTreeUI}");

            if (iD == "BS" && skillTreeUI != null)
            {
                ToggleSkillTree();
            }
            else
            {
                dialog?.Interact();
                NPCQuestCounter();
            }
        }

    }

    void ToggleSkillTree()
    {
        skillUIOpen = !skillUIOpen;

        if (skillUIOpen)
            skillTreeUI.Open();
        else
            skillTreeUI.Close();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerNearby = false;

        if (skillUIOpen && skillTreeUI != null)
        {
            skillTreeUI.Close();
            skillUIOpen = false;
        }
    }

    void NPCQuestCounter()
    {
        QuestManager.instance.NotifyEvent(
            QuestType.TalkToNPC,
            iD,
            1);
    }
}
