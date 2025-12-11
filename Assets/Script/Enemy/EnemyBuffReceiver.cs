using UnityEngine;
using System.Collections.Generic;

public class EnemyBuffReceiver : MonoBehaviour
{
    public void ApplyBuffs(List<IBulletBuff> buffs)
    {
        foreach (var buff in buffs)
        {
            buff.Apply(GetComponent<Enemy>());
        }
    }
}
