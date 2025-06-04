using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemComponent : MonoBehaviour
{
    [SerializeField] private Image icon = null;
    [SerializeField] private TextMeshProUGUI quantityNeededLabel = null;
    [SerializeField] private TextMeshProUGUI quantityHeldLabel = null;

    private ItemData itemData = null;
    #region Accessors
    public ItemData ItemData { get { return itemData; } }
    #endregion

    public void Setup(ItemData itemData, Sprite itemIcon, int heldQuantity)
    {
        this.itemData = itemData;
        icon.sprite = itemIcon;
        quantityNeededLabel.text = itemData.quantity.ToString();
        quantityHeldLabel.text = heldQuantity.ToString();
    }
}
