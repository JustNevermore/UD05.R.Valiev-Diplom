using UnityEngine;

namespace InventorySystem
{
    public class ItemConfig : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField] private Sprite itemIcon;
        [SerializeField] private int itemCost;
        [SerializeField] private int rarity;
        [SerializeField] private string description;
    }
}