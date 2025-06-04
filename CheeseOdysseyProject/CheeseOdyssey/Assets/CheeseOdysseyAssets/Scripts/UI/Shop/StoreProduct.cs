using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreProduct : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;

    private Item item = null;
    #region Accessors
    public Item Item {  get { return item; } }
    #endregion

    public void Setup(Item itemSO)
    {
        icon.sprite = itemSO.icon;
        nameText.text = itemSO.itemName;
        item = itemSO;
    }

    public void ItemPressed()
    {
        UIManager.Instance.ShowSelectedProductDetails(item);
    }
}
