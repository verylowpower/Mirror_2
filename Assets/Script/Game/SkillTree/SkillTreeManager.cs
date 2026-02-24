using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager instance;
    public event System.Action OnSkillTreeChanged;

    // [Header("Player Skill Points")]
    // public int skillPoints;

    [Header("All Skill Nodes")]
    public List<SkillNode> skills = new();

    private Dictionary<string, SkillNode> skillDict = new();

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        foreach (var s in skills)
        {
            if (s.data != null && !skillDict.ContainsKey(s.data.skillId))
                skillDict.Add(s.data.skillId, s);
        }
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => PointCounter.instance != null);
        //skillPoints = PointCounter.instance.point;
    }


    public bool IsSkillUnlocked(string id)
    {
        return MetaBuffManager.instance.IsUnlocked(id);
    }

    public bool IsQuestUnlocked(string id)
    {
        if (!skillDict.ContainsKey(id))
            return false;

        var node = skillDict[id];

        if (string.IsNullOrEmpty(node.data.requiredQuestId))
            return true;

        return QuestManager.instance != null &&
               QuestManager.instance
                   .IsQuestCompletedById(node.data.requiredQuestId);
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

        if (!PointCounter.instance.SpendPoint(node.data.pointRequire))
            return false;

        MetaBuffManager.instance.Unlock(id);

        OnSkillTreeChanged?.Invoke();
        return true;
    }

    public void SaveBuffData(GameProgress progress)
    {
        if (progress == null) return;

        progress.unlockedBuffsJson =
            MetaBuffManager.instance.GetSaveJson();
    }

    public void LoadBuffData(GameProgress progress)
    {
        if (progress == null) return;

        MetaBuffManager.instance.LoadFromJson(
            progress.unlockedBuffsJson
        );

        OnSkillTreeChanged?.Invoke();
    }
}
