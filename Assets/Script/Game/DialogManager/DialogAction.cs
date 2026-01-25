public enum DialogActionType
{
    None,
    StartQuest,
    GiveReward
}

[System.Serializable]
public class DialogAction
{
    public DialogActionType type;
    public QuestData questToStart;

    public void Execute()
    {
        switch (type)
        {
            case DialogActionType.StartQuest:
                if (questToStart != null)
                    QuestManager.instance.StartQuest(questToStart);
                break;
        }
    }
}
