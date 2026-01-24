using UnityEngine;

public class QuestRewardSystem : MonoBehaviour
{
    public static QuestRewardSystem Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void GiveReward(QuestData quest)
    {
        Debug.Log($"Give reward for quest: {quest.questName}");

        switch (quest.questId)
        {
            // case "quest_kill_10":
            //     Reward_Kill10();
            //     break;

            // case "quest_unlock_skill":
            //     Reward_UnlockSkill();
            //     break;

            // case "quest_spawn_boss":
            //     Reward_SpawnBoss();
            //     break;

            default:
                Debug.Log("No reward logic for this quest");
                break;
        }
    }

    // void Reward_Kill10()
    // {
    //     Debug.Log("Reward: Increase player damage");

    //     PlayerStats.Instance?.AddDamage(2);
    // }

    // void Reward_UnlockSkill()
    // {
    //     Debug.Log("Reward: Unlock new skill");

    //     SkillManager.Instance?.UnlockSkill("Fireball");
    // }

    // void Reward_SpawnBoss()
    // {
    //     Debug.Log("Reward: Spawn Boss");

    //     BossSpawner.Instance?.SpawnBoss();
    // }
}
