using System;
using System.Collections.Generic;

[Serializable]
public class BuffSaveWrapper
{
    public List<string> buffIds;

    public BuffSaveWrapper(HashSet<string> buffs)
    {
        buffIds = new List<string>(buffs);
    }
}