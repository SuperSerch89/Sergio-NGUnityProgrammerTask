using UnityEngine;
using UnityEngine.UI;

public class EquippmentSlot : InventorySlot
{
    #region SerializedFields
    [SerializeField] private Text description = null;
    [SerializeField] private string emptySlotDescrition = "EMPTY";
    [SerializeField] private ItemType defaultItemType = ItemType.Helmet;
    #endregion

    #region Overriden Methods
    public override void SetItemInSlot(InventoryItem newItem)
    {
        base.SetItemInSlot(newItem);
        if (newItem == null) { return; }
        description.text = itemInSlot.ItemSO.description;
        UIManager.Instance.ChangedSlotEquipment(itemInSlot.ItemSO.itemType, itemInSlot.ItemSO.itemID);
    }
    public override void ClearSlot()
    {
        UIManager.Instance.ChangedSlotEquipment(defaultItemType, ItemID.Empty);
        base.ClearSlot();
        description.text = emptySlotDescrition;
    }
    #endregion
}
