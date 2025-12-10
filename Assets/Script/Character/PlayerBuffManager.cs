using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffManager : MonoBehaviour
{
    public static PlayerBuffManager instance;

    private Dictionary<string, ActiveBuff> activeBuffs = new();
    public HashSet<string> unlockedBuffs = new();
    public bool buffUIActive = false;

    private void Awake()
    {
        instance = this;
    }

    public void AddBuff(string buffID, Buff buffData)
    {
        // Nếu đang có buff này → reset duration
        if (activeBuffs.ContainsKey(buffID))
        {
            activeBuffs[buffID].roomsLeft = buffData.Duration;
            Debug.Log($"Buff {buffID} refreshed to {buffData.Duration} rooms");
            return;
        }

        // Apply effect
        buffData.ApplyEffect?.Invoke();

        // Nếu buff có duration theo room → lưu vào danh sách
        if (buffData.Duration > 0)
        {
            activeBuffs[buffID] = new ActiveBuff(buffData.Duration, buffData.RemoveEffect);
        }

        Debug.Log($"Buff {buffID} applied ({buffData.Duration} rooms)");
    }

    // GỌI HÀM NÀY mỗi lần đổi sang room mới
    public void OnEnterNewRoom()
    {
        List<string> expired = new();

        foreach (var kvp in activeBuffs)
        {
            kvp.Value.roomsLeft--;

            if (kvp.Value.roomsLeft <= 0)
                expired.Add(kvp.Key);
        }

        foreach (string id in expired)
        {
            activeBuffs[id].onRemove?.Invoke();
            activeBuffs.Remove(id);

            Debug.Log($"Buff expired: {id}");
        }
    }

    public bool IsBuffActive(string buffID)
    {
        return activeBuffs.ContainsKey(buffID);
    }


    private class ActiveBuff
    {
        public int roomsLeft;
        public System.Action onRemove;

        public ActiveBuff(int rooms, System.Action remove)
        {
            roomsLeft = rooms;
            onRemove = remove;
        }
    }
}
