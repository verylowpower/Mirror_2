using UnityEngine;

public class Boss : Enemy
{
    private enum BossAction
    {
        Idle,
        Attack,
        Dash,
        Skill1,
        Skill2,
    }

    private BossAction currentAction = BossAction.Idle;

    [Header("Shoot Settings")]
    public float detectRange = 10f;
    public float shootingRange = 7f;
    public float shootingFireRate = 1f;
    public int shootingDamage = 10;
    public int bulletsPerBurst = 3;
    public float shootTimer;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private void BossBehavior()
    {

        switch (currentAction)
        {
            case BossAction.Attack:
                break;

            case BossAction.Dash:
                break;

            case BossAction.Skill1:
                break;

            case BossAction.Skill2:
                break;
        }
    }

}
