using NicoUtilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    #region Serialized Fields
    [SerializeField] private GameObject inventoryPanel = null;
    [SerializeField] private GameObject inventoryItemPrefab = null;
    [SerializeField] private List<Item> allItems = null;
    [SerializeField] private Tooltip tooltip = null;
    #endregion
    #region Private Fields
    private Dictionary<int, InventorySlot> inventorySlots = new Dictionary<int, InventorySlot>();
    private Dictionary<int, InventorySlot> equippedSlots = new Dictionary<int, InventorySlot>();
    #endregion

    #region Unity Overriden Methods
    protected override void Awake()
    {
        base.Awake();
        GetSlots();
    }
    #endregion
    #region Private Methods
    private void GetSlots()
    {
        foreach (InventorySlot slot in GetComponentsInChildren<InventorySlot>(true))
        {
            switch (slot.SlotType)
            {
                case SlotSavedType.Inventory:
                    inventorySlots[slot.SlotId] = slot;
                    break;
                default:
                    equippedSlots[slot.SlotId] = slot;
                    break;
            }
        }
    }
    #endregion
    #region Public Methods
    public void SetupInventoryItems(List<InventoryItemData> inventoryItems)
    {
        foreach (InventoryItemData item in inventoryItems)
        {
            if (item == null) { continue; }

            GameObject newItem = Instantiate(inventoryItemPrefab);
            InventoryItem newInventoryItem = newItem.GetComponent<InventoryItem>();
            Item itemSO = allItems.Find(itemSO => itemSO.itemID == item.itemData.itemID);
            if (itemSO == null) 
            {
                Debug.LogError($"Couldn't find {itemSO.itemID} item scriptable Object.");
                continue;
            }
            newInventoryItem.SetupInventoryItem(itemSO);
            newInventoryItem.SetQuantity(item.itemData.quantity);

            switch (item.savedSlotType)
            {
                case SlotSavedType.Inventory:
                    inventorySlots[item.slotUsed].SetItemInSlot(newInventoryItem);
                    break;
                default:
                    equippedSlots[item.slotUsed].SetItemInSlot(newInventoryItem);
                    break;
            }
        }
    }
    public void UpdateItemQuantity(InventoryItemData updatedInventoryItemData)
    {
        int newQuantity = updatedInventoryItemData.itemData.quantity;
        switch (updatedInventoryItemData.savedSlotType)
        {
            case SlotSavedType.Inventory:
                inventorySlots[updatedInventoryItemData.slotUsed].GetItem().SetQuantity(newQuantity);
                break;
            default:
                equippedSlots[updatedInventoryItemData.slotUsed].GetItem().SetQuantity(newQuantity);
                break;
        }
    }
    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
    }
    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        LevelManager.Instance.ShowInventory(false);
        ShowTooltip(false);
    }
    public int GetFreeSlot()
    {
        var freeSlot = inventorySlots.FirstOrDefault(item => item.Value.GetItem() == null);
        if (freeSlot.Value != null) { return freeSlot.Key; }
        Debug.LogWarning("No free inventory slots available.");
        return -1;
    }
    public void ShowTooltip(bool state, string itemName = "") => tooltip.Show(state, itemName);
    #endregion
}
