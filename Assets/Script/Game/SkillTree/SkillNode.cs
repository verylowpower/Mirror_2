[System.Serializable]
public class SkillNode
{
    public SkillNodeData data;

    public bool questUnlocked;   // mở khi hoàn thành quest
    public bool isUnlocked;      // đã mua chưa

    public bool CanBuy()
    {
        if (!questUnlocked) return false;
        if (isUnlocked) return false;

        foreach (var req in data.requiredSkills)
        {
            if (!SkillTreeManager.instance.IsSkillUnlocked(req))
                return false;
        }

        return PointCounter.instance.point >= data.pointRequire;
    }
}