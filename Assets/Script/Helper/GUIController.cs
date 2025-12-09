using UnityEngine;
using TMPro;

public class GUIController : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI pointText;

    private float time;

    void Start()
    {
        PlayerExperience.instance.OnLevelUp += UpdateLevelText;

        GameController.instance.TimeChange += UpdateGameTime;
        GameController.instance.KilledEnemy += UpdatePointText;

        UpdateLevelText(PlayerExperience.instance.GetLevel());
        UpdatePointText();
        UpdateGameTime();
    }

    void OnDestroy()
    {
        if (PlayerExperience.instance != null)
            PlayerExperience.instance.OnLevelUp -= UpdateLevelText;

        if (GameController.instance != null)
        {
            GameController.instance.TimeChange -= UpdateGameTime;
            GameController.instance.KilledEnemy -= UpdatePointText;
        }
    }

    private void UpdateLevelText(int newLevel)
    {
        levelText.text = "Level: " + newLevel;
    }

    private void UpdateGameTime()
    {
        time = GameController.instance.inGameTime;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timeText.text = $"{minutes:00}:{seconds:00}";
    }

    private void UpdatePointText()
    {
        pointText.text = "Score: " + GameController.instance.enemyKilled;
    }
}
