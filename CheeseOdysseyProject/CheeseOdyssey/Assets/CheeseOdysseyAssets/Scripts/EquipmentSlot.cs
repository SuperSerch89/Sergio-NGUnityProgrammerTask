using UnityEngine;
using UnityEngine.UI;

public class EquippmentSlot : InventorySlot
{
    #region SerializedFields
    [SerializeField] private Text description = null;
    [SerializeField] private string emptySlotDescrition = "EMPTY";
    #endregion

    #region Overriden Methods
    public override void SetItemInSlot(InventoryItem newItem)
    {
        base.SetItemInSlot(newItem);
        if (newItem == null) { return; }
        description.text = itemInSlot.ItemSO.description;
    }
    public override void ClearSlot()
    {
        base.ClearSlot();
        description.text = emptySlotDescrition;
    }
    #endregion
}
