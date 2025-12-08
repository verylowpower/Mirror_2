using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public System.Action onEnemyDeath;

    [Header("Stats")]
    [SerializeField] protected float health = 10f;
    [SerializeField] protected int damage = 5;
    public int Damage => damage;
    public float Health => health;

    [Header("Visual")]
    [SerializeField] protected SpriteRenderer spriteRender;
    [SerializeField] protected Color flashColor = Color.white;
    protected Color originColor;
    [SerializeField] protected float flashTime = 0.1f;

    [Header("NavMesh")]
    protected NavMeshAgent agent;
    [SerializeField] protected float moveSpeed = 3f;

    [Header("EXP")]
    [SerializeField] private int expDropMin = 1;
    [SerializeField] private int expDropMax = 3;
    [SerializeField] private GameObject expPrefab;


    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        // 2D setup
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
        health -= amount;

        if (amount > 0 && spriteRender != null)
            StartCoroutine(FlashHit());

        if (health <= 0f)
            KillEnemy();
    }

    public virtual void KillEnemy()
    {
        DropExp();
        onEnemyDeath?.Invoke();
        Destroy(gameObject);
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


    //EFFECT
    // public virtual void ApplyBurn(float dmgPerSec, float duration)
    // {
    //     StartCoroutine(DmgOverTime(dmgPerSec, duration));
    // }

    // public virtual void ApplySlow(float slowDownNumber, float duration)
    // {
    //     StartCoroutine(SlowSpeed(slowDownNumber, duration));
    // }

    // private IEnumerator DmgOverTime(float dmgPerSec, float duration)
    // {
    //     float interval = 1f;
    //     float elapsed = 0f;

    //     while (elapsed < duration)
    //     {
    //         ChangeHealth(dmgPerSec);
    //         yield return new WaitForSeconds(interval);
    //         elapsed += interval;
    //     }
    // }

    // private IEnumerator SlowSpeed(float slowDownNumber, float duration)
    // {
    //     if (isSlowed) yield break;

    //     isSlowed = true;
    //     movementSpeed -= slowDownNumber;
    //     StartCoroutine(FlashWhenHit(spriteRender, originColor, slowColor, duration));
    //     yield return new WaitForSeconds(duration);
    //     movementSpeed += slowDownNumber;
    //     isSlowed = false;
    // }


}
