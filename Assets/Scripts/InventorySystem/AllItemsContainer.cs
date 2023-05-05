using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    public class AllItemsContainer : MonoBehaviour
    {
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private Sprite borderImage;
        [SerializeField] private float fontSize = 50;
        [SerializeField] private Color textColor = Color.red;
        
        [SerializeField] private List<ItemConfig> allItemsList = new List<ItemConfig>();

        private Dictionary<int, ItemConfig> allItemsDic = new Dictionary<int, ItemConfig>();

        public List<ItemConfig> AllItemsList => allItemsList;
        public Sprite BorderImage => borderImage;
        public float FontSize => fontSize;
        public Color TextColor => textColor;
        public GameObject ItemPrefab => itemPrefab;

        private void Awake()
        {
            var x = 1;
            foreach (var item in allItemsList)
            {
                item.SetId(x++);
            }

            var orderedByRarity = from item in allItemsList
                orderby item.Rarity
                select item;

            allItemsList = orderedByRarity.ToList();

            foreach (var item in allItemsList)
            {
                allItemsDic.Add(item.ItemId, item);
            }
        }

        public ItemConfig GetConfigById(int id)
        {
            return allItemsDic[id];
        }
    }
}