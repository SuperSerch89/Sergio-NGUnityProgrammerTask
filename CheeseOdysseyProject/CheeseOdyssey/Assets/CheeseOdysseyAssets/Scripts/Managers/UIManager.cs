using NicoUtilities;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    #region Serialized Fields
    [SerializeField] private GameObject inventoryPanel = null;
    [SerializeField] private GameObject inventoryItemPrefab = null;
    [SerializeField] private List<Item> allItems = null;
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
    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
    }
    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        LevelManager.Instance.ShowInventory(false);
    }
    #endregion
}
