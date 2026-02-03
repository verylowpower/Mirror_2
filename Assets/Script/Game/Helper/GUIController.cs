using UnityEngine;
using TMPro;
using System.Collections;

public class GUIController : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI pointText;
    public TextMeshProUGUI waveText;

    private float time;

    IEnumerator Start()
    {
        yield return new WaitUntil(() =>
            PlayerExperience.instance != null &&
            GameController.instance != null
        );

        PlayerExperience.instance.OnLevelUp += UpdateLevelText;
        GameController.instance.TimeChange += UpdateGameTime;
        PointCounter.instance.OnPointChanged += UpdatePointText;
        UpdatePointText();


        Room.OnWaveStarted += UpdateWaveText;

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
            PointCounter.instance.OnPointChanged -= UpdatePointText;
        }

        Room.OnWaveStarted -= UpdateWaveText;
    }


    private void UpdateLevelText(int newLevel)
    {
        levelText.text = $"Level: {newLevel}";
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
        pointText.text = $"Score: {PointCounter.instance.point}";
    }

    private Coroutine waveRoutine;

    private void UpdateWaveText(int currentWave, int totalWave)
    {
        if (waveRoutine != null)
            StopCoroutine(waveRoutine);

        waveRoutine = StartCoroutine(ShowWaveText(currentWave, totalWave));
    }

    private IEnumerator ShowWaveText(int currentWave, int totalWave)
    {
        waveText.text = $"WAVE {currentWave}/{totalWave}";
        waveText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        waveText.gameObject.SetActive(false);
    }

}
