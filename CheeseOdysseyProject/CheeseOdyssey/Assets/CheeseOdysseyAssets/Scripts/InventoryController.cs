using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private List<InventoryItemData> inventory = new List<InventoryItemData>();
}

[System.Serializable]
public class InventoryItemData
{
    public ItemData itemData = null;
    public SlotSavedType savedSlotType = SlotSavedType.Inventory;
    public int slotUsed = -1;

    public InventoryItemData() { }
    public InventoryItemData(ItemData itemData, SlotSavedType slotType, int slotUsed)
    {
        this.itemData = itemData;
        this.savedSlotType = slotType;
        this.slotUsed = slotUsed;
    }
}
[System.Serializable]
public class ItemData
{
    public ItemID itemID = ItemID.Empty;
    public int quantity = 0;
    public ItemData() { }
    public ItemData(ItemID itemID, int quantity, bool isEquipped = false)
    {
        this.itemID = itemID;
        this.quantity = quantity;
    }
}
#region Public Enums
public enum ItemType { Material, Helmet, Jetpack}
public enum SlotSavedType { Inventory, Equipped}
#endregion
