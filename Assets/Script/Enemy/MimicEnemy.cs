using UnityEngine;
using UnityEngine.AI;

public class MimicEnemy : Enemy
{
    [Header("Interaction")]
    [SerializeField] private float detectRange = 2f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private Animator anim;
    private Transform player;

    private bool interacted = false;
    private bool canChase = false;

    protected override void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        agent.isStopped = true;
        canTakeDamage = false;

        anim.SetBool("isAttack", false);
    }

    void Update()
    {
        if (player == null) return;

        if (!interacted)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= detectRange && Input.GetKeyDown(interactKey))
            {
                ActivateMimic();
            }
        }
        else if (canChase)
        {
            agent.SetDestination(player.position);
        }
    }

    void ActivateMimic()
    {
        interacted = true;
        canTakeDamage = true;

        anim.SetBool("isAttack", true);

        agent.isStopped = false;
        canChase = true;
    }
}