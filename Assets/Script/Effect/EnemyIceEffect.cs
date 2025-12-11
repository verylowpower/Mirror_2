using UnityEngine;
using System.Collections;

public class EnemyIceEffect : MonoBehaviour
{
    private Enemy enemy;
    private SpriteRenderer sr;

    private float originalSpeed;
    private Color originalColor;

    private Coroutine routine;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        sr = GetComponent<SpriteRenderer>();

        originalSpeed = enemy.moveSpeed;
        originalColor = sr.color;
    }

    public void Trigger(float multiplier, float duration)
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(DoEffect(multiplier, duration));
    }

    private IEnumerator DoEffect(float multiplier, float duration)
    {
        enemy.moveSpeed = originalSpeed * multiplier;
        sr.color = Color.cyan;

        yield return new WaitForSeconds(duration);

        enemy.moveSpeed = originalSpeed;
        sr.color = originalColor;
    }
}
