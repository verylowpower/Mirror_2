using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightningBulletBuff : IBulletBuff
{
    private int chainTargets;
    private int chainDamage;
    private float chainRadius = 4f;

    public LightningBulletBuff(int targets, int dmg)
    {
        chainTargets = targets;
        chainDamage = dmg;
    }

    public void Apply(Enemy enemy)
    {
        enemy.StartCoroutine(ChainLightning(enemy));
    }

    private IEnumerator ChainLightning(Enemy startEnemy)
    {
        List<Enemy> hitEnemies = new List<Enemy>();
        Enemy current = startEnemy;
        hitEnemies.Add(current);

        for (int i = 0; i < chainTargets; i++)
        {
            Enemy next = FindClosestEnemy(current.transform.position, hitEnemies);

            if (next == null) break;

            next.ChangeHealth(chainDamage);

            Color original = next.spriteRender.color;
            next.spriteRender.color = Color.yellow;
            yield return new WaitForSeconds(0.1f);
            next.spriteRender.color = original;

            hitEnemies.Add(next);
            current = next;

            yield return new WaitForSeconds(0.05f);
        }
    }

    private Enemy FindClosestEnemy(Vector2 origin, List<Enemy> excluded)
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        Enemy best = null;
        float bestDist = Mathf.Infinity;

        foreach (var e in enemies)
        {
            if (excluded.Contains(e)) continue;

            float dist = Vector2.Distance(origin, e.transform.position);
            if (dist < chainRadius && dist < bestDist)
            {
                best = e;
                bestDist = dist;
            }
        }

        return best;
    }
}
