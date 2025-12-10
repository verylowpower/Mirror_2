using UnityEngine;
using System;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    public static PlayerHealth instance;
    public event Action OnDeath;
    public event Action<int, int> OnHealthChanged;

    [SerializeField] public int maxHealth = 100;
    [SerializeField] private float iFrameTime = 0.3f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.05f;
    public bool isKilled = false;

    [SerializeField] public int currentHealth;
    private bool isIFrame = false;
    private Color originalColor;

    void Awake() => instance = this;
    private void Start()
    {
        currentHealth = maxHealth;
        originalColor = spriteRenderer.color;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int dmg)
    {
        if (isIFrame) return;

        currentHealth = Mathf.Clamp(currentHealth - dmg, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log("Player HP = " + currentHealth);

        StartCoroutine(IFrameRoutine());
        StartCoroutine(FlashEffect());
        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
            DeathHandler.instance.HandlePlayerDeath();
        }
    }

    IEnumerator IFrameRoutine()
    {
        isIFrame = true;
        yield return new WaitForSeconds(iFrameTime);
        isIFrame = false;
    }

    IEnumerator FlashEffect()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    public void KillPlayer()
    {
        //isKilled = true;
        DeathHandler.instance.HandlePlayerDeath();
        Destroy(gameObject);
    }
}
