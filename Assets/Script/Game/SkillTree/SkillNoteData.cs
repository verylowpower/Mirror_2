using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillTree/Skill")]
public class SkillNodeData : ScriptableObject
{
    // public static SkillNodeData instance;
    public string skillId;
    public string displayName;
    public string description;
    public Sprite icon;
    public List<string> requiredSkills;
    public int pointRequire;


    // void Awake()
    // {
    //     instance = this;
    // }
}
