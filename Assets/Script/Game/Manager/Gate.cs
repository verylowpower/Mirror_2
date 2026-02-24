using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // SaveLoadManager save = SaveLoadManager.instance;
        // if (save == null) return;

        // GameProgress progress = save.LoadOrCreateDefault(1);

        // PlayerController player = collision.GetComponent<PlayerController>();
        // if (player != null)
        // {
        //     progress.playerHealth = PlayerHealth.instance.currentHealth;
        //     progress.moveSpeed = player.moveSpeed;
        //     progress.meleeDamage = PlayerAttack.instance.meleeDamage;
        // }
        // save.SaveSceneEntrySnapshot(progress);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
