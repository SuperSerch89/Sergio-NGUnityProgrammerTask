using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Image image = null;
    public Transform ParentOnDrag { get; set; } = null;
    public InventorySlot SlotAssigned { get; set; } = null;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ParentOnDrag = transform.parent;

        if (SlotAssigned != null) { SlotAssigned.ClearSlot(); }
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData) => transform.position = Input.mousePosition;
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(ParentOnDrag);
        transform.localPosition = Vector3.zero;
        image.raycastTarget = true;
    }
    public void SetSlotAssigned(InventorySlot slot)
    {
        SlotAssigned = slot;
    }
}
