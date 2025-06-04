using NicoUtilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    #region Serialized Fields
    [Header("Inventory")]
    [SerializeField] private GameObject inventoryPanel = null;
    [SerializeField] private GameObject inventoryItemPrefab = null;
    [SerializeField] private List<Item> allItems = null;
    [SerializeField] private Tooltip tooltip = null;
    [Header("Dialogues")]
    [SerializeField] private Animator dialoguesAnimator = null;
    [SerializeField] private TextMeshProUGUI dialogueNameLabel = null;
    [SerializeField] private TextMeshProUGUI dialogueLabel = null;
    [SerializeField] private string dialogueOpenBool= "open";
    [SerializeField] private string dialogueCloseBool= "close";
    [SerializeField] private float showingDuration = 1f;
    [Header("Shop")]
    [SerializeField] private GameObject shopPanel = null;
    [SerializeField] private Transform allProductsTransform = null;
    [SerializeField] private TextMeshProUGUI productNameLabel = null;
    [SerializeField] private Image productIconLabel = null;
    [SerializeField] private TextMeshProUGUI productDescriptionLabel = null;
    [SerializeField] private List<StoreItemComponent> storeItemComponents = new List<StoreItemComponent>();
    [SerializeField] private GameObject craftSuccessPanel = null;
    [SerializeField] private GameObject craftFailedPanel = null;
    #endregion
    #region Private Fields
    private Dictionary<int, InventorySlot> inventorySlots = new Dictionary<int, InventorySlot>();
    private Dictionary<int, InventorySlot> equippedSlots = new Dictionary<int, InventorySlot>();
    private Coroutine currentRoutine;
    private List<StoreProduct> storeProducts = new List<StoreProduct>();
    private Item selectedItem = null;
    private ShopDataSO selectedShopProductsData = null;
    private Product selectedProduct = null;
    #endregion
    #region Delegates
    public UnityAction DialogueFinishedCallback = null;
    public UnityAction ShopClosedCallback = null;
    #endregion

    #region Unity Overriden Methods
    protected override void Awake()
    {
        base.Awake();
        GetInventorySlots();
        GetStoreProductSlots();
    }
    #endregion
    #region Inventory Methods
    private void GetInventorySlots()
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
    public void SetupInventoryItems(List<InventoryItemData> inventoryItems)
    {
        foreach (InventoryItemData item in inventoryItems)
        {
            if (item == null) { continue; }

            GameObject newItem = Instantiate(inventoryItemPrefab);
            InventoryItem newInventoryItem = newItem.GetComponent<InventoryItem>();
            Item itemSO = FindItemSO(item.itemData.itemID);
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
        InventorySlot targetSlot = null;
        switch (updatedInventoryItemData.savedSlotType)
        {
            case SlotSavedType.Inventory:
                targetSlot = inventorySlots[updatedInventoryItemData.slotUsed];
                break;
            default:
                targetSlot = equippedSlots[updatedInventoryItemData.slotUsed];
                break;
        }
        InventoryItem item = targetSlot.GetItem();
        if (item == null)
        {
            Debug.LogWarning($"Tried to update quantity but no item found in slot {updatedInventoryItemData.slotUsed}");
            return;
        }

        if (newQuantity <= 0)
        {
            Destroy(item.gameObject);
            targetSlot.ClearSlot();
        }
        else
        {
            item.SetQuantity(newQuantity);
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
    #endregion
    #region Dialogue Methods
    public void ShowDialogue(string text, Dialogue dialogue, UnityAction OnDialogueFinishedShowing = null)
    {
        if (currentRoutine != null) { StopCoroutine(currentRoutine); }
        DialogueFinishedCallback = OnDialogueFinishedShowing;

        dialoguesAnimator.SetBool(dialogueOpenBool, true);
        dialoguesAnimator.SetBool(dialogueCloseBool, false);
        dialogueNameLabel.text = dialogue.characterName;
        dialogueLabel.text = text;
        currentRoutine = StartCoroutine(DialogueCoroutine());
    }
    private void CloseDialogue()
    {
        dialoguesAnimator.SetBool(dialogueOpenBool, false);
        dialoguesAnimator.SetBool(dialogueCloseBool, true);
        DialogueFinishedCallback?.Invoke();
    }
    #endregion
    #region Shop Methods
    private void GetStoreProductSlots()
    {
        foreach(Transform child in allProductsTransform)
        {
            StoreProduct storeProduct = child.GetComponent<StoreProduct>();
            if (storeProduct == null) { continue; }
            storeProducts.Add(storeProduct);
            storeProduct.gameObject.SetActive(false);
        }
    }
    public void OpenShop(ShopDataSO shopProductsData, UnityAction OnShopClosed = null)
    {
        ShopClosedCallback = OnShopClosed;
        SetupShop(shopProductsData);
        shopPanel.SetActive(true);
    }
    public void CloseShop()
    {
        shopPanel.SetActive(false);
        LevelManager.Instance.ShowShop(false);
        ShowTooltip(false);
        ShopClosedCallback?.Invoke();
    }
    private void SetupShop(ShopDataSO shopProductsData)
    {
        foreach (StoreProduct product in storeProducts) { product.gameObject.SetActive(false); }
        selectedShopProductsData = shopProductsData;
        for (int i = 0; i < shopProductsData.products.Count; i++)
        {
            if (i >= storeProducts.Count)
            {
                Debug.LogError("Not enough product slots to place all store items.");
                break;
            }
            Item itemSO = FindItemSO(shopProductsData.products[i].itemData.itemID);
            storeProducts[i].Setup(itemSO);
            storeProducts[i].gameObject.SetActive(true);
        }
        if(storeProducts.Count > 0)
        {
            ShowSelectedProductDetails(storeProducts[0].Item);
        }
    }
    public void ShowSelectedProductDetails(Item itemToShow)
    {
        selectedItem = itemToShow;
        productNameLabel.text = itemToShow.itemName;
        productIconLabel.sprite = itemToShow.icon;
        productDescriptionLabel.text = itemToShow.description;

        foreach (StoreItemComponent itemComponent in storeItemComponents) { itemComponent.gameObject.SetActive(false); }
        Product foundProduct = selectedShopProductsData.products.Find(product => product.itemData.itemID == itemToShow.itemID);
        if (foundProduct == null)
        {
            Debug.LogError("Couldn't find store product "+ itemToShow.itemID +", to show details.");
            return;
        }
        selectedProduct = foundProduct;
        InventoryController nibblesInventory = LevelManager.Instance.GetNibblesInventory();
        for (int i = 0; i < selectedProduct.requiredMaterials.Count; i++)
        {
            if (i >= storeItemComponents.Count)
            {
                Debug.LogError("Not enough storeItemComponents to show all requiredMaterials.");
                break;
            }
            storeItemComponents[i].gameObject.SetActive(true);
            Item itemSO = FindItemSO(selectedProduct.requiredMaterials[i].itemID);
            int nibblesHeldComponentQuantity = nibblesInventory.GetItemQuantity(itemSO.itemID);
            storeItemComponents[i].Setup(selectedProduct.requiredMaterials[i], itemSO.icon, nibblesHeldComponentQuantity);
        }
    }
    public void TryCrafting()
    {
        InventoryController nibblesInventory = LevelManager.Instance.GetNibblesInventory();
        bool craftAvailable = true;
        List<ItemData> modifiedItemDatas = new List<ItemData>();
        foreach (ItemData component in selectedProduct.requiredMaterials)
        {
            int heldQuantity = nibblesInventory.GetItemQuantity(component.itemID);
            if (heldQuantity < component.quantity)
            {
                craftAvailable = false;
                break;
            }
            modifiedItemDatas.Add(new ItemData(component.itemID, heldQuantity - component.quantity));
        }
        if (!craftAvailable)
        {
            craftFailedPanel.SetActive(true);
            return;
        }
        craftSuccessPanel.SetActive(true);
        foreach(ItemData modifiedItem in modifiedItemDatas)
        {
            nibblesInventory.ModifyItemQuantity(modifiedItem);
        }
        nibblesInventory.AddItem(selectedProduct.itemData);
        SetupShop(selectedShopProductsData);
    }
    #endregion
    #region Tooltip Methods
    public void ShowTooltip(bool state, string itemName = "") => tooltip.Show(state, itemName);
    #endregion
    #region Utilities
    private Item FindItemSO(ItemID itemIDSearched)
    {
        Item itemSO = allItems.Find(itemSO => itemSO.itemID == itemIDSearched);
        if (itemSO == null)
        {
            Debug.LogError($"Couldn't find {itemIDSearched} item scriptable Object.");
            return null;
        }
        return itemSO;
    }
    #endregion
    #region Coroutines
    private IEnumerator DialogueCoroutine()
    {
        yield return new WaitForSeconds(showingDuration);
        currentRoutine = null;
        CloseDialogue();
    }
    #endregion
}
