using UnityEngine;

public enum DialogQuestState
{
    Normal,
    BeforeQuest,
    InProgress,
    Completed
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