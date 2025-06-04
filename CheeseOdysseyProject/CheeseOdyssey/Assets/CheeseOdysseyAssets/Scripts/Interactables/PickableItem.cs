using TMPro;
using UnityEngine;

public class PickableItem : MonoBehaviour, IPickableItem
{
    [SerializeField] private ItemData itemData = null;
    [SerializeField] private TextMeshPro textMeshPro = null;
    [SerializeField] private bool saveCollected = false;

    private void Awake()
    {
        textMeshPro.text = $"x{itemData.quantity}";
        if (saveCollected) { LevelManager.Instance.OnSceneStarted += CheckItemCollected; }
    }
    private void CheckItemCollected()
    {
        int itemQuantity = LevelManager.Instance.CheckNibblesItemQuantity(itemData.itemID);
        gameObject.SetActive(itemQuantity == 0);
    }
    public void OnPickUp()
    {
        if (itemData.quantity > 0) { MouseController.Instance.InventoryController.AddItem(itemData); }
        Destroy(gameObject);
    }
}
