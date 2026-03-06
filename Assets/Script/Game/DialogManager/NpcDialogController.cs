using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NPCDialog : MonoBehaviour
{
    [SerializeField] private string npcId;

    private DialogData[] allDialogs;
    private int currentQuestIndex = 1;

    void Awake()
    {
        LoadDialogs();
    }

    void LoadDialogs()
    {
        allDialogs = Resources.LoadAll<DialogData>($"Dialogs/{npcId}");

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
        string questId = $"Q{currentQuestIndex:D2}";

        var questDialogs = allDialogs
            .Where(d => d.relatedQuest != null &&
                        d.relatedQuest.questId == questId)
            .ToArray();

        foreach (var dialog in questDialogs)
        {
            var quest = dialog.relatedQuest;

            bool hasQuest = QuestManager.instance.HasQuest(quest);
            bool completed = QuestManager.instance.IsQuestCompleted(quest);
            bool completedDialogShown =
                QuestManager.instance.IsCompletedDialogShown(quest);

            switch (dialog.questState)
            {
                case DialogQuestState.BeforeQuest:
                    if (!hasQuest && !completed)
                        return dialog;
                    break;

                case DialogQuestState.InProgress:
                    if (hasQuest && !completed)
                        return dialog;
                    break;

                case DialogQuestState.Completed:
                    if (completed && !completedDialogShown)
                        return dialog;
                    break;
            }
        }

        return null;
    }

    void OnDialogFinish(DialogData dialog)
    {
        if (dialog.questState == DialogQuestState.Completed)
        {
            QuestManager.instance.MarkCompletedDialogShown(dialog.relatedQuest);
            currentQuestIndex++;
        }

        if (dialog.startQuestOnEnd && dialog.relatedQuest != null)
        {
            QuestManager.instance.StartQuest(dialog.relatedQuest);
        }
    }
}