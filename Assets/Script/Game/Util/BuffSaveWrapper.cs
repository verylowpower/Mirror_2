using System.Collections.Generic;

[System.Serializable]
public class BuffSaveWrapper
{
    public List<string> buffIds;
    public List<string> questUnlockedIds;

    public BuffSaveWrapper(HashSet<string> buffs, HashSet<string> questUnlocked)
    {
        buffIds = new List<string>(buffs);
        questUnlockedIds = new List<string>(questUnlocked);
    }
}