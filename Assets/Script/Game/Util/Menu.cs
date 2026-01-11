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

    [Header("First Selected Button")]
    public Button firstButton;

    private PlayerInputAction input;

    private void Awake()
    {
        input = new PlayerInputAction();
        input.Input.Confirm.performed += OnConfirm;
    }

    private void OnEnable()
    {
        input.Enable();

        // Auto focus button đầu tiên
        if (firstButton != null)
            EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void PlayClick()
    {
        if (audioSource && clickSound)
            audioSource.PlayOneShot(clickSound);
    }

    // ===== GAMEPAD CONFIRM =====
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

    // ===== BUTTON EVENTS =====
    public void StartButton()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
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
