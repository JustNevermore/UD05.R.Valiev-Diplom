using System.Collections.Generic;
using Player;
using SaveSystem;
using UnityEngine;
using Zenject;

namespace InventorySystem
{
    public class InventoryController : MonoBehaviour
    {
        private const int ItemSlotsCount = 8;

        private DiContainer _diContainer;
        private AllItemsContainer _allItemsContainer;
        private InventoryWindow _inventoryWindow;
        private PlayerStats _playerStats;

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
        private void Construct(DiContainer diContainer, AllItemsContainer allItemsContainer, InventoryWindow inventoryWindow, PlayerStats playerStats)
        {
            _diContainer = diContainer;
            _allItemsContainer = allItemsContainer;
            _inventoryWindow = inventoryWindow;
            _playerStats = playerStats;
            
            _inventoryItems = new List<ItemData>();
        }

        private void Start()
        {
            
        }

        private void InitInventory()
        {
            FillItemSlot(_weapon, _weaponSlot);
            FillItemSlot(_necklace, _necklaceSlot);
            FillItemSlot(_ring, _ringSlot);
            FillItemSlot(_armor, _armorSlot);
            FillItemSlot(_cons1, _conSlot1);
            FillItemSlot(_cons2, _conSlot2);
            FillItemSlot(_cons3, _conSlot3);
            FillItemSlot(_cons4, _conSlot4);
            
            foreach (var item in _inventoryItems)
            {
                FillItemSlot(item, _inventoryWindow);
            }
            
            EquipItemInSlot(_weapon);
            EquipItemInSlot(_necklace);
            EquipItemInSlot(_ring);
            EquipItemInSlot(_armor);
        }

        private void FillItemSlot(ItemData item, Component parent)
        {
            if (item != null)
            {
                var newItem = _diContainer.InstantiateComponent<Item>(new GameObject("Item"));
                newItem.transform.SetParent(parent.transform);
                newItem.Init(item);
            }
        }

        private void EquipItemInSlot(ItemData item)
        {
            if (item != null)
            {
                _playerStats.IncreaseStats(_allItemsContainer.GetConfigById(item.ItemId));
            }
        }

        public void SetInventoryData(ItemSaveData[] data)
        {
            _weapon = SetItemData(data[0]);
            _necklace = SetItemData(data[1]);
            _ring = SetItemData(data[2]);
            _armor = SetItemData(data[3]);
            _cons1 = SetItemData(data[4]);
            _cons2 = SetItemData(data[5]);
            _cons3 = SetItemData(data[6]);
            _cons4 = SetItemData(data[7]);

            for (int i = ItemSlotsCount; i < data.Length; i++)
            {
                _inventoryItems.Add(SetItemData(data[i]));
            }
            
            InitInventory();
        }

        private ItemData SetItemData(ItemSaveData data)
        {
            if (data.ItemId != 0)
            {
                var newItem = new ItemData(data.ItemId, _allItemsContainer);
                newItem.ItemAmount = data.ItemAmount;
                newItem.BelongToPlayer = true;
                return newItem;
            }

            return null;
        }

        public ItemSaveData[] GetInventoryData()
        {
            var count = _inventoryItems.Count + ItemSlotsCount;
            var data = new ItemSaveData[count];

            data[0] = GetItemData(_weapon);
            data[1] = GetItemData(_necklace);
            data[2] = GetItemData(_ring);
            data[3] = GetItemData(_armor);
            data[4] = GetItemData(_cons1);
            data[5] = GetItemData(_cons2);
            data[6] = GetItemData(_cons3);
            data[7] = GetItemData(_cons4);

            for (int i = ItemSlotsCount, x = 0; i < data.Length && x < _inventoryItems.Count; i++, x++)
            {
                data[i] = GetItemData(_inventoryItems[x]);
            }

            return data;
        }

        private ItemSaveData GetItemData(ItemData data)
        {
            if (data != null)
            {
                var saveData = new ItemSaveData(data.ItemId, data.ItemAmount);
                return saveData;
            }
            else
            {
                var saveData = new ItemSaveData();
                return saveData;
            }
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