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

        switch (quest.questId)
        {
            case "quest_kill":
                Reward_Kill();
                break;

            case "quest_unlock_skill":
                Reward_UnlockSkill(quest);
                break;

            // case "quest_spawn_boss":
            //     Reward_SpawnBoss();
            //     break;

            default:
                Debug.Log("No reward logic for this quest");
                break;
        }
    }

    void Reward_Kill()
    {
        Debug.Log("Reward: Increase player damage");

        PlayerAttack.instance.bulletDamage += 2;
    }

    void Reward_UnlockSkill(QuestData quest)
    {
        Debug.Log("Reward: Unlock new skill");
        SkillTreeManager.instance.UnlockSkillByQuest(quest.rewardSkillId);
    }

}
