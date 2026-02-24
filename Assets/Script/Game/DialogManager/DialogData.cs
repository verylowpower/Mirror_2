using UnityEngine;

public enum DialogQuestState
{
    None,           // Dialog bình thường
    BeforeQuest,    // Trước khi nhận quest
    InProgress,     // Đang làm quest
    Completed       // Hoàn thành quest (special)
}

[CreateAssetMenu(menuName = "Dialog/Dialog Data")]
public class DialogData : ScriptableObject
{
    [TextArea(2, 5)]
    public string[] sentences;

    [Header("Quest")]
    public QuestData relatedQuest;
    public DialogQuestState questState;

    [Header("Auto Actions")]
    public bool startQuestOnEnd;
}