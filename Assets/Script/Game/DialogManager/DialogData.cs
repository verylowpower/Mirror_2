using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialog/Dialog Data")]
public class DialogData : ScriptableObject
{
    public string dialogId;

    [TextArea(2, 5)]
    public string[] sentences;

    [Header("Quest")]
    public QuestData startQuest;
    public bool completeQuestOnEnd;
}
