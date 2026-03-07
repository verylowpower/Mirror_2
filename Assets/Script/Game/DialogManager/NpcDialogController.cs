using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NPCDialog : MonoBehaviour
{
    [SerializeField] private string npcId;

    private DialogData[] allDialogs;
    HashSet<DialogData> shownNormalDialogs = new HashSet<DialogData>();

    void Awake()
    {
        LoadDialogs();
    }

    void LoadDialogs()
    {
        allDialogs = Resources.LoadAll<DialogData>($"Dialogs/{npcId}");

        Debug.Log($"Loaded dialogs: {allDialogs.Length}");

        foreach (var d in allDialogs)
        {
            Debug.Log($"Dialog: {d.name}");
        }

        if (allDialogs == null || allDialogs.Length == 0)
        {
            Debug.LogWarning($"No dialogs found for NPC: {npcId}");
        }
    }

    public void Interact()
    {
        DialogData dialog = GetValidDialog();

        if (dialog == null)
        {
            Debug.Log("No valid dialog found.");
            return;
        }

        DialogUI.Instance.Show(
            dialog.sentences,
            () => OnDialogFinish(dialog)
        );
    }

    DialogData GetValidDialog()
    {
        Debug.Log($"Total dialogs: {allDialogs.Length}");

        foreach (var d in allDialogs)
        {
            Debug.Log($"Check dialog {d.name} | quest = {d.relatedQuest}");
        }

        var normalDialog = allDialogs.FirstOrDefault(d => d.questState == DialogQuestState.Normal &&
        !shownNormalDialogs.Contains(d));

        if (normalDialog != null)
        {
            Debug.Log("Returning NORMAL dialog");
            return normalDialog;
        }

        Debug.Log("No normal dialog");

        int currentQuestIndex = GetCurrentQuestIndex();
        string questId = $"Q{currentQuestIndex:D2}";

        Debug.Log($"Current quest id: {questId}");

        var questDialogs = allDialogs.Where(d => d.relatedQuest != null && d.relatedQuest.questId == questId).ToArray();

        if (questDialogs.Length == 0)
            return null;

        var quest = questDialogs[0].relatedQuest;

        bool hasQuest = QuestManager.instance.HasQuest(quest);
        bool completed = QuestManager.instance.IsQuestCompleted(quest);
        bool completedDialogShown = QuestManager.instance.IsCompletedDialogShown(quest);

        if (!hasQuest && !completed)
        {
            return questDialogs.FirstOrDefault(d =>
                d.questState == DialogQuestState.BeforeQuest);
        }

        if (hasQuest && !completed)
        {
            return questDialogs.FirstOrDefault(d =>
                d.questState == DialogQuestState.InProgress);
        }

        if (completed && !completedDialogShown)
        {
            return questDialogs.FirstOrDefault(d =>
                d.questState == DialogQuestState.Completed);
        }

        return null;
    }

    int GetCurrentQuestIndex()
    {
        int index = 0;

        while (true)
        {
            string questId = $"Q{index:D2}";

            bool completed = QuestManager.instance.IsQuestCompletedById(questId);

            var quest = Resources.Load<QuestData>($"Quests/{questId}");

            if (quest == null)
                return index;

            bool dialogShown =
                QuestManager.instance.IsCompletedDialogShown(quest);

            if (!completed || !dialogShown)
                return index;

            index++;
        }
    }

    void OnDialogFinish(DialogData dialog)
    {
        if (dialog.questState == DialogQuestState.Normal)
        {
            shownNormalDialogs.Add(dialog);
            return;
        }

        if (dialog.questState == DialogQuestState.Completed)
        {
            QuestManager.instance.MarkCompletedDialogShown(dialog.relatedQuest);

            if (!string.IsNullOrEmpty(dialog.relatedQuest.rewardSkillId))
            {
                MetaBuffManager.instance.UnlockByQuest(dialog.relatedQuest.rewardSkillId);
                SkillTreeManager.instance.SyncQuestUnlock();
            }
        }

        if (dialog.startQuestOnEnd && dialog.relatedQuest != null)
        {
            QuestManager.instance.StartQuest(dialog.relatedQuest);
        }
    }


}