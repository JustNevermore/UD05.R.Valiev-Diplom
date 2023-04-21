using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class AllItemsContainer : MonoBehaviour
    {
        [SerializeField] private List<ItemConfig> allItemsList = new List<ItemConfig>();

        private Dictionary<int, ItemConfig> allItemsDic = new Dictionary<int, ItemConfig>();

        public List<ItemConfig> AllItemsList => allItemsList;

        private void Awake()
        {
            var x = 1;
            foreach (var item in allItemsList)
            {
                item.SetId(x++);
            }

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