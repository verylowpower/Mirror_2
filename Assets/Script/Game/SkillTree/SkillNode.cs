[System.Serializable]
public class SkillNode
{
    public SkillNodeData data;

    public bool questUnlocked;   // ← GIỮ CÁI NÀY
    // public bool isUnlocked;   ← XÓA CÁI NÀY

    public bool CanBuy()
    {
        if (!SkillTreeManager.instance.IsQuestUnlocked(data.skillId))
            return false;

        //if (isUnlocked) return false;

        foreach (var req in data.requiredSkills)
        {
            if (!SkillTreeManager.instance.IsSkillUnlocked(req))
                return false;
        }

        return PointCounter.instance.point >= data.pointRequire;
    }
}