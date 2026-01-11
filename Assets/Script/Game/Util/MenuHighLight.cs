using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuHighlight : MonoBehaviour
{
    public Color normalColor = Color.white;
    public Color selectedColor = Color.red;

    private Button[] buttons;
    private GameObject lastSelected;

    void Awake()
    {
        buttons = GetComponentsInChildren<Button>(true);
    }

    void Update()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current == lastSelected) return;

        UpdateHighlight(current);
        lastSelected = current;
    }

    void UpdateHighlight(GameObject selected)
    {
        foreach (Button btn in buttons)
        {
            Image img = btn.GetComponent<Image>();
            if (img == null) continue;

            img.color = (btn.gameObject == selected)
                ? selectedColor
                : normalColor;
        }
    }
}
