using System;
using Zenject;

namespace InventorySystem
{
    public class ItemData
    {
        public event Action AmountChange; 

        private int _itemId;
        private bool _isStackable;
        private ItemType _itemType;
        private int _itemAmount;
        public EquipmentSlot InSlot;
        public bool BelongToPlayer;
        public bool InStash;

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

        public ItemData()
        {
            _itemId = 0;
            _itemAmount = 1;
        }
        
        public ItemData(int id, AllItemsContainer container)
        {
            _itemId = id;
            var config = container.GetConfigById(_itemId);
            _isStackable = config.IsStackable;
            _itemType = config.Type;
            _itemAmount = 1;
        }

        public void CopyData(ItemData refItem)
        {
            _itemId = refItem.ItemId;
            _isStackable = refItem.IsStackable;
            _itemType = refItem.Type;
            ItemAmount = refItem.ItemAmount;
        }
    }
}