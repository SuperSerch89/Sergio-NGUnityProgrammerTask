using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    #region SerializableFields
    [SerializeField] private GameObject interactPrompt = null;
    #endregion
    #region Private Fields
    private List<IInteractable> interactablesInRange = new List<IInteractable>();
    #endregion

    #region Unity Life Cycle
    private void Start()
    {
        ShowInteractPrompt(interactablesInRange.Count == 0 ? false : true);
    }
    #endregion

    public IInteractable GetClosestInteractable()
    {
        if (interactablesInRange.Count == 0) { return null; }
        return interactablesInRange[0];
    }
    private void ShowInteractPrompt(bool state) => interactPrompt.SetActive(state);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var interactable = collision.GetComponentInParent<IInteractable>();
        if (interactable != null && !interactablesInRange.Contains(interactable))
        {
            interactablesInRange.Add(interactable);
        }
        ShowInteractPrompt(interactablesInRange.Count == 0 ? false : true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var interactable = collision.GetComponentInParent<IInteractable>();
        if (interactable != null)
        {
            interactablesInRange.Remove(interactable);
        }
        ShowInteractPrompt(interactablesInRange.Count == 0 ? false : true);
    }
}
