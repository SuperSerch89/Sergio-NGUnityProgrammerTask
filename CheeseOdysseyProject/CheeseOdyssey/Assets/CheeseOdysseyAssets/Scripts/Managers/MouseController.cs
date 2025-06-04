using NicoUtilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;

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
    [SerializeField] private List<EquipmentResolver> equipmentResolvers = new List<EquipmentResolver>();
    [Header("Interactions")]
    [SerializeField] private InteractableDetector interactableDetector = null;
    #endregion

    #region Private Fields
    private Rigidbody2D rb = null;
    private InventoryController inventoryController = null;
    private PlayerInput playerInput = null;
    private int runningBooleanHash = 0;
    private bool isMoving = false;
    private Vector2 currentVelocity = Vector2.zero;
    #endregion
    #region Accessors
    public InventoryController InventoryController {  get { return inventoryController; } }
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
    public void ChangeEquipment(ItemType itemType, ItemID itemID)
    {
        EquipmentResolver equipmentFound = equipmentResolvers.FirstOrDefault(resolver => resolver.itemType == itemType);
        if (equipmentFound == null) 
        {
            Debug.LogError($"Couldn't find equipment resolver with type {itemType}");
            return;
        }
        ItemLabel labelFound = equipmentFound.labels.FirstOrDefault(itemLabel => itemLabel.itemID == itemID);
        if (labelFound == null)
        {
            Debug.LogError($"Couldn't find equipment item label with itemID {itemID}");
            return;
        }
        equipmentFound.spriteResolver.SetCategoryAndLabel(equipmentFound.itemType.ToString(), labelFound.label.ToString());
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
        if (!context.started) { return; }
        LevelManager.Instance.ShowInventory(true);
    }
    public void OnCloseInventory(InputAction.CallbackContext context)
    {
        if (!context.started) { return; }
        LevelManager.Instance.ShowInventory(false);
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.started) { return; }
        var interactable = interactableDetector.GetClosestInteractable();
        if (interactable == null) { return; }
        interactable.Interact();
    }
    public void SwitchActionMap(InputMaps inputMap)
    {
        playerInput.SwitchCurrentActionMap(inputMap.ToString());
    }
    #endregion
    #region Enums
    private enum MovementMode { Normal, Jetpack }
    public enum InputMaps { Gameplay, UI}
    public enum EquippableLabel
    {
        Empty = 0,
        Blue, Orange, Pink, Purple,
        Basic, Improved,
    }
    #endregion
    [System.Serializable]
    public class EquipmentResolver
    {
        public SpriteResolver spriteResolver = null;
        public ItemType itemType = ItemType.Helmet;
        public List<ItemLabel> labels = new List<ItemLabel>();
    }
    [System.Serializable]
    public class ItemLabel
    {
        public ItemID itemID = ItemID.Empty;
        public EquippableLabel label = EquippableLabel.Basic;
    }
}