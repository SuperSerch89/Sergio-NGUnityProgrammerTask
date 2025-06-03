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
    [SerializeField] private Transform helmetTransform;
    #endregion

    #region Private Fields
    private Rigidbody2D rb = null;
    private int runningBooleanHash = 0;
    private bool isMoving = false;
    private Vector2 currentVelocity = Vector2.zero;
    private InventoryController inventoryController = null;
    private PlayerInput playerInput = null;
    #endregion

    #region Unity Life Cycle
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        inventoryController = GetComponent<InventoryController>();
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

    #region Public Methods
    public void Initialize()
    {
        runningBooleanHash = Animator.StringToHash(runningBoolean);
    }
    #endregion
    #region Private Methods
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
        if (moveInput.x > 0)
        {
            playerArtRenderer.flipX = false;
            helmetTransform.localScale = Vector2.one;
        }
        else if (moveInput.x < 0)
        {
            playerArtRenderer.flipX = true;
            helmetTransform.localScale = new Vector2(-1, 1);
        }
    }
    private void Animations()
    {
        animator.SetBool(runningBooleanHash, isMoving);
    }
    #endregion
    #region PlayerInput
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }
    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            LevelManager.Instance.ShowInventory(true);
        }
    }
    public void OnCloseInventory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            LevelManager.Instance.ShowInventory(false);
        }
    }
    public void SwitchActionMap(InputMaps inputMap)
    {
        playerInput.SwitchCurrentActionMap(inputMap.ToString());
    }
    #endregion

    #region Enums
    private enum MovementMode { Normal, Jetpack }
    public enum InputMaps { Gameplay, UI}
    #endregion
}