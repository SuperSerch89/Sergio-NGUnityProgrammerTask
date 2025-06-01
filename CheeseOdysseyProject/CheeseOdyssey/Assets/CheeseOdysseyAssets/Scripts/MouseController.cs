using NicoUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : Singleton<MouseController>
{
    #region Serialized Fields
    [Header("Movement")]
    [SerializeField] private MovementMode currentMode = MovementMode.Normal;
    [SerializeField] private float normalSpeed = 5f;
    [SerializeField] private Vector2 moveInput = Vector2.zero;
    [SerializeField] private float jetpackForce = 5f;
    [SerializeField] private float maxJetpackSpeed = 4f;
    [Header("Visuals")]
    [SerializeField] private SpriteRenderer playerArtRenderer = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private string runningBoolean = "Running";
    #endregion

    #region Private Fields
    private Rigidbody2D rb = null;
    private int runningBooleanHash = 0;
    private bool isMoving = false;
    private Vector2 currentVelocity = Vector2.zero;
    #endregion

    #region Unity Life Cycle
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        Initialize();
    }
    private void Update()
    {
        VisualsFlip();
        Animations();
    }
    private void FixedUpdate()
    {
        Move();
    }
    #endregion

    public void Initialize()
    {
        runningBooleanHash = Animator.StringToHash(runningBoolean);
    }
    private void Move()
    {
        isMoving = moveInput != Vector2.zero;
        switch (currentMode)
        {
            case MovementMode.Normal:
                ApplyGroundMovement();
                break;

            case MovementMode.Jetpack:
                ApplyJetpackMovement();
                break;
        }
    }
    private void ApplyGroundMovement()
    {
        currentVelocity = moveInput * normalSpeed;
        rb.linearVelocity = currentVelocity;
    }
    private void ApplyJetpackMovement()
    {
        rb.AddForce(moveInput * jetpackForce, ForceMode2D.Force);
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxJetpackSpeed);
    }
    private void VisualsFlip()
    {
        if (moveInput.x > 0) { playerArtRenderer.flipX = false; }
        else if (moveInput.x < 0) { playerArtRenderer.flipX = true; }
    }
    private void Animations()
    {
        animator.SetBool(runningBooleanHash, isMoving);
    }

    #region PlayerInput
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }
    #endregion

    private enum MovementMode { Normal, Jetpack }
}