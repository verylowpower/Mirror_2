using UnityEngine;
using TMPro;
using System.Collections;

public class GUIController : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI pointText;
    public TextMeshProUGUI waveText;

    public TextMeshProUGUI currentWaveText;

    private Coroutine waveRoutine;

    IEnumerator Start()
    {
        yield return new WaitUntil(() =>
            PlayerExperience.instance != null &&
            GameController.instance != null &&
            PointCounter.instance != null
        );
        PlayerExperience.instance.OnLevelUp += UpdateLevelText;
        GameController.instance.TimeChange += UpdateGameTime;
        PointCounter.instance.OnPointChanged += UpdatePointText;
        Room.OnWaveStarted += UpdateWaveText;

        UpdateLevelText(PlayerExperience.instance.GetLevel());
        UpdateGameTime();
        UpdatePointText();

        if (Room.instance != null)
        {
            int total = Room.instance.TotalWave;
            int currentDisplay = Room.instance.currentWave + 1;

            if (Room.instance.currentWave == 0)
                currentDisplay = 0;

            currentWaveText.text = $"Wave: {currentDisplay}/{total}";
        }
    }

    void OnDestroy()
    {
        if (PlayerExperience.instance != null)
            PlayerExperience.instance.OnLevelUp -= UpdateLevelText;

        if (GameController.instance != null)
            GameController.instance.TimeChange -= UpdateGameTime;

        if (PointCounter.instance != null)
            PointCounter.instance.OnPointChanged -= UpdatePointText;

        Room.OnWaveStarted -= UpdateWaveText;
    }

    private void UpdateLevelText(int newLevel)
    {
        if (levelText != null)
            levelText.text = $"Level: {newLevel}";
    }

    private void UpdateGameTime()
    {
        if (GameController.instance == null || timeText == null)
            return;

        float time = GameController.instance.inGameTime;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timeText.text = $"{minutes:00}:{seconds:00}";
    }
    private void UpdatePointText()
    {
        if (PointCounter.instance == null || pointText == null)
            return;

        pointText.text = $"Score: {PointCounter.instance.point}";
    }

    private void UpdateWaveText(int currentWave, int totalWave)
    {
        if (currentWaveText != null)
            currentWaveText.text = $"Wave: {currentWave}/{totalWave}";

        if (waveRoutine != null)
            StopCoroutine(waveRoutine);

        waveRoutine = StartCoroutine(ShowWaveText(currentWave, totalWave));
    }

    private IEnumerator ShowWaveText(int currentWave, int totalWave)
    {
        if (waveText == null)
            yield break;

        waveText.text = $"WAVE {currentWave}/{totalWave}";
        waveText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        waveText.gameObject.SetActive(false);
    }
}