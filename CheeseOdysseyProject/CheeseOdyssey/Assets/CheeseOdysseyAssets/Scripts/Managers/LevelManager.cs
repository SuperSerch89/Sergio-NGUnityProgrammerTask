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
    private bool openingInventory = false;
    #endregion

    #region Public Methods
    public virtual void StartScene()
    {
        //Retrieves saves data from the GameManager
        //Sets it into the Mousecontroller Inventory
        //Sets the inventory data into the InventoryUI
        //PlaceHolder data:
        List<InventoryItemData> testInventory = new List<InventoryItemData>()
        {
            new InventoryItemData( new ItemData(ItemID.JetpackBasic, 1, true), SlotSavedType.Equipped, 1),
            //new InventoryItemData( new ItemData(ItemID.Scrap, 10, false), SlotSavedType.Inventory, 0),
            //new InventoryItemData( new ItemData(ItemID.HelmetOrange, 1, false), SlotSavedType.Inventory, 3),
        };
        UIManager.Instance.SetupInventoryItems(testInventory);
    }
    public void ShowInventory(bool stateOn)
    {
        if (openingInventory) { return; }
        StartCoroutine(OpeningDelayRountine());

        if (stateOn)
        {
            GameManager.Instance.StopTime();
            UIManager.Instance.OpenInventory();
            MouseController.Instance.SwitchActionMap(MouseController.InputMaps.UI);
        }
        else
        {
            GameManager.Instance.ResumeTime();
            UIManager.Instance.CloseInventory();
            MouseController.Instance.SwitchActionMap(MouseController.InputMaps.Gameplay);
        }
    }
    public void ShowDialogue(Dialogue dialogue, UnityAction OnDialogueFinishedShowing = null)
    {
        string resultText = DialogueCSVLoader.Instance.GetDialogueText(dialogue.dialogueID);
        UIManager.Instance.ShowDialogue(resultText, dialogue, OnDialogueFinishedShowing);
    }
    #endregion
    #region Coroutines
    private IEnumerator OpeningDelayRountine()
    {
        openingInventory = true;
        yield return new WaitForSecondsRealtime(UIOpenDelay);
        openingInventory = false;
    }
    #endregion
}