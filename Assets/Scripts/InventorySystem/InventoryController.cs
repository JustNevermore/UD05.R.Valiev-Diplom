using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace InventorySystem
{
    public class InventoryController : MonoBehaviour
    {
        private InventoryWindow _inventoryWindow;

        private List<ItemData> _inventoryItems;
        public List<ItemData> InventoryItems => _inventoryItems;

        [SerializeField] private InventorySlot _weaponSlot;
        [SerializeField] private InventorySlot _necklaceSlot;
        [SerializeField] private InventorySlot _ringSlot;
        [SerializeField] private InventorySlot _armorSlot;
        [SerializeField] private InventorySlot _conSlot1;
        [SerializeField] private InventorySlot _conSlot2;
        [SerializeField] private InventorySlot _conSlot3;
        [SerializeField] private InventorySlot _conSlot4;

        private ItemData _weapon;
        private ItemData _necklace;
        private ItemData _ring;
        private ItemData _armor;
        private ItemData _cons1;
        private ItemData _cons2;
        private ItemData _cons3;
        private ItemData _cons4;

        public ItemData Weapon => _weapon;
        public ItemData Necklace => _necklace;
        public ItemData Ring => _ring;
        public ItemData Armor => _armor;
        public ItemData Cons1 => _cons1;
        public ItemData Cons2 => _cons2;
        public ItemData Cons3 => _cons3;
        public ItemData Cons4 => _cons4;


        [Inject]
        private void Construct(InventoryWindow inventoryWindow)
        {
            _inventoryWindow = inventoryWindow;
        }

        private void Start()
        {
            _inventoryItems = new List<ItemData>();
        }

        public void AddToInventory(ItemData item)
        {
            _inventoryItems.Add(item);
        }

        public void RemoveFromInventory(ItemData item)
        {
            _inventoryItems.Remove(item);
        }

        public void AddToSlot(EquipmentSlot slot, ItemData item)
        {
            switch (slot)
            {
                case EquipmentSlot.Weapon:
                    _weapon = item;
                    break;
                case EquipmentSlot.Necklace:
                    _necklace = item;
                    break;
                case EquipmentSlot.Ring:
                    _ring = item;
                    break;
                case EquipmentSlot.Armor:
                    _armor = item;
                    break;
                case EquipmentSlot.Cons1:
                    _cons1 = item;
                    break;
                case EquipmentSlot.Cons2:
                    _cons2 = item;
                    break;
                case EquipmentSlot.Cons3:
                    _cons3 = item;
                    break;
                case EquipmentSlot.Cons4:
                    _cons4 = item;
                    break;
            }
        }

        public void RemoveFromSlot(EquipmentSlot slot)
        {
            switch (slot)
            {
                case EquipmentSlot.Weapon:
                    _weapon = null;
                    break;
                case EquipmentSlot.Necklace:
                    _necklace = null;
                    break;
                case EquipmentSlot.Ring:
                    _ring = null;
                    break;
                case EquipmentSlot.Armor:
                    _armor = null;
                    break;
                case EquipmentSlot.Cons1:
                    _cons1 = null;
                    break;
                case EquipmentSlot.Cons2:
                    _cons2 = null;
                    break;
                case EquipmentSlot.Cons3:
                    _cons3 = null;
                    break;
                case EquipmentSlot.Cons4:
                    _cons4 = null;
                    break;
            }
        }
        
        private void RedrawInventory()
        {
            
        }
    }
}