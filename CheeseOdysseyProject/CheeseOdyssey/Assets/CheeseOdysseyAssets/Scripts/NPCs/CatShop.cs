using UnityEngine;

public class CatShop : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialogue openShopDialogue = null;
    [SerializeField] private ShopDataSO shopProductsData = null;

    private bool showedDialogue = false;
    private bool shopOpened = false;

    public void Interact()
    {
        if (shopOpened) { return; }
        shopOpened = true;

        if (!showedDialogue)
        {
            LevelManager.Instance.ShowDialogue(openShopDialogue, OpenShop);
            showedDialogue = true;
            return;
        }
        OpenShop();
    }
    private void OpenShop()
    {
        LevelManager.Instance.ShowShop(true, shopProductsData, OnShopClosed);
    }
    private void OnShopClosed()
    {
        shopOpened = false;
    }
}
