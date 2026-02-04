using UnityEngine;

public class SkillTreeUI : MonoBehaviour
{
    public static SkillTreeUI instance;

    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Close()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
