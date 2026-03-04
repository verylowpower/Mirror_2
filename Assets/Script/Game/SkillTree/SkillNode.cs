using UnityEngine;

[System.Serializable]
public class SkillNode
{
    public SkillNodeData data;
    public bool isUnlocked;

    public bool CanBuy()
    {
        var mgr = SkillTreeManager.instance;

        bool questUnlocked = mgr.IsQuestUnlocked(data.skillId);

        Debug.Log($"[Skill Check] Skill: {data.skillId}");
        // Debug.Log($"   Required Quest: {data.requiredQuestId}");
        // Debug.Log($"   Quest Completed: {questUnlocked}");
        // Debug.Log($"   IsUnlocked: {isUnlocked}");
        // Debug.Log($"   Player Point: {PointCounter.instance.point}");
        // Debug.Log($"   Require Point: {data.pointRequire}");

        if (!questUnlocked)
        {
            Debug.Log("   ❌ Cannot buy: Quest not completed");
            return false;
        }

        if (isUnlocked)
        {
            Debug.Log("   ❌ Cannot buy: Already unlocked");
            return false;
        }

        foreach (var req in data.requiredSkills)
        {
            if (!mgr.IsSkillUnlocked(req))
            {
                Debug.Log($"   ❌ Missing required skill: {req}");
                return false;
            }
        }

        bool enoughPoint = PointCounter.instance.point >= data.pointRequire;

        if (!enoughPoint)
        {
            Debug.Log("   ❌ Not enough points");
            return false;
        }

        Debug.Log("   ✅ Can buy");
        return true;
    }
}