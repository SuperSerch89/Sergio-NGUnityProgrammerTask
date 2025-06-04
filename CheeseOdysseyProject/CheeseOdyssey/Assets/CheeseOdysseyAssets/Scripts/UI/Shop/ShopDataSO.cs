using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShopDataSO : ScriptableObject
{
    public List<Product> products = new List<Product>();
}
[System.Serializable]
public class Product
{
    public ItemData itemData = null;
    public List<ItemData> requiredMaterials = new List<ItemData>();
}