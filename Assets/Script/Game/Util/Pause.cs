using UnityEngine;

public class Pause : MonoBehaviour
{
    public static Pause instance;
    public GameObject pauseMenu;
    public bool isPaused;

    private SaveLoadManager saveLoad;

    void Awake()
    {
        instance = this;
        saveLoad = FindObjectOfType<SaveLoadManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);


    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }


}
