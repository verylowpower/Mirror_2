using UnityEngine;
using System;

public class PointCounter : MonoBehaviour
{
    public static PointCounter instance;
    public int point;

    public event Action OnPointChanged;

    void Awake()
    {
        instance = this;
    }

    public void AddPoint(int value)
    {
        point += value;
        OnPointChanged?.Invoke();
    }
}
