using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    //public bool startQuest;
    List<QuestRuntime> activeQuests = new();
    HashSet<string> completedQuestIds = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartQuest(QuestData data)
    {
        if (completedQuestIds.Contains(data.questId))
            return;

        if (activeQuests.Exists(q => q.data.questId == data.questId))
            return;

        activeQuests.Add(new QuestRuntime(data));
        Debug.Log($"🟡 Quest started: {data.questName}");
    }

    public void NotifyEvent(
        QuestType type,
        string targetId,
        int amount = 1
    )
    {
        for (int i = activeQuests.Count - 1; i >= 0; i--)
        {
            var q = activeQuests[i];
            q.AddProgress(type, targetId, amount);

            if (q.IsCompleted())
                CompleteQuest(q);
        }
    }

    void CompleteQuest(QuestRuntime quest)
    {
        Debug.Log($"✅ Quest completed: {quest.data.questName}");

        completedQuestIds.Add(quest.data.questId);
        activeQuests.Remove(quest);

        QuestRewardSystem.Instance?.GiveReward(quest.data);

        if (quest.data.autoStartNext && quest.data.nextQuest != null)
        {
            StartQuest(quest.data.nextQuest);
        }
    }
}
