using UnityEngine;

public class CatShop : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialogue openShopDialogue = null;
    private bool showedDialogue = false;
    private bool openingShop = false;

    public void Interact()
    {
        if (openingShop) { return; }
        openingShop = true;

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
        Debug.Log("Opening shop");
    }
}
