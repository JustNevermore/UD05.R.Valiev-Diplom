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
        public EquipmentSlot InSlot;

        public ItemSaveData()
        {
            ItemId = 0;
            ItemAmount = 1;
            InSlot = EquipmentSlot.None;
        }

        public ItemSaveData(int id, int amount, EquipmentSlot inSlot)
        {
            ItemId = id;
            ItemAmount = amount;
            InSlot = inSlot;
        }
    }
}