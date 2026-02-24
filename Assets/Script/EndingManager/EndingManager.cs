using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public static EndingManager instance;
    [SerializeField] private string badEndingScene = "BadEnd";
    [SerializeField] private string goodEndingScene = "GoodEnd";
    [SerializeField] private string bestEndingScene = "BestEnd";

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

        bool allQuestDone = QuestManager.instance.AreAllQuestsCompleted();
        bool allBuffBought = MetaBuffManager.instance.AreAllBuffsUnlocked();

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
        Debug.Log("ENDING: " + type.ToString());

        switch (type)
        {
            case EndingType.Bad:
                SceneManager.LoadScene(badEndingScene);
                break;

            // case EndingType.Good:
            //     SceneManager.LoadScene(goodEndingScene);
            //     break;

            // case EndingType.Best:
            //     SceneManager.LoadScene(bestEndingScene);
            //     break;
        }
    }
}