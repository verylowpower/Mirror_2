using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] public float moveSpeed = 5f;

    [SerializeField] private Transform aimArrow;

    Rigidbody2D rb;
    Animator animator;
    PlayerInputAction inputActions;

    Vector2 moveInput;
    Vector2 lookDir = Vector2.down;

    public Vector2 aimInput;
    public bool usingGamepad;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        inputActions = new PlayerInputAction();

        inputActions.Input.Move.performed += ctx =>
            moveInput = ctx.ReadValue<Vector2>();

        inputActions.Input.Move.canceled += _ =>
            moveInput = Vector2.zero;

        inputActions.Input.Aim.performed += ctx =>
        {
            aimInput = ctx.ReadValue<Vector2>();
            usingGamepad = ctx.control.device is Gamepad;
        };

        inputActions.Input.Aim.canceled += _ =>
            aimInput = Vector2.zero;

        inputActions.Input.Shoot.performed += _ =>
        {
            PlayerAttack.instance?.StartAttack();
        };

        inputActions.Input.Shoot.canceled += _ =>
        {
            PlayerAttack.instance?.StopAttack();
        };

        inputActions.Input.Melee.performed += _ =>
        {
            PlayerAttack.instance?.TryMelee();
        };
    }

    private void OnEnable() => inputActions.Enable();

    private void OnDisable()
    {
        if (inputActions != null)
            inputActions.Disable();
    }

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
        }

        animator.SetFloat("MoveX", lookDir.x);
        animator.SetFloat("MoveY", lookDir.y);

        Vector2 aimDir;

        if (usingGamepad && aimInput.sqrMagnitude > 0.1f)
        {
            aimDir = aimInput.normalized;
        }
        else
        {
            Camera cam = Camera.main;
            Vector3 mouseWorld = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mouseWorld.z = 0f;
            aimDir = (mouseWorld - transform.position).normalized;
        }

        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

        if (aimArrow != null)
        {
            aimArrow.rotation = Quaternion.Euler(0, 0, angle - 270);
        }
    }
}