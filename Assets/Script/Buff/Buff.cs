using UnityEngine;

public class Buff
{
    public string ID;
    public string Name;
    public string Description;
    public Sprite Icon;
    public int Duration;      
    public int Weight;        

    public System.Action ApplyEffect;
    public System.Action RemoveEffect;

    public string RequirementBuffID;

    public Buff(
        string id,
        string name,
        string desc,
        Sprite icon,
        int duration,
        int weight,
        System.Action apply,
        System.Action remove,
        string requirement = null)
    {
        ID = id;
        Name = name;
        Description = desc;
        Icon = icon;

        Duration = duration;
        Weight = weight;

        ApplyEffect = apply;
        RemoveEffect = remove;

        RequirementBuffID = requirement;
    }
}
