using TMPro;
using UnityEngine;

public class PickableItem : MonoBehaviour, IPickableItem
{
    [SerializeField] private ItemData itemData = null;
    [SerializeField] private TextMeshPro textMeshPro = null;
    [SerializeField] private bool saveCollected = false;
    [SerializeField] private bool floatAnimate = false;
    [SerializeField] private string floatingBool = "float";
    private Animator animator = null;

    private void Awake()
    {
        textMeshPro.text = $"x{itemData.quantity}";
        if (saveCollected) { LevelManager.Instance.OnSceneStarted += CheckItemCollected; }
        animator = GetComponent<Animator>();
        if (floatAnimate) { animator.SetBool(floatingBool, true); }
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
