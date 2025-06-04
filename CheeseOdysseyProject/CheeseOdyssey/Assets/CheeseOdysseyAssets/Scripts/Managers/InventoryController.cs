using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private List<InventoryItemData> inventory = new List<InventoryItemData>();

    public void AddItem(ItemData addedItem)
    {
        InventoryItemData foundItemInInventory = inventory.Find(inventoryItem => inventoryItem.itemData.itemID == addedItem.itemID);
        if (foundItemInInventory == null)
        {
            int freeInventorySlot = UIManager.Instance.GetFreeSlot();
            foundItemInInventory = new InventoryItemData(addedItem, SlotSavedType.Inventory, freeInventorySlot);
            UIManager.Instance.SetupInventoryItems(new List<InventoryItemData> { foundItemInInventory });
            inventory.Add(foundItemInInventory);
        }
        else
        {
            foundItemInInventory.itemData.quantity += addedItem.quantity;
            UIManager.Instance.UpdateItemQuantity(foundItemInInventory);

        }
    }
    public void ModifyItemQuantity(ItemData modifiedItem)
    {
        InventoryItemData foundItemInInventory = inventory.Find(inventoryItem => inventoryItem.itemData.itemID == modifiedItem.itemID);
        foundItemInInventory.itemData.quantity = modifiedItem.quantity;
        if (foundItemInInventory.itemData.quantity <= 0) { inventory.Remove(foundItemInInventory); }
        UIManager.Instance.UpdateItemQuantity(foundItemInInventory);
    }
    public void ChangeItemInventorySlot(Item itemSO, SlotSavedType slotSavedType, int newSlotID)
    {
        InventoryItemData foundItemInInventory = inventory.Find(inventoryItem => inventoryItem.itemData.itemID == itemSO.itemID);
        if (foundItemInInventory == null) { return; }
        foundItemInInventory.savedSlotType = slotSavedType;
        foundItemInInventory.slotUsed = newSlotID;
    }
    public int GetItemQuantity(ItemID itemID)
    {
        int quantityFound = 0;
        InventoryItemData inventoryItemData = inventory.Find(item => item.itemData.itemID == itemID);
        if (inventoryItemData != null) { quantityFound = inventoryItemData.itemData.quantity; }
        return quantityFound;
    }
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
