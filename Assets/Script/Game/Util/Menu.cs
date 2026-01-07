using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    private void PlayClick()
    {
        if (audioSource && clickSound)
            audioSource.PlayOneShot(clickSound);
    }

    public void StartButton()
    {
        PlayClick();
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void MenuButton()
    {
        PlayClick();
        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
    }

    public void ResumeButton()
    {
        PlayClick();
        SceneManager.UnloadSceneAsync("Menu");
        Pause.instance.isPaused = false;
        Pause.instance.pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitButton()
    {
        PlayClick();
        Application.Quit();
    }
}
