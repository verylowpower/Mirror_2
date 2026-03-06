using UnityEngine;

public class QuestRewardSystem : MonoBehaviour
{
    public static QuestRewardSystem instance;

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

    public void GiveReward(QuestData quest)
    {
        Debug.Log($"Give reward for quest: {quest.questName}");

        if (!string.IsNullOrEmpty(quest.rewardSkillId))
        {
            MetaBuffManager.instance.Unlock(quest.rewardSkillId);
            SkillTreeManager.instance.SyncFromMetaBuff();

            Debug.Log("Unlocked skill: " + quest.rewardSkillId);
        }
    }

    // void Reward_Kill()
    // {
    //     Debug.Log("Reward: Increase player damage");

    //     PlayerAttack.instance.bulletDamage += 2;
    // }

    // void Reward_UnlockSkill(QuestData quest)
    // {
    //     Debug.Log("Reward: Unlock new skill");

    //     SkillTreeManager.instance.NotifySkillTreeChanged();
    // }
}
