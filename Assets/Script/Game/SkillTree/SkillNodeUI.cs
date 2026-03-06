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

        var node = mgr.skills
            .Find(s => s.data != null && s.data.skillId == skillId);

        if (node == null) return;

        bool questUnlocked = node.questUnlocked;
        bool skillUnlocked = node.isUnlocked;

        if (!questUnlocked)
        {
            if (lockOverlay != null)
                lockOverlay.SetActive(true);

            iconImage.color = lockedColor;
            button.interactable = false;

            if (pointText != null)
                pointText.text = "";

            return;
        }

        if (lockOverlay != null)
            lockOverlay.SetActive(false);

        if (skillUnlocked)
        {
            iconImage.color = unlockedColor;
            button.interactable = false;

            if (pointText != null)
                pointText.text = "OWNED";
        }
        else
        {
            iconImage.color = availableColor;
            button.interactable = mgr.CanBuy(skillId);

            if (pointText != null)
                pointText.text = node.data.pointRequire.ToString();
        }
    }
}
