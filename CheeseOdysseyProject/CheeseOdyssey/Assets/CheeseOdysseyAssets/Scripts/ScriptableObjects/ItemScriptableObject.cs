using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public ItemID itemID = ItemID.Empty;
    public string itemName = string.Empty;
    public string description = string.Empty;
    public Sprite icon = null;
    public ItemType itemType = ItemType.Material;
}

public enum ItemID
{
    Empty = 0,
    Scrap,
    EnergyCell,
    BlueCheese,
    GreenGel,
    JetpackBasic = 100,
    JetpackImproved,
    HelmetBlue = 200,
    HelmetOrange,
    HelmetPink,
    HelmetPurple,
}