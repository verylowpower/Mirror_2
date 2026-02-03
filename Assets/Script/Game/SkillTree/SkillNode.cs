[System.Serializable]
public class SkillNode
{
    public SkillNodeData data;

    public bool questUnlocked;
    public bool isUnlocked;

    public bool CanBuy()
    {
        if (!questUnlocked) return false;
        if (isUnlocked) return false;

        foreach (var req in data.requiredSkills)
        {
            if (!SkillTreeManager.instance.IsSkillUnlocked(req))
                return false;
        }

        return SkillTreeManager.instance.skillPoints >= data.pointRequire;
    }
}
