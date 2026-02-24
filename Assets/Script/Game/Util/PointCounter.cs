using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PointCounter : MonoBehaviour
{
    public static PointCounter instance;

    public int point;

    public event Action OnPointChanged;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SyncFromDatabase();
    }

    void SyncFromDatabase()
    {
        if (SaveLoadManager.Instance == null)
            return;

        var progress = SaveLoadManager.Instance.GetProgress();
        if (progress == null)
            return;

        point = progress.playerPoint;
        OnPointChanged?.Invoke();

        Debug.Log("Point synced: " + point);
    }

    public void AddPoint(int amount)
    {
        point += amount;
        OnPointChanged?.Invoke();
        SaveLoadManager.Instance.UpdatePoint(point);
    }

    public bool SpendPoint(int value)
    {
        if (point < value) return false;

        point -= value;
        OnPointChanged?.Invoke();
        SaveLoadManager.Instance.UpdatePoint(point);

        return true;
    }

    public void SetPoint(int value)
    {
        point = value;
        OnPointChanged?.Invoke();
    }
}