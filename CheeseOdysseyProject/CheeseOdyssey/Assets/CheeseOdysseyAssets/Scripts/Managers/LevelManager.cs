using NicoUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class LevelManager : Singleton<LevelManager>
{
    #region SerializedFields
    [SerializeField] private float UIOpenDelay = 0.2f;
    #endregion
    #region Private Methods
    [ReadOnly]private CurrentPanelOpened currentPanelOpened = CurrentPanelOpened.None;
    private bool openingPanel = false;
    #endregion

    #region Public Methods
    public virtual void StartScene()
    {
        currentPanelOpened = CurrentPanelOpened.None;
        //Retrieves saves data from the GameManager
        //Sets it into the Mousecontroller Inventory
        //Sets the inventory data into the InventoryUI
        //PlaceHolder data:
        /* Testing Data
        List<InventoryItemData> testInventory = new List<InventoryItemData>()
        {
            //new InventoryItemData( new ItemData(ItemID.JetpackBasic, 1, true), SlotSavedType.Equipped, 1),
            //new InventoryItemData( new ItemData(ItemID.Scrap, 10, false), SlotSavedType.Inventory, 0),
            //new InventoryItemData( new ItemData(ItemID.HelmetOrange, 1, false), SlotSavedType.Inventory, 3),
        };
        UIManager.Instance.SetupInventoryItems(testInventory);
        */
    }
    public void ShowInventory(bool stateOn)
    {
        if (stateOn && currentPanelOpened != CurrentPanelOpened.None) { return; }
        if (!stateOn && currentPanelOpened != CurrentPanelOpened.Inventory) { return; }
        if (openingPanel) { return; }
        StartCoroutine(OpeningDelayRountine());

        if (stateOn)
        {
            GameManager.Instance.StopTime();
            UIManager.Instance.OpenInventory();
            MouseController.Instance.SwitchActionMap(MouseController.InputMaps.UI);
            currentPanelOpened = CurrentPanelOpened.Inventory;
        }
        else
        {
            GameManager.Instance.ResumeTime();
            UIManager.Instance.CloseInventory();
            MouseController.Instance.SwitchActionMap(MouseController.InputMaps.Gameplay);
            currentPanelOpened = CurrentPanelOpened.None;
        }
    }
    public void ShowDialogue(Dialogue dialogue, UnityAction OnDialogueFinishedShowing = null)
    {
        UIManager.Instance.CloseInventory();
        UIManager.Instance.CloseShop();
        string resultText = DialogueCSVLoader.Instance.GetDialogueText(dialogue.dialogueID);
        UIManager.Instance.ShowDialogue(resultText, dialogue, OnDialogueFinishedShowing);
    }
    public void ShowShop(bool stateOn, ShopDataSO shopProductsData = null, UnityAction OnShopClosed = null)
    {
        if (stateOn && currentPanelOpened != CurrentPanelOpened.None) { return; }
        if (!stateOn && currentPanelOpened != CurrentPanelOpened.Shop) { return; }
        currentPanelOpened = CurrentPanelOpened.Shop;

        if (openingPanel) { return; }
        StartCoroutine(OpeningDelayRountine());

        if (stateOn)
        {
            GameManager.Instance.StopTime();
            UIManager.Instance.OpenShop(shopProductsData, OnShopClosed);
            MouseController.Instance.SwitchActionMap(MouseController.InputMaps.UI);
            currentPanelOpened = CurrentPanelOpened.Shop;
        }
        else
        {
            GameManager.Instance.ResumeTime();
            UIManager.Instance.CloseShop();
            MouseController.Instance.SwitchActionMap(MouseController.InputMaps.Gameplay);
            currentPanelOpened = CurrentPanelOpened.None;
        }
    }
    public InventoryController GetNibblesInventory() => MouseController.Instance.InventoryController;
    #endregion
    #region Coroutines
    private IEnumerator OpeningDelayRountine()
    {
        openingPanel = true;
        yield return new WaitForSecondsRealtime(UIOpenDelay);
        openingPanel = false;
    }
    #endregion
    public enum CurrentPanelOpened
    {
        None = 0,
        Inventory,
        Shop,
    }
}