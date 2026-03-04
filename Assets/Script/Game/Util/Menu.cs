using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    [Header("Buttons")]
    public Button firstButton;
    public Button loadButton;

    private PlayerInputAction input;

    private void Awake()
    {
        input = new PlayerInputAction();
        input.Input.Confirm.performed += OnConfirm;
    }

    private void OnEnable()
    {
        input.Enable();

        if (firstButton != null)
            EventSystem.current.SetSelectedGameObject(firstButton.gameObject);

        UpdateLoadButtonVisibility();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void UpdateLoadButtonVisibility()
    {
        if (loadButton == null)
            return;

        bool hasSave = System.IO.File.Exists(
            System.IO.Path.Combine(
                Application.persistentDataPath,
                "GameProgress.db"));

        loadButton.gameObject.SetActive(hasSave);
    }

    private void PlayClick()
    {
        if (audioSource && clickSound)
            audioSource.PlayOneShot(clickSound);
    }

    private void OnConfirm(InputAction.CallbackContext ctx)
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (selected == null) return;

        Button btn = selected.GetComponent<Button>();
        if (btn != null)
        {
            PlayClick();
            btn.onClick.Invoke();
        }
    }


    public void StartButton()
    {
        PlayClick();
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void LoadButton()
    {
        PlayClick();

        if (SaveLoadManager.Instance == null)
        {
            Debug.LogError("SaveLoadManager not found!");
            return;
        }

        Time.timeScale = 1f;
        SaveLoadManager.Instance.LoadGame();
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
    }

    public void ResumeButton()
    {
        SceneManager.UnloadSceneAsync("Menu");
        Pause.instance.isPaused = false;
        Pause.instance.pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}