using System.Collections.Generic;
using UnityEngine;

public class BuffLibrary : MonoBehaviour
{
    public static BuffLibrary instance;
    public static Dictionary<string, Buff> AllBuffs = new();
    void Awake()
    {
        instance = this;
    }
    static BuffLibrary()
    {
        Sprite H1Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/H1");
        Sprite H2Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/H2");
        Sprite H3Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/H3");

        Sprite B1Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/B1");
        Sprite B2Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/B2");
        Sprite B3Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/B3");

        Sprite BD1Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/BD1");
        Sprite BD2Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/BD2");
        Sprite BD3Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/BD3");

        Sprite S1Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/S1");
        Sprite S2Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/S2");
        Sprite S3Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/S3");

        Sprite CR1Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/CR1");
        Sprite CR2Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/CR2");
        Sprite CR3Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/CR3");

        Sprite ICE1Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/ICE1");
        Sprite BURN1Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/BURN1");
        Sprite LIGHT1Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/LIGHT1");
        Sprite SPREAD1Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/SPREAD1");
        Sprite PIERCE1Icon = Resources.Load<Sprite>("BuffIcons/ExportedSprites/PIERCE1");




        AllBuffs["H1"] = new Buff("H1", "Increase Health 1", "Health +1", H1Icon, 0, 20,
            () => { PlayerHealth.instance.maxHealth += 2; },
            () => { PlayerHealth.instance.maxHealth -= 2; }
        );

        AllBuffs["H2"] = new Buff("H2", "Increase Health 2", "Health +2", H2Icon, 0, 15,
            () => { PlayerHealth.instance.maxHealth += 5; },
            () => { PlayerHealth.instance.maxHealth -= 5; },
            "H1"
        );

        AllBuffs["H3"] = new Buff("H3", "Increase Health 3", "Health +3", H3Icon, 0, 10,
            () => { PlayerHealth.instance.maxHealth += 6; },
            () => { PlayerHealth.instance.maxHealth -= 6; },
            "H2"
        );

        AllBuffs["B1"] = new Buff("B1", "Faster Fire Rate 1", "Fire Rate +1", B1Icon, 3, 10,
            () => { PlayerAttack.instance.fireRate += 1f; },
            () => { PlayerAttack.instance.fireRate -= 1f; }
        );

        AllBuffs["B2"] = new Buff("B2", "Faster Fire Rate 2", "Fire Rate +2", B2Icon, 3, 10,
            () => { PlayerAttack.instance.fireRate += 2f; },
            () => { PlayerAttack.instance.fireRate -= 2f; },
            "B1"
        );

        AllBuffs["B3"] = new Buff("B3", "Faster Fire Rate 3", "Fire Rate +3", B3Icon, 3, 10,
            () => { PlayerAttack.instance.fireRate += 3f; },
            () => { PlayerAttack.instance.fireRate -= 3f; },
            "B2"
        );


        AllBuffs["BD1"] = new Buff("BD1", "Bullet Damage 1", "Bullet Dmg +25%", BD1Icon, 5, 50,
            () => { PlayerAttack.instance.bulletDamage += 2; },
            () => { PlayerAttack.instance.bulletDamage -= 2; }
        );

        AllBuffs["BD2"] = new Buff("BD2", "Bullet Damage 2", "Bullet Dmg +50%", BD2Icon, 5, 10,
            () => { PlayerAttack.instance.bulletDamage += 5; },
            () => { PlayerAttack.instance.bulletDamage -= 5; },
            "BD1"
        );

        AllBuffs["BD3"] = new Buff("BD3", "Bullet Damage 3", "Bullet Dmg +75%", BD3Icon, 5, 10,
            () => { PlayerAttack.instance.bulletDamage += 10; },
            () => { PlayerAttack.instance.bulletDamage -= 10; },
            "BD2"
        );

        AllBuffs["S1"] = new Buff("S1", "Move Speed 1", "Speed +20%", S1Icon, 3, 5,
            () => { PlayerController.instance.moveSpeed += 0.2f; },
            () => { PlayerController.instance.moveSpeed -= 0.2f; }
        );

        AllBuffs["S2"] = new Buff("S2", "Move Speed 2", "Speed +50%", S2Icon, 3, 5,
            () => { PlayerController.instance.moveSpeed += 0.5f; },
            () => { PlayerController.instance.moveSpeed -= 0.5f; },
            "S1"
        );

        AllBuffs["S3"] = new Buff("S3", "Move Speed 3", "Speed +100%", S3Icon, 3, 5,
            () => { PlayerController.instance.moveSpeed += 1f; },
            () => { PlayerController.instance.moveSpeed -= 1f; },
            "S2"
        );

        AllBuffs["CR1"] = new Buff("CR1", "Collect Radius 1", "Pickup Radius +20%", CR1Icon, 2, 10,
            () => { PlayerExperience.instance.collectRadius += 0.2f; },
            () => { PlayerExperience.instance.collectRadius -= 0.2f; }
        );

        AllBuffs["CR2"] = new Buff("CR2", "Collect Radius 2", "Pickup Radius +50%", CR2Icon, 2, 20,
            () => { PlayerExperience.instance.collectRadius += 1f; },
            () => { PlayerExperience.instance.collectRadius -= 1f; },
            "CR1"
        );

        AllBuffs["CR3"] = new Buff("CR3", "Collect Radius 3", "Pickup Radius +100%", CR3Icon, 2, 30,
            () => { PlayerExperience.instance.collectRadius += 1.5f; },
            () => { PlayerExperience.instance.collectRadius -= 1.5f; },
            "CR2"
        );

        AllBuffs["ICE1"] = new Buff("ICE1", "Ice Bullet", "Bullets slow enemies", ICE1Icon, 5, 100,
            () => { PlayerAttack.instance.hasIceBuff = true; },
            () => { PlayerAttack.instance.hasIceBuff = false; }
        );

        AllBuffs["BURN1"] = new Buff("BURN1", "Burn Bullet", "Bullets apply burning DoT", BURN1Icon, 5, 100,
            () => { PlayerAttack.instance.hasBurnBuff = true; },
            () => { PlayerAttack.instance.hasBurnBuff = false; }
        );

        AllBuffs["LIGHT1"] = new Buff("LIGHT1", "Lightning Bullet", "Bullets chain lightning on hit", LIGHT1Icon, 5, 100,
            () => { PlayerAttack.instance.hasLightningBuff = true; },
            () => { PlayerAttack.instance.hasLightningBuff = false; }
        );

        AllBuffs["SPREAD1"] = new Buff("SPREAD1", "Spread Shot", "Shoot 3 bullets in a cone", SPREAD1Icon, 3, 50,
            () => { PlayerAttack.instance.hasSpreadShot = true; },
            () => { PlayerAttack.instance.hasSpreadShot = false; }
        );

        AllBuffs["PIERCE1"] = new Buff("PIERCE1", "Piercing Shot", "Bullets pierce enemies", PIERCE1Icon, 3, 50,
            () => { PlayerAttack.instance.pierceCount += 2; },
            () => { PlayerAttack.instance.pierceCount -= 2; }
        );
    }
}
