using System;
using UnityEngine;

namespace InventorySystem
{
    public class ItemData
    {
        public event Action AmountChange; 

        private int _itemId;
        private bool _isStackable;
        private ItemType _itemType;
        private int _itemAmount = 1;

        public int ItemAmount
        {
            get => _itemAmount;
            set
            {
                _itemAmount = value;
                AmountChange?.Invoke();
            }
        }
        
        public int ItemId => _itemId;
        public bool IsStackable => _isStackable;
        public ItemType Type => _itemType;
        public bool BelongToPlayer;


        public void SetStatus(int id, bool stackable, ItemType type)
        {
            _itemId = id;
            _isStackable = stackable;
            _itemType = type;
        }

        public void CopyData(ItemData refItem)
        {
            _itemId = refItem.ItemId;
            _isStackable = refItem.IsStackable;
            _itemType = refItem.Type;
            ItemAmount = refItem.ItemAmount;
            BelongToPlayer = refItem.BelongToPlayer;
        }
    }
}