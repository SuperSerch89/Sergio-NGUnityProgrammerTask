using NicoUtilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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
    #endregion
    #region Private Fields
    private Dictionary<int, InventorySlot> inventorySlots = new Dictionary<int, InventorySlot>();
    private Dictionary<int, InventorySlot> equippedSlots = new Dictionary<int, InventorySlot>();
    private Coroutine currentRoutine;
    #endregion
    #region Delegates
    public UnityAction DialogueFinishedCallback = null;
    #endregion

    #region Unity Overriden Methods
    protected override void Awake()
    {
        base.Awake();
        GetSlots();
    }
    #endregion
    #region Inventory Methods
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
    public void ShowTooltip(bool state, string itemName = "") => tooltip.Show(state, itemName);
    #region Coroutines
    private IEnumerator DialogueCoroutine()
    {
        yield return new WaitForSeconds(showingDuration);
        currentRoutine = null;
        CloseDialogue();
    }
    #endregion
}
