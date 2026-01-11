using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BuffSlot : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public Image iconImg;

    [Header("Highlight")]
    [SerializeField] private GameObject highlight;


    private string buffID;
    private Action<string> onSelect;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Setup(Buff buff, Action<string> callback)
    {
        buffID = buff.ID;
        nameText.text = buff.Name;

        if (descText != null)
            descText.text = buff.Description;

        if (iconImg != null && buff.Icon != null)
            iconImg.sprite = buff.Icon;

        onSelect = callback;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Select);
        }

        Highlight(false);
    }


    public void Highlight(bool value)
    {
        if (highlight != null)
            highlight.SetActive(value);

   
        transform.localScale = value ? Vector3.one * 1.05f : Vector3.one;
    }

   
    public void Select()
    {
        // Debug.Log("Selected buff: " + buffID);
        onSelect?.Invoke(buffID);
    }
}
