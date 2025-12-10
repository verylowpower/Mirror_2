using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartButton()
    {
        //SaveLoadManager.DeleteSave();

        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    // public void ContinueButton()
    // {
    //     GameProgress data = SaveLoadManager.Load();
    //     if (data != null)
    //     {
    //         SceneManager.LoadSceneAsync(data.currentLevel).completed += (op) =>
    //         {
    //             PlayerController.instance.transform.position = data.playerPosition;
    //             PlayerHealth.instance.currentHealth = data.playerHealth;   
    //         };

    //         Time.timeScale = 1f;
    //     }
    //     else
    //     {
    //         Debug.Log("[Menu] No save data found, starting new game.");
    //         StartButton();
    //     }
    // }

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
