using System;
using System.Collections.Generic;

[Serializable]
public class QuestSaveWrapper
{
    public List<string> completedQuestIds;
    public List<QuestRuntimeSaveData> activeQuests;
}

[Serializable]
public class QuestRuntimeSaveData
{
    public string questId;
    public List<int> progress;
}
