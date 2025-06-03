using TMPro;
using UnityEngine;

public class PickableItem : MonoBehaviour, IPickableItem
{
    [SerializeField] private ItemData itemData = null;
    [SerializeField] private TextMeshPro textMeshPro = null;

    private void Awake() => textMeshPro.text = $"x{itemData.quantity}";
    public void OnPickUp()
    {
        if (itemData.quantity > 0) { MouseController.Instance.AddItem(itemData); }
        Destroy(gameObject);
    }
}
