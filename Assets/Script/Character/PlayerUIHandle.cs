using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Bars")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider expBar;

    // [Header("Death Screen")]
    // [SerializeField] private GameObject deathScreen;

    private PlayerHealth health;
    private PlayerExperience exp;

    private void Awake()
    {
        health = GetComponent<PlayerHealth>();
        exp = GetComponent<PlayerExperience>();
    }

    private void OnEnable()
    {
        // Health
        health.OnHealthChanged += UpdateHealthUI;
        //health.OnDeath += ShowDeathScreen;

        // EXP
        exp.OnExpChanged += UpdateExpUI;
        exp.OnLevelUp += OnLevelUpUI;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= UpdateHealthUI;
        //health.OnDeath -= ShowDeathScreen;

        exp.OnExpChanged -= UpdateExpUI;
        exp.OnLevelUp -= OnLevelUpUI;
    }

    void UpdateHealthUI(int cur, int max)
    {
        healthBar.value = (float)cur / max;
    }

    void UpdateExpUI(long cur, long max)
    {
        expBar.value = (float)cur / max;
    }

    void OnLevelUpUI(int newLevel)
    {

    }

    // void ShowDeathScreen()
    // {
    //     if (deathScreen != null)
    //         deathScreen.SetActive(true);
    // }
}
