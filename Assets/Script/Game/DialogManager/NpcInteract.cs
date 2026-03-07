using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteract : MonoBehaviour
{
    [Header("NPC ID")]
    public string iD;

    [Header("Optional UI")]
    public SkillTreeUI skillTreeUI;

    NPCDialog dialog;

    bool isPlayerNearby;
    bool skillUIOpen;

    PlayerInputAction input;

    void Awake()
    {
        dialog = GetComponent<NPCDialog>();
        input = new PlayerInputAction();
    }

    void OnEnable()
    {
        input.Enable();
        input.Input.Confirm.performed += OnConfirm;
    }

    void OnDisable()
    {
        input.Input.Confirm.performed -= OnConfirm;
        input.Disable();
    }

    void OnConfirm(InputAction.CallbackContext ctx)
    {
        // nếu dialog đang mở → tiếp tục hội thoại
        if (DialogUI.Instance != null && DialogUI.Instance.IsShowing)
        {
            DialogUI.Instance.NextSentence();
            return;
        }

        if (!isPlayerNearby) return;

        Debug.Log($"Interact with NPC: {iD}");

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