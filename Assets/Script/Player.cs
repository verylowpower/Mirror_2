using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private PlayerInputAction inputActions;
    private Vector2 moveInput;

    public static Player instance;

    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();

        inputActions = new PlayerInputAction();

        inputActions.Input.PlayerInput.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Input.PlayerInput.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
