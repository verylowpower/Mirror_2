using System.Collections.Generic;
using UnityEngine;

public class BuffLibrary : MonoBehaviour
{
    public static Dictionary<string, Buff> AllBuffs = new();

    static BuffLibrary()
    {
        Sprite dummyIcon = null;

        // HEALTH BUFFS (vĩnh viễn)
        AllBuffs["H1"] = new Buff("H1", "Increase Health 1", "Health +1", dummyIcon, 0, 20,
            () => { PlayerHealth.instance.maxHealth += 2; },
            () => { PlayerHealth.instance.maxHealth -= 2; }
        );

        AllBuffs["H2"] = new Buff("H2", "Increase Health 2", "Health +2", dummyIcon, 0, 15,
            () => { PlayerHealth.instance.maxHealth += 5; },
            () => { PlayerHealth.instance.maxHealth -= 5; },
            "H1"
        );

        AllBuffs["H3"] = new Buff("H3", "Increase Health 3", "Health +3", dummyIcon, 0, 10,
            () => { PlayerHealth.instance.maxHealth += 6; },
            () => { PlayerHealth.instance.maxHealth -= 6; },
            "H2"
        );

        // FIRE RATE BUFFS (tồn tại 3 room)
        AllBuffs["B1"] = new Buff("B1", "Faster Fire Rate 1", "Fire Rate +1", dummyIcon, 3, 10,
            () => { PlayerAttack.instance.bulletSpeed -= 0.1f; },
            () => { PlayerAttack.instance.bulletSpeed += 0.1f; }
        );

        AllBuffs["B2"] = new Buff("B2", "Faster Fire Rate 2", "Fire Rate +2", dummyIcon, 3, 10,
            () => { PlayerAttack.instance.bulletSpeed -= 0.2f; },
            () => { PlayerAttack.instance.bulletSpeed += 0.2f; },
            "B1"
        );

        AllBuffs["B3"] = new Buff("B3", "Faster Fire Rate 3", "Fire Rate +3", dummyIcon, 3, 10,
            () => { PlayerAttack.instance.bulletSpeed -= 0.2f; },
            () => { PlayerAttack.instance.bulletSpeed += 0.2f; },
            "B2"
        );

        // BULLET DAMAGE BUFFS (5 room)
        AllBuffs["BD1"] = new Buff("BD1", "Bullet Damage 1", "Bullet Dmg +25%", dummyIcon, 5, 50,
            () => { PlayerAttack.instance.bulletDamage += 2; },
            () => { PlayerAttack.instance.bulletDamage -= 2; }
        );

        AllBuffs["BD2"] = new Buff("BD2", "Bullet Damage 2", "Bullet Dmg +50%", dummyIcon, 5, 10,
            () => { PlayerAttack.instance.bulletDamage += 5; },
            () => { PlayerAttack.instance.bulletDamage -= 5; },
            "BD1"
        );

        AllBuffs["BD3"] = new Buff("BD3", "Bullet Damage 3", "Bullet Dmg +75%", dummyIcon, 5, 10,
            () => { PlayerAttack.instance.bulletDamage += 10; },
            () => { PlayerAttack.instance.bulletDamage -= 10; },
            "BD2"
        );

        // -------------------
        // PLAYER SPEED BUFFS (3 room)
        // -------------------
        AllBuffs["S1"] = new Buff("S1", "Move Speed 1", "Speed +20%", dummyIcon, 3, 5,
            () => { PlayerController.instance.moveSpeed += 0.2f; },
            () => { PlayerController.instance.moveSpeed -= 0.2f; }
        );

        AllBuffs["S2"] = new Buff("S2", "Move Speed 2", "Speed +50%", dummyIcon, 3, 5,
            () => { PlayerController.instance.moveSpeed += 0.5f; },
            () => { PlayerController.instance.moveSpeed -= 0.5f; },
            "S1"
        );

        AllBuffs["S3"] = new Buff("S3", "Move Speed 3", "Speed +100%", dummyIcon, 3, 5,
            () => { PlayerController.instance.moveSpeed += 1f; },
            () => { PlayerController.instance.moveSpeed -= 1f; },
            "S2"
        );

        // COLLECT RADIUS BUFF (2 room)
        AllBuffs["CR1"] = new Buff("CR1", "Collect Radius 1", "Pickup Radius +20%", dummyIcon, 2, 10,
            () => { PlayerExperience.instance.collectRadius += 0.2f; },
            () => { PlayerExperience.instance.collectRadius -= 0.2f; }
        );

        AllBuffs["CR2"] = new Buff("CR2", "Collect Radius 2", "Pickup Radius +50%", dummyIcon, 2, 20,
            () => { PlayerExperience.instance.collectRadius += 1f; },
            () => { PlayerExperience.instance.collectRadius -= 1f; },
            "CR1"
        );

        AllBuffs["CR3"] = new Buff("CR3", "Collect Radius 3", "Pickup Radius +100%", dummyIcon, 2, 30,
            () => { PlayerExperience.instance.collectRadius += 1.5f; },
            () => { PlayerExperience.instance.collectRadius -= 1.5f; },
            "CR2"
        );
    }
}
