using UnityEngine;

[System.Serializable]
public class SkillNode
{
    public SkillNodeData data;
    public bool isUnlocked;
    public bool questUnlocked;

    public bool CanBuy()
    {
        var mgr = SkillTreeManager.instance;

        bool questUnlocked = MetaBuffManager.instance.IsQuestUnlocked(data.skillId);

        Debug.Log($"[Skill Check] Skill: {data.skillId}");

        if (!questUnlocked)
        {
            Debug.Log("Quest not completed");
            return false;
        }

        if (isUnlocked)
        {
            Debug.Log("Already unlocked");
            return false;
        }

        foreach (var req in data.requiredSkills)
        {
            if (!mgr.IsSkillUnlocked(req))
            {
                Debug.Log($"Missing required skill: {req}");
                return false;
            }
        }

        bool enoughPoint = PointCounter.instance.point >= data.pointRequire;

        if (!enoughPoint)
        {
            Debug.Log("Not enough points");
            return false;
        }

        Debug.Log("Can buy");
        return true;
    }
}