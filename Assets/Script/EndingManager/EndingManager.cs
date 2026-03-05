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

        bool allQuestDone = overrideQuestCondition
            ? forceAllQuestDone
            : QuestManager.instance.AreAllQuestsCompleted();

        bool allBuffBought = overrideBuffCondition
            ? forceAllBuffBought
            : MetaBuffManager.instance.AreAllBuffsUnlocked();

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
            //CleanupPersistentObjects();
        }
    }

    void ShowEnding(EndingType type)
    {
        Debug.Log("ENDING: " + type.ToString());

        switch (type)
        {
            case EndingType.Bad:
                CleanupPersistentObjects();
                SceneManager.LoadScene(badEndingScene);
                break;

            case EndingType.Good:
                CleanupPersistentObjects();
                SceneManager.LoadScene(goodEndingScene);
                break;

            case EndingType.Best:
                CleanupPersistentObjects();
                SceneManager.LoadScene(bestEndingScene);
                break;
        }
    }

    private void CleanupPersistentObjects()
    {
        var persistents = FindObjectsOfType<PersistentObject>();

        foreach (var obj in persistents)
        {
            if (obj.GetComponent<MetaBuffManager>() != null)
                continue;

            if (obj.GetComponent<SaveLoadManager>() != null)
                continue;

            Destroy(obj.gameObject);
        }
    }
}