using UnityEngine;

public class MouseInventoryCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        IPickableItem pickable = other.GetComponent<IPickableItem>();
        if (pickable != null) { pickable.OnPickUp(); }
    }
}
