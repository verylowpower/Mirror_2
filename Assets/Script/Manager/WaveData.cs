using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject prefab;
    public int count;
}

[CreateAssetMenu(fileName = "WaveData", menuName = "Game/Wave Data")]
public class WaveData : ScriptableObject
{
    public List<EnemySpawnInfo> groups;
}
