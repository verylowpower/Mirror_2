using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager instance;
    public event System.Action OnSkillTreeChanged;


    [Header("Player Skill Points")]
    public int skillPoints;

    [Header("All Skill Nodes")]
    public List<SkillNode> skills = new();

    private Dictionary<string, SkillNode> skillDict = new();

    void Awake()
    {
        instance = this;

        foreach (var s in skills)
        {
            if (s.data != null && !skillDict.ContainsKey(s.data.skillId))
                skillDict.Add(s.data.skillId, s);
        }
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => PointCounter.instance != null);
        skillPoints = PointCounter.instance.point;
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
        OnSkillTreeChanged?.Invoke();
    }

    public bool BuySkill(string id)
    {
        if (!skillDict.TryGetValue(id, out SkillNode node))
            return false;

        if (!node.CanBuy())
            return false;

        skillPoints -= node.data.pointRequire;
        node.isUnlocked = true;

        MetaBuffManager.instance.Unlock(id);

        OnSkillTreeChanged?.Invoke();
        return true;
    }



}
