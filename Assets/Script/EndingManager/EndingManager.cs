using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public static EndingManager instance;

    [SerializeField] private string badEndingScene = "BadEnd";
    [SerializeField] private string goodEndingScene = "GoodEnd";
    [SerializeField] private string bestEndingScene = "BestEnd";

    [Header("DEBUG OVERRIDE")]
    [SerializeField] private bool overrideQuestCondition = false;
    [SerializeField] private bool forceAllQuestDone = false;

    [SerializeField] private bool overrideBuffCondition = false;
    [SerializeField] private bool forceAllBuffBought = false;

    public bool isBossFightActive = false;
    [SerializeField] private string scene1Name = "Scene1";

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartBossFight()
    {
        isBossFightActive = true;
    }

    public void EndBossFight()
    {
        isBossFightActive = false;
    }

    public void TriggerPlayerDefeated()
    {
        if (!isBossFightActive) return;

        Debug.Log("BAD ENDING - Player defeated by boss");
        ShowEnding(EndingType.Bad);
    }

    public void TriggerBossDefeated()
    {
        isBossFightActive = false;

        bool allQuestDone;
        bool allBuffBought;

        // DEBUG QUEST
        if (overrideQuestCondition)
        {
            allQuestDone = forceAllQuestDone;
            Debug.Log("DEBUG QUEST OVERRIDE: " + allQuestDone);
        }
        else
        {
            allQuestDone = QuestManager.instance.AreAllQuestsCompleted();
        }

        // DEBUG BUFF
        if (overrideBuffCondition)
        {
            allBuffBought = forceAllBuffBought;
            Debug.Log("DEBUG BUFF OVERRIDE: " + allBuffBought);
        }
        else
        {
            allBuffBought = MetaBuffManager.instance.AreAllBuffsUnlocked();
        }

        Debug.Log("Quest Done: " + allQuestDone);
        Debug.Log("Buff Bought: " + allBuffBought);

        if (allQuestDone && allBuffBought)
        {
            ShowEnding(EndingType.Best);
        }
        else if (allQuestDone || allBuffBought)
        {
            ShowEnding(EndingType.Good);
        }
        else
        {
            SceneManager.LoadScene(scene1Name);
        }
    }

    void ShowEnding(EndingType type)
    {
        Debug.Log("ENDING: " + type);

        CleanupPersistentObjects();

        switch (type)
        {
            case EndingType.Bad:
                SceneManager.LoadScene(badEndingScene);
                break;

            case EndingType.Good:
                SceneManager.LoadScene(goodEndingScene);
                break;

            case EndingType.Best:
                SceneManager.LoadScene(bestEndingScene);
                break;
        }
    }

    private void CleanupPersistentObjects()
    {
        var persistents = FindObjectsOfType<PersistentObject>();

        foreach (var obj in persistents)
        {
            if (obj.GetComponent<MetaBuffManager>() != null) continue;
            if (obj.GetComponent<SaveLoadManager>() != null) continue;

            Destroy(obj.gameObject);
        }
    }
}