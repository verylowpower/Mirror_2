using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillNodeUI : MonoBehaviour
{
    public string skillId;

    public Image iconImage;
    public GameObject lockOverlay;
    public Button button;
    public TextMeshProUGUI pointText;

    public Color lockedColor;
    public Color availableColor;
    public Color unlockedColor;

    void Start()
    {
        button.onClick.AddListener(OnClick);
        Refresh();
    }

    void OnClick()
    {
        SkillTreeManager.instance.BuySkill(skillId);
    }

    public void Refresh()
    {
        var mgr = SkillTreeManager.instance;

        bool questUnlocked = mgr.IsQuestUnlocked(skillId);
        bool unlocked = mgr.IsSkillUnlocked(skillId);
        bool canBuy = mgr.CanBuy(skillId);

        // LOCK
        lockOverlay.SetActive(!questUnlocked);

        // ICON COLOR
        if (!questUnlocked)
            iconImage.color = lockedColor;
        else if (!unlocked)
            iconImage.color = availableColor;
        else
            iconImage.color = unlockedColor;

        // BUTTON
        button.interactable = canBuy;

        // POINT TEXT
        if (pointText != null && mgr.skills != null)
        {
            var node = mgr.skills.Find(s => s.data.skillId == skillId);
            pointText.text = unlocked ? "OWNED" : node.data.pointRequire.ToString();
        }
    }
}
