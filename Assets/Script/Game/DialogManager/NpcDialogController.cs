using UnityEngine;

public class NPCDialog : MonoBehaviour
{
    [SerializeField] private DialogData[] dialogs;

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
        foreach (var dialog in dialogs)
        {
            if (dialog.relatedQuest == null)
                return dialog;

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
        }

        if (dialog.startQuestOnEnd && dialog.relatedQuest != null)
        {
            QuestManager.instance.StartQuest(dialog.relatedQuest);
        }
    }
}