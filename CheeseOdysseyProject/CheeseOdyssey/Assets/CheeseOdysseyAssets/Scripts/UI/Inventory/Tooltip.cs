using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private Text textLabel = null;

    private void Update() => transform.position = Input.mousePosition;
    public void Show(bool active, string itemName = "")
    {
        if (active)
        {
            textLabel.text = itemName;
            gameObject.SetActive(true);
            return;
        }
        gameObject.SetActive(false);
    }
}
