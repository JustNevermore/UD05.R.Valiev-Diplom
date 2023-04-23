using System;
using InventorySystem;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public class ItemSaveData
    {
        public int ItemId;
        public int ItemAmount;

        public ItemSaveData()
        {
            ItemId = 0;
            ItemAmount = 1;
        }

        public ItemSaveData(int id, int amount)
        {
            ItemId = id;
            ItemAmount = amount;
        }
    }
}