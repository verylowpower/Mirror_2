using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public System.Action onEnemyDeath;

    [Header("ID")]
    [SerializeField] protected string enemyId;

    [Header("Stats")]
    [SerializeField] protected float health = 10f;
    [SerializeField] protected int damage = 5;

    [SerializeField] protected int bulletSpeed = 5;
    [SerializeField] protected int point;
    public int Damage => damage;
    public float Health => health;

    [Header("Visual")]
    [SerializeField] public SpriteRenderer spriteRender;
    [SerializeField] protected Color flashColor = Color.white;
    protected Color originColor;
    [SerializeField] protected float flashTime = 0.1f;

    [Header("NavMesh")]
    protected NavMeshAgent agent;
    [SerializeField] public float moveSpeed = 3f;

    [Header("EXP")]
    [SerializeField] private int expDropMin = 1;
    [SerializeField] private int expDropMax = 3;
    [SerializeField] private GameObject expPrefab;

    public bool canTakeDamage = false;

    public void EnableAfterSummon()
    {
        canTakeDamage = true;
    }

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    protected virtual void Start()
    {
        if (spriteRender != null)
            originColor = spriteRender.color;
    }

    public virtual void ChangeHealth(float amount)
    {
        if (!canTakeDamage) return;
        health -= amount;
        if (amount > 0f && spriteRender != null)
            StartCoroutine(FlashHit());
        if (health <= 0f)
            KillEnemy();
    }


    public virtual void KillEnemy()
    {
        DropExp();
        onEnemyDeath?.Invoke();
        EnemyQuestCounter();
        Destroy(gameObject);
        PointCounter.instance.AddPoint(point);

    }

    private void DropExp()
    {
        if (expPrefab == null) return;

        int amount = Random.Range(expDropMin, expDropMax + 1);

        GameObject obj = Instantiate(expPrefab, transform.position, Quaternion.identity);
        obj.GetComponent<Experience>().expAmount = amount;
    }


    protected virtual IEnumerator FlashHit()
    {
        if (spriteRender == null) yield break;

        spriteRender.color = flashColor;
        yield return new WaitForSeconds(flashTime);
        spriteRender.color = originColor;
    }

    public void EnemyQuestCounter()
    {
        QuestManager.instance.NotifyEvent(
           QuestType.KillEnemies,
           enemyId,
           1);
    }
}