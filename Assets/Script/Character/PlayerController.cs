using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private PlayerInputAction inputActions;
    private Vector2 moveInput;
    public Vector2 lookDir = Vector2.right;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        inputActions = new PlayerInputAction();

        inputActions.Input.PlayerInput.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Input.PlayerInput.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void Update()
    {
        if (moveInput.sqrMagnitude > 0.1f)
            lookDir = moveInput.normalized;
    }
}
