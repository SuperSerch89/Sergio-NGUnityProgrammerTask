using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region SerializedFields
    [SerializeField] private Image iconImage = null;
    [SerializeField][ReadOnly] private ItemID itemID = ItemID.Empty;
    #endregion
    #region Private Fields
    private Item itemSO = null;
    #endregion
    #region Accessors
    public Transform ParentOnDrag { get; set; } = null;
    public InventorySlot SlotAssigned { get; set; } = null;
    public Item ItemSO { get {  return itemSO; } }
    #endregion

    #region Unity Handlers
    public void OnBeginDrag(PointerEventData eventData)
    {
        ParentOnDrag = transform.parent;

        if (SlotAssigned != null) { SlotAssigned.ClearSlot(); }
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        iconImage.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData) => transform.position = Input.mousePosition;
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(ParentOnDrag);
        transform.localPosition = Vector3.zero;
        iconImage.raycastTarget = true;
    }
    #endregion
    #region Public Methods
    public void SetupInventoryItem(Item newItemSO)
    {
        itemSO = newItemSO;
        iconImage.sprite = itemSO.icon;
        itemID = itemSO.itemID;
    }
    public void SetSlotAssigned(InventorySlot slot) => SlotAssigned = slot;
    #endregion
}
