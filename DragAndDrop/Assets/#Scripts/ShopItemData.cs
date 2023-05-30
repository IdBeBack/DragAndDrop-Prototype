using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "ShopItem")]

public class ShopItemData : ScriptableObject
{
    public new string name;
    public GameObject prefab;
}