using UnityEngine;

public class NPCDialog : MonoBehaviour
{
    [SerializeField] DialogData dialog;

    bool talked = false;

    public void Interact()
    {
        if (talked) return;

        DialogUI.Instance.Show(
            dialog.sentences,
            OnDialogFinish
        );
        OnDialogFinish();
        talked = true;
    }

    void OnDialogFinish()
    {
        if (dialog.startQuest != null)
        {
            QuestManager.instance.StartQuest(dialog.startQuest);
        }

        if (dialog.completeQuestOnEnd)
        {
            QuestManager.instance.NotifyEvent(
                QuestType.TalkToNPC,
                dialog.dialogId,
                1
            );
        }
    }
}
