using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region SerializedFields
    [SerializeField] private Image iconImage = null;
    [SerializeField][ReadOnly] private ItemID itemID = ItemID.Empty;
    [SerializeField] private Text quantityText = null;
    #endregion
    #region Private Fields
    private Item itemSO = null;
    private RectTransform rect = null;
    #endregion
    #region Accessors
    public Transform ParentOnDrag { get; set; } = null;
    public InventorySlot SlotAssigned { get; set; } = null;
    public Item ItemSO { get {  return itemSO; } }
    #endregion

    private void Awake() => rect = GetComponent<RectTransform>();

    #region Unity Handlers
    public void OnBeginDrag(PointerEventData eventData)
    {
        ParentOnDrag = transform.parent;

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
        rect.localScale = Vector3.one;
    }
    public void OnPointerEnter(PointerEventData eventData) => UIManager.Instance.ShowTooltip(true, itemSO.itemName);
    public void OnPointerExit(PointerEventData eventData) => UIManager.Instance.ShowTooltip(false);
    #endregion
    #region Public Methods
    public void SetupInventoryItem(Item newItemSO)
    {
        itemSO = newItemSO;
        iconImage.sprite = itemSO.icon;
        itemID = itemSO.itemID;
    }
    public void SetQuantity(int newQuantity)
    {
        quantityText.text = newQuantity.ToString();
        quantityText.gameObject.SetActive(newQuantity > 1 ? true : false);
    }
    public void SetSlotAssigned(InventorySlot slot) => SlotAssigned = slot;
    #endregion
}
