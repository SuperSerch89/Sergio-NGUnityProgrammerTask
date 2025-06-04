using NicoUtilities;
using System.Collections;
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
    #region Delegates
    public UnityAction OnSceneStarted = null;
    #endregion

    #region Public Methods
    public virtual void StartScene()
    {
        currentPanelOpened = CurrentPanelOpened.None;
        MouseController.Instance.InventoryController.SetupInventory(GameManager.Instance.GameData.inventory);
        UIManager.Instance.SetupInventoryItems(GameManager.Instance.GameData.inventory);
        OnSceneStarted?.Invoke();
    }
    public virtual void PlaySFX(SFX sfxToPlay) { }
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
            GameManager.Instance.SaveGame();
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
            GameManager.Instance.SaveGame();
            MouseController.Instance.SwitchActionMap(MouseController.InputMaps.Gameplay);
            currentPanelOpened = CurrentPanelOpened.None;
        }
    }
    public InventoryController GetNibblesInventory() => MouseController.Instance.InventoryController;
    public void NibblesEquipmentChange(ItemType itemType, ItemID itemID) => MouseController.Instance.ChangeEquipment(itemType, itemID);
    public int CheckNibblesItemQuantity(ItemID itemID) => MouseController.Instance.InventoryController.GetItemQuantity(itemID);
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