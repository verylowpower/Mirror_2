using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{
    public static Pause instance;

    public GameObject pausePanel;
    public bool isPaused;

    [Header("First Selected Button")]
    public Button firstButton;

    [Header("Audio Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    PlayerInputAction input;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        input = new PlayerInputAction();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Input.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        input.Input.Pause.performed -= OnPause;
        input.Disable();
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

    void OnPause(InputAction.CallbackContext ctx)
    {
        if (!isPaused)
            PauseGame();
        else
            ResumeGame();
    }

    public void PauseGame()
    {
        if (isPaused) return;

        isPaused = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;

        pausePanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
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
            SceneManager.LoadScene("Menu");
        }
    }

    public void QuitButton()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}