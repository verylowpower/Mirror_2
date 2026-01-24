using System.Collections.Generic;

public class QuestRuntime
{
    public QuestData data;

    Dictionary<QuestCondition, int> progress =
        new Dictionary<QuestCondition, int>();

    public QuestRuntime(QuestData data)
    {
        this.data = data;

        foreach (var c in data.conditions)
            progress[c] = 0;
    }

    public void AddProgress(QuestType type, string targetId, int amount = 1)
    {
        foreach (var c in data.conditions)
        {
            if (c.type == type && c.targetId == targetId)
            {
                progress[c] += amount;
            }
        }
    }

    public bool IsCompleted()
    {
        foreach (var c in data.conditions)
        {
            if (progress[c] < c.requiredAmount)
                return false;
        }
        return true;
    }
}
