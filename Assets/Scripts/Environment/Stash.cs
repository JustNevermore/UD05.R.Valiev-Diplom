using System.Collections.Generic;
using InventorySystem;
using Player;
using SaveSystem;
using Signals;
using UnityEngine;

namespace Environment
{
    public class Stash : Chest
    {
        private void Awake()
        {
            ItemsInChest = new List<ItemData>();
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<PlayerController>())
            {
                ChestWindow.OpenChestWindow(this, ItemsInChest);
                SignalBus.Fire<OpenChestSignal>();
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.GetComponent<PlayerController>())
            {
                SignalBus.Fire<CloseChestSignal>();
            }
        }

        public override void UpdateChestLoot(List<ItemData> items)
        {
            ItemsInChest = items;
        }
        
        public ItemSaveData[] GetStashData()
        {
            ItemSaveData[] data = new ItemSaveData[ItemsInChest.Count];

            for (int i = 0; i < ItemsInChest.Count; i++)
            {
                data[i] = new ItemSaveData(ItemsInChest[i].ItemId, ItemsInChest[i].ItemAmount,
                    ItemsInChest[i].InSlot);
            }

            return data;
        }
        
        public void SetStashData(ItemSaveData[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                var itemData = new ItemData(data[i].ItemId, Container);
                itemData.BelongToPlayer = true;
                itemData.InStash = true;
                itemData.InSlot = data[i].InSlot;
                itemData.ItemAmount = data[i].ItemAmount;
                ItemsInChest.Add(itemData);
            }
        }
    }
}