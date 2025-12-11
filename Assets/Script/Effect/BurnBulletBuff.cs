using UnityEngine;
using System.Collections;

public class BurnBulletBuff : IBulletBuff
{
    private int burnDamage;
    private float duration;
    private float tickInterval = 0.5f;

    public BurnBulletBuff(int dmgPerTick, float dur)
    {
        burnDamage = dmgPerTick;
        duration = dur;
    }

    public void Apply(Enemy enemy)
    {
        enemy.StartCoroutine(DoBurn(enemy));
    }

    private IEnumerator DoBurn(Enemy enemy)
    {
        float elapsed = 0f;

        Color originalColor = enemy.spriteRender.color;
        enemy.spriteRender.color = new Color(1f, 0.4f, 0.1f);

        while (elapsed < duration)
        {
            enemy.ChangeHealth(burnDamage);
            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }

        enemy.spriteRender.color = originalColor;
    }
}
