using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider expBar;
    [SerializeField] private GameObject deathScreen;

    private PlayerHealth health;
    private PlayerExperience exp;

    private void Awake()
    {
        health = GetComponent<PlayerHealth>();
        exp = GetComponent<PlayerExperience>();
    }

    private void OnEnable()
    {
        health.OnHealthChanged += UpdateHealthUI;
        health.OnDeath += ShowDeathScreen;

        exp.OnExpChanged += UpdateExpUI;
        exp.OnLevelUp += (_) => { /* show level up UI */ };
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= UpdateHealthUI;
        health.OnDeath -= ShowDeathScreen;
        exp.OnExpChanged -= UpdateExpUI;
    }

    void UpdateHealthUI(int cur, int max)
    {
        healthBar.value = (float)cur / max;
    }

    void UpdateExpUI(long cur, long max)
    {
        expBar.value = (float)cur / max;
    }

    void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
    }
}
