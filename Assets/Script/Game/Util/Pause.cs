using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public static Pause instance;

    public GameObject pausePanel;   // Panel UI pause
    public bool isPaused;

    [Header("Audio Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        pausePanel.SetActive(false);

        masterSlider.value = AudioManager.instance.GetMasterVolume();
        musicSlider.value = AudioManager.instance.GetMusicVolume();
        sfxSlider.value = AudioManager.instance.GetSFXVolume();

        masterSlider.onValueChanged.AddListener(AudioManager.instance.SetMasterVolume);
        musicSlider.onValueChanged.AddListener(AudioManager.instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.instance.SetSFXVolume);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        if (isPaused) return;

        isPaused = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        isPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        pausePanel.SetActive(false);
    }

    public void BackToMainMenu()
    {
        if (!SceneManager.GetSceneByName("Menu").isLoaded)
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        }
    }
    
    public void QuitButton()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}