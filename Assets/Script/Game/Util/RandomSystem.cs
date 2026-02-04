using System.Collections.Generic;
using UnityEngine;

public class RandomSystem : MonoBehaviour
{
    public static RandomSystem instance;
    public Buff_GUI buffUI;

    void Awake()
    {
        instance = this;
    }

    public void RandomBuff()
    {
        Time.timeScale = 0;


        List<Buff> available = new();

        foreach (var kvp in BuffLibrary.AllBuffs)
        {
            Buff buff = kvp.Value;
            if (!SkillTreeManager.instance.IsSkillUnlocked(buff.ID))
                continue;
            if (PlayerBuffManager.instance.IsBuffActive(buff.ID))
                continue;
            if (!string.IsNullOrEmpty(buff.RequirementBuffID) &&
                !SkillTreeManager.instance.IsSkillUnlocked(buff.RequirementBuffID))
                continue;
            available.Add(buff);
        }
        if (available.Count == 0)
        {
            Debug.Log("No valid buffs available.");
            PlayerBuffManager.instance.buffUIActive = false;
            Time.timeScale = 1;
            return;
        }
        List<Buff> selected = PickWeightedBuffs(available, 3);
        string[] ids = selected.ConvertAll(b => b.ID).ToArray();
        buffUI.ShowBuffs(ids, selectedBuffID =>
        {
            OnBuffSelected(selectedBuffID);
        });
    }

    private List<Buff> PickWeightedBuffs(List<Buff> pool, int count)
    {
        List<Buff> result = new();

        List<Buff> temp = new(pool);

        for (int i = 0; i < count && temp.Count > 0; i++)
        {
            int totalWeight = 0;

            foreach (var b in temp)
                totalWeight += b.Weight;

            int rnd = Random.Range(0, totalWeight);
            int sum = 0;
            Buff chosen = null;
            foreach (var b in temp)
            {
                sum += b.Weight;
                if (rnd < sum)
                {
                    chosen = b;
                    break;
                }
            }

            if (chosen != null)
            {
                result.Add(chosen);
                temp.Remove(chosen);
            }
        }

        return result;
    }

    private void OnBuffSelected(string selectedBuffID)
    {
        if (BuffLibrary.AllBuffs.TryGetValue(selectedBuffID, out Buff buff))
        {
            PlayerBuffManager.instance.AddBuff(buff.ID, buff);
            MetaBuffManager.instance.unlockedBuffs.Add(buff.ID);
        }
        buffUI.HideAll();
        PlayerBuffManager.instance.buffUIActive = false;
        Time.timeScale = 1;
    }
}
