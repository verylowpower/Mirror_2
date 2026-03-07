using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
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
            System.IO.Path.Combine(Application.persistentDataPath, "GameProgress.db"));

        loadButton.gameObject.SetActive(hasSave);
    }

    private void PlayClick()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlayClick();
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
        CleanupPersistentObjects();
        AudioListener.pause = false;
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

   public void ResumeButton()
{
    if (Pause.instance != null)
    {
        Pause.instance.ResumeGame();
    }

    if (SceneManager.GetSceneByName("Menu").isLoaded)
    {
        SceneManager.UnloadSceneAsync("Menu");
    }
}
    public void QuitButton()
    {
        Time.timeScale = 1f;
        Application.Quit();
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