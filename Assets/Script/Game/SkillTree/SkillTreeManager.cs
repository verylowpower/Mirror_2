using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager instance;

    [Header("Player Skill Points")]
    public int skillPoints;

    [Header("All Skill Nodes")]
    public List<SkillNode> skills = new();

    private Dictionary<string, SkillNode> skillDict = new();

    void Awake()
    {
        instance = this;
        skillPoints = PointCounter.instance.point;

        foreach (var s in skills)
        {
            if (s.data != null && !skillDict.ContainsKey(s.data.skillId))
                skillDict.Add(s.data.skillId, s);
        }
    }

    public bool IsSkillUnlocked(string id)
    {
        return skillDict.ContainsKey(id) && skillDict[id].isUnlocked;
    }

    public bool IsQuestUnlocked(string id)
    {
        return skillDict.ContainsKey(id) && skillDict[id].questUnlocked;
    }

    public bool CanBuy(string id)
    {
        if (!skillDict.ContainsKey(id)) return false;
        return skillDict[id].CanBuy();
    }

    public void UnlockSkillByQuest(string id)
    {
        if (!skillDict.ContainsKey(id)) return;

        skillDict[id].questUnlocked = true;
        RefreshAllUI();
    }

    public bool BuySkill(string id)
    {
        if (!skillDict.ContainsKey(id)) return false;

        SkillNode node = skillDict[id];

        if (!node.CanBuy())
            return false;

        skillPoints -= node.data.pointRequire;
        node.isUnlocked = true;

        // Apply Buff
        if (BuffLibrary.AllBuffs.TryGetValue(id, out Buff buff))
            buff.ApplyEffect?.Invoke();

        RefreshAllUI();
        return true;
    }

    public void RefreshAllUI()
    {
        foreach (var ui in FindObjectsOfType<SkillNodeUI>())
            ui.Refresh();
    }
}
