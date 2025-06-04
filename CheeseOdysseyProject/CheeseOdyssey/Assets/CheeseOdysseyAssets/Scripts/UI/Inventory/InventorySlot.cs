using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    #region SerializedFields
    [SerializeField] private SlotSavedType slotType = SlotSavedType.Inventory;
    [SerializeField] private ItemType itemType = ItemType.Material;
    [SerializeField] private int slotId = -1;
    #endregion
    #region Private Fields
    protected InventoryItem itemInSlot = null;
    private RectTransform rectTransform = null;
    #endregion
    #region Accessors
    public SlotSavedType SlotType { get { return slotType; } }
    public ItemType ItemType { get { return itemType; } }
    public int SlotId { get {  return slotId; } }
    #endregion

    private void Awake() => rectTransform = GetComponent<RectTransform>();

    #region Unity Handlers
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (droppedItem == null) { return; }
        if (itemType != ItemType.Material && itemType != droppedItem.ItemSO.itemType) { return; }

        InventorySlot originalSlot = droppedItem.SlotAssigned;
        if (itemInSlot == null)
        {
            originalSlot.ClearSlot();
            SetItemInSlot(droppedItem);
            return;
        }
        InventoryItem tempItem = itemInSlot;
        originalSlot.ClearSlot();
        SetItemInSlot(droppedItem);
        originalSlot.SetItemInSlot(tempItem);
    }
    #endregion
    #region Public Methods
    public virtual void SetItemInSlot(InventoryItem newItem)
    {
        if (newItem == null) return;
        if (newItem.SlotAssigned != null) { newItem.SlotAssigned.ClearSlot(); }
        
        itemInSlot = newItem;
        itemInSlot.SetSlotAssigned(this);
        itemInSlot.ParentOnDrag = transform;
        itemInSlot.transform.SetParent(transform);
        itemInSlot.transform.localPosition = Vector3.zero;
        LevelManager.Instance.GetNibblesInventory().ChangeItemInventorySlot(itemInSlot.ItemSO, SlotType, SlotId);
    }
    public virtual void ClearSlot() => itemInSlot = null;
    public InventoryItem GetItem() => itemInSlot;
    #endregion
}
