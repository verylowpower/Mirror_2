using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ChestInteractable : MonoBehaviour
{
    [SerializeField] private float detectRange = 2f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [SerializeField] private GameObject rewardPrefab;

    private Transform player;
    private Animator anim;

    private bool opened = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        anim = GetComponent<Animator>();

        anim.SetBool("isOpen", false);
    }

    void Update()
    {
        if (opened) return;
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectRange && Input.GetKeyDown(interactKey))
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        opened = true;

        anim.SetBool("isOpen", true);

        if (rewardPrefab != null)
            Instantiate(rewardPrefab, transform.position, Quaternion.identity);

        Debug.Log("Chest Opened!");
    }
}