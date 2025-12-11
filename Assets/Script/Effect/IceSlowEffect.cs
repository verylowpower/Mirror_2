using UnityEngine;

public class IceBulletBuff : IBulletBuff
{
    private float slowMultiplier;
    private float duration;

    public IceBulletBuff(float slow, float time)
    {
        slowMultiplier = slow;
        duration = time;
    }

    public void Apply(Enemy target)
    {
        var handler = target.GetComponent<EnemyIceEffect>();
        if (handler != null)
        {
            handler.Trigger(slowMultiplier, duration);
        }
    }
}
