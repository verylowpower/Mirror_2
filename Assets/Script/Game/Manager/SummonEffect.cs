using UnityEngine;
using System;

public class SummonEffect : MonoBehaviour
{
    public float summonDuration = 1f;
    private Action onSummonFinished;

    public void Init(Action callback)
    {
        onSummonFinished = callback;
        Invoke(nameof(FinishSummon), summonDuration);
    }

    void FinishSummon()
    {
        onSummonFinished?.Invoke();
        Destroy(gameObject);
    }
}
