using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Bars")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider expBar;


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

        exp.OnExpChanged += UpdateExpUI;
        exp.OnLevelUp += OnLevelUpUI;

        UpdateHealthUI(health.currentHealth, health.maxHealth);
        UpdateExpUI(exp.GetExp(), ExpTable.GetExpRequired(exp.GetLevel()));
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= UpdateHealthUI;


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


}
