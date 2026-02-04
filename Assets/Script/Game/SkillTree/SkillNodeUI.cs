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

    void OnEnable()
    {
        if (SkillTreeManager.instance != null)
        {
            SkillTreeManager.instance.OnSkillTreeChanged += Refresh;
            Refresh();
        }
    }

    void OnDisable()
    {
        if (SkillTreeManager.instance != null)
            SkillTreeManager.instance.OnSkillTreeChanged -= Refresh;
    }

    void OnClick()
    {
        SkillTreeManager.instance.BuySkill(skillId);
    }

    public void Refresh()
    {
        var mgr = SkillTreeManager.instance;
        if (mgr == null) return;

        bool questUnlocked = mgr.IsQuestUnlocked(skillId);
        bool unlocked = mgr.IsSkillUnlocked(skillId);
        bool canBuy = mgr.CanBuy(skillId);

        lockOverlay.SetActive(!questUnlocked);

        if (!questUnlocked)
            iconImage.color = lockedColor;
        else if (!unlocked)
            iconImage.color = availableColor;
        else
            iconImage.color = unlockedColor;

        button.interactable = questUnlocked && !unlocked && canBuy;

        if (pointText != null)
        {
            var node = mgr.skills.Find(s => s.data != null && s.data.skillId == skillId);
            pointText.text = unlocked ? "OWNED" : node != null ? node.data.pointRequire.ToString() : "";
        }
    }

}
