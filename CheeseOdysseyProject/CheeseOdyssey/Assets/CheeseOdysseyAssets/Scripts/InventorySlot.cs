using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    private InventoryItem itemInSlot = null;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (droppedItem == null) { return; }
        InventorySlot originalSlot = droppedItem.SlotAssigned;

        if (itemInSlot == null)
        {
            SetItemInSlot(droppedItem);
            return;
        }
        InventoryItem tempItem = itemInSlot;
        SetItemInSlot(droppedItem);
        originalSlot.SetItemInSlot(tempItem);
    }

    public void SetItemInSlot(InventoryItem newItem)
    {
        if (newItem == null) return;
        if (newItem.SlotAssigned != null) { newItem.SlotAssigned.ClearSlot(); }
        
        itemInSlot = newItem;
        itemInSlot.SetSlotAssigned(this);
        itemInSlot.ParentOnDrag = transform;
        itemInSlot.transform.SetParent(transform);
        itemInSlot.transform.localPosition = Vector3.zero;
    }
    public void ClearSlot()
    {
        itemInSlot = null;
    }
    public InventoryItem GetItem() => itemInSlot;
}
