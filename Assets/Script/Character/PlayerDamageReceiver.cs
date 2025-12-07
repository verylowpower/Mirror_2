using UnityEngine;

public class PlayerDamageReceiver : MonoBehaviour
{
    private PlayerHealth health;

    private void Awake()
    {
        health = GetComponent<PlayerHealth>();
    }

    public void TakeDamage(int amount)
    {
        health.TakeDamage(amount);
    }
}
