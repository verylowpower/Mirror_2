using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] public float moveSpeed = 5f;

    Rigidbody2D rb;
    Animator animator;
    PlayerInputAction inputActions;

    Vector2 moveInput;
    Vector2 lookDir = Vector2.down;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        inputActions = new PlayerInputAction();
        inputActions.Input.PlayerInput.performed += ctx =>
            moveInput = ctx.ReadValue<Vector2>();

        inputActions.Input.PlayerInput.canceled += _ =>
            moveInput = Vector2.zero;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void FixedUpdate()
    {
        if (PlayerAttack.instance != null && PlayerAttack.instance.IsAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void Update()
    {
        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            lookDir = moveInput.normalized;

            animator.SetFloat("MoveX", lookDir.x);
            animator.SetFloat("MoveY", lookDir.y);
        }
        else
        {
            animator.SetFloat("MoveX", lookDir.x);
            animator.SetFloat("MoveY", lookDir.y);
        }
    }
}
