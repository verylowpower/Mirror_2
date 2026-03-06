using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    List<QuestRuntime> activeQuests = new();
    HashSet<string> completedQuestIds = new();
    HashSet<string> completedDialogShown = new();

    //bool _allQuestCompleted = false;

    Dictionary<string, QuestData> questLookup =
        new Dictionary<string, QuestData>();

    void Awake()
    {
        Debug.Log("QuestManager Awake: " + GetInstanceID());

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadAllQuestData();
    }

    void LoadAllQuestData()
    {
        QuestData[] all = Resources.LoadAll<QuestData>("Quests");

        foreach (var q in all)
        {
            if (!questLookup.ContainsKey(q.questId))
            {
                questLookup.Add(q.questId, q);
            }
        }

        Debug.Log($"Have {questLookup.Count} quest");
    }

    QuestData GetQuestById(string id)
    {
        if (questLookup.TryGetValue(id, out QuestData data))
            return data;

        return null;
    }

    public void StartQuest(QuestData data)
    {
        Debug.Log("StartQuest instance: " + GetInstanceID());
        if (data == null) return;

        if (completedQuestIds.Contains(data.questId))
            return;

        if (activeQuests.Exists(q => q.data.questId == data.questId))
            return;

        activeQuests.Add(new QuestRuntime(data));
        SaveSystem();

        Debug.Log($"Quest started: {data.questName}");
    }

    public void NotifyEvent(QuestType type, string targetId, int amount = 1)
    {
        Debug.Log("NotifyEvent instance: " + GetInstanceID());
        Debug.Log("Active quest count: " + activeQuests.Count);
        Debug.Log($"NotifyEvent: {type} - {targetId} - {amount}");

        for (int i = activeQuests.Count - 1; i >= 0; i--)
        {
            var quest = activeQuests[i];

            Debug.Log($"Checking quest: {quest.data.questId}");

            quest.AddProgress(type, targetId, amount);

            if (quest.IsCompleted())
                CompleteQuest(quest);
        }
    }

    void CompleteQuest(QuestRuntime quest)
    {
        completedQuestIds.Add(quest.data.questId);
        activeQuests.Remove(quest);

        Debug.Log($"Quest completed: {quest.data.questName}");

        SkillTreeManager.instance.UnlockSkillsFromQuest(quest.data.questId);

        SaveSystem();

        SkillTreeManager.instance?.NotifySkillTreeChanged();
    }

    public void SaveQuestData(GameProgress progress)
    {
        if (progress == null) return;

        QuestSaveWrapper wrapper = new QuestSaveWrapper();
        wrapper.completedQuestIds = new List<string>(completedQuestIds);
        wrapper.activeQuests = new List<QuestRuntimeSaveData>();

        foreach (var quest in activeQuests)
        {
            QuestRuntimeSaveData saveData = new QuestRuntimeSaveData();
            saveData.questId = quest.data.questId;
            saveData.progress = quest.GetProgressList();

            wrapper.activeQuests.Add(saveData);
        }

        string json = JsonUtility.ToJson(wrapper);

        progress.completedQuestsJson = json;
    }

    public void LoadQuestData(GameProgress progress)
    {
        if (progress == null) return;
        if (string.IsNullOrEmpty(progress.completedQuestsJson))
            return;

        QuestSaveWrapper wrapper =
            JsonUtility.FromJson<QuestSaveWrapper>(
                progress.completedQuestsJson
            );

        if (wrapper == null) return;

        activeQuests.Clear();
        completedQuestIds.Clear();
        if (wrapper.completedQuestIds != null)
        {
            foreach (var id in wrapper.completedQuestIds)
                completedQuestIds.Add(id);
        }
        if (wrapper.activeQuests != null)
        {
            foreach (var saveData in wrapper.activeQuests)
            {
                QuestData data = GetQuestById(saveData.questId);
                if (data == null) continue;

                QuestRuntime runtime = new QuestRuntime(data);
                runtime.SetProgressList(saveData.progress);

                activeQuests.Add(runtime);
            }
        }
    }

    void SaveSystem()
    {
        var progress = SaveLoadManager.Instance.GetProgress();
        if (progress == null) return;

        SaveQuestData(progress);

        SaveLoadManager.Instance.SaveGame();
    }

    public bool HasQuest(QuestData data)
    {
        if (data == null) return false;

        return activeQuests.Exists(q => q.data.questId == data.questId);

    }

    public bool IsQuestCompleted(QuestData data)
    {
        if (data == null) return false;

        return completedQuestIds.Contains(data.questId);
    }

    public bool IsQuestActive(QuestData data)
    {
        if (data == null) return false;

        return activeQuests.Exists(q => q.data.questId == data.questId);
    }

    public bool IsCompletedDialogShown(QuestData data)
    {
        if (data == null) return false;
        return completedDialogShown.Contains(data.questId);
    }

    public void MarkCompletedDialogShown(QuestData data)
    {
        if (data == null) return;
        completedDialogShown.Add(data.questId);
    }
    public bool IsQuestCompletedById(string id)
    {
        return completedQuestIds.Contains(id);
    }
    // public bool IsQuestCompletedById(string questId)
    // {
    //     return false;
    // }

    public bool AreAllQuestsCompleted()
    {
        if (questLookup.Count == 0)
            return false;

        return completedQuestIds.Count == questLookup.Count;
    }

    // public void AllQuestCompleted()
    // {
    //     if (questLookup.Count == 0)
    //     {
    //         _allQuestCompleted = true;
    //     }
    // }
}
