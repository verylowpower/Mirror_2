using System.Collections.Generic;

public class QuestRuntime
{
    public QuestData data;

    Dictionary<QuestCondition, int> progress =
        new Dictionary<QuestCondition, int>();

    public QuestRuntime(QuestData data)
    {
        this.data = data;

        UnityEngine.Debug.Log(
            $"QuestRuntime created: {data.questId} | condition count: {data.conditions.Count}");

        foreach (var c in data.conditions)
            progress[c] = 0;
    }


    public List<int> GetProgressList()
    {
        List<int> list = new List<int>();

        foreach (var c in data.conditions)
        {
            list.Add(progress[c]);
        }

        return list;
    }

    public void SetProgressList(List<int> list)
    {
        if (list == null) return;

        for (int i = 0; i < data.conditions.Count; i++)
        {
            if (i >= list.Count) break;

            var condition = data.conditions[i];
            progress[condition] = list[i];
        }
    }


    public void AddProgress(QuestType type, string targetId, int amount = 1)
    {
        foreach (var c in data.conditions)
        {
            UnityEngine.Debug.Log(
                $"Compare: type {c.type} vs {type} | target '{c.targetId}' vs '{targetId}'");

            if (c.type == type && c.targetId == targetId)
            {
                progress[c] += amount;

                UnityEngine.Debug.Log(
                    $"Progress now: {progress[c]}/{c.requiredAmount}");
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
