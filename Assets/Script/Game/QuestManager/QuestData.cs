using UnityEngine;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(menuName = "Quest/Quest")]
public class QuestData : ScriptableObject
{
    public string questId;
    public string questName;
    [TextArea] public string description;
    public List<QuestCondition> conditions;
    public QuestData nextQuest;
    [Header("Skill ID")]
    public string rewardSkillId;
    public bool autoStartNext = true;
}
