using System.Collections.Generic;
using Player;
using SaveSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace InventorySystem
{
    public class InventoryWindow : MonoBehaviour, IDropHandler
    {
        private DiContainer _diContainer;
        private AllItemsContainer _allItemsContainer;
        private PlayerStats _playerStats;
        private ChestWindow _chestWindow;
        
        [SerializeField] private GameObject contentWindow;
        [Space]
        
        [SerializeField] private InventorySlot weaponSlot;
        [SerializeField] private InventorySlot necklaceSlot;
        [SerializeField] private InventorySlot ringSlot;
        [SerializeField] private InventorySlot armorSlot;
        [SerializeField] private InventorySlot conSlot1;
        [SerializeField] private InventorySlot conSlot2;
        [SerializeField] private InventorySlot conSlot3;
        [SerializeField] private InventorySlot conSlot4;

        private List<ItemData> _inventoryItems;

        public List<ItemData> InventoryItems => _inventoryItems;
        
        
        public InventorySlot WeaponSlot => weaponSlot;
        public InventorySlot NecklaceSlot => necklaceSlot;
        public InventorySlot RingSlot => ringSlot;
        public InventorySlot ArmorSlot => armorSlot;
        public InventorySlot ConSlot1 => conSlot1;
        public InventorySlot ConSlot2 => conSlot2;
        public InventorySlot ConSlot3 => conSlot3;
        public InventorySlot ConSlot4 => conSlot4;
        

        [Inject]
        private void Construct(DiContainer diContainer, AllItemsContainer allItemsContainer, PlayerStats playerStats, ChestWindow chestWindow)
        {
            _diContainer = diContainer;
            _allItemsContainer = allItemsContainer;
            _playerStats = playerStats;
            _chestWindow = chestWindow;
        }

        private void Start()
        {
            _inventoryItems = new List<ItemData>();
            
            // todo подгрузить инвентарь из сейва
        }

        public ItemData AddItemToInventory(ItemData item, EquipmentSlot inSlot)
        {
            var itemDuplicate = new ItemData();
            itemDuplicate.CopyData(item);
            itemDuplicate.BelongToPlayer = true;
            itemDuplicate.InSlot = inSlot;
            _inventoryItems.Add(itemDuplicate);
            
            RedrawInventory();

            return itemDuplicate;
        }

        public void DeleteItemFromInventory(ItemData item)
        {
            _inventoryItems.Remove(item);
            
            RedrawInventory();
        }

        public void RedrawInventory()
        {
            foreach (var item in contentWindow.GetComponentsInChildren<Item>())
            {
                Destroy(item.gameObject);
            }

            foreach (var item in weaponSlot.GetComponentsInChildren<Item>())
            {
                Destroy(item.gameObject);
            }
            foreach (var item in necklaceSlot.GetComponentsInChildren<Item>())
            {
                Destroy(item.gameObject);
            }
            foreach (var item in ringSlot.GetComponentsInChildren<Item>())
            {
                Destroy(item.gameObject);
            }
            foreach (var item in armorSlot.GetComponentsInChildren<Item>())
            {
                Destroy(item.gameObject);
            }
            foreach (var item in conSlot1.GetComponentsInChildren<Item>())
            {
                Destroy(item.gameObject);
            }
            foreach (var item in conSlot2.GetComponentsInChildren<Item>())
            {
                Destroy(item.gameObject);
            }
            foreach (var item in conSlot3.GetComponentsInChildren<Item>())
            {
                Destroy(item.gameObject);
            }
            foreach (var item in conSlot4.GetComponentsInChildren<Item>())
            {
                Destroy(item.gameObject);
            }
            
            
            foreach (var item in _inventoryItems)
            {
                var newItem = _diContainer.InstantiateComponent<Item>(new GameObject("Item"));
                newItem.Init(item);

                switch (item.InSlot)
                {
                    case EquipmentSlot.None:
                        newItem.transform.SetParent(contentWindow.transform);
                        break;
                    case EquipmentSlot.Weapon:
                        newItem.transform.SetParent(weaponSlot.transform);
                        break;
                    case EquipmentSlot.Necklace:
                        newItem.transform.SetParent(necklaceSlot.transform);
                        break;
                    case EquipmentSlot.Ring:
                        newItem.transform.SetParent(ringSlot.transform);
                        break;
                    case EquipmentSlot.Armor:
                        newItem.transform.SetParent(armorSlot.transform);
                        break;
                    case EquipmentSlot.Cons1:
                        newItem.transform.SetParent(conSlot1.transform);
                        break;
                    case EquipmentSlot.Cons2:
                        newItem.transform.SetParent(conSlot2.transform);
                        break;
                    case EquipmentSlot.Cons3:
                        newItem.transform.SetParent(conSlot3.transform);
                        break;
                    case EquipmentSlot.Cons4:
                        newItem.transform.SetParent(conSlot4.transform);
                        break;
                }
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            var obj = eventData.pointerDrag;
            if (obj == null)
                return;

            if (obj.GetComponent<Item>())
            {
                var anchor = obj.GetComponent<DragDrop>().DragAnchor;
                var item = obj.GetComponent<Item>().Data;
                var itemConfig = _allItemsContainer.GetConfigById(item.ItemId);
            
                obj.GetComponent<DragDrop>().SetDropFlag();

                // покупаем
                if (!item.BelongToPlayer)
                {
                    if (_playerStats.CurrentGold > itemConfig.ItemCost) // хватает ли золота
                    {
                        if (item.IsStackable) // предмет стакается
                        {
                            var flag = false;
                            
                            foreach (var invItem in _inventoryItems) // ищем стакаемый предмет
                            {
                                if (item.ItemId == invItem.ItemId)
                                {
                                    flag = true;
                                    invItem.ItemAmount += item.ItemAmount;
                                }
                            }

                            if (!flag)
                            {
                                AddItemToInventory(item, EquipmentSlot.None);
                            }
                            
                            obj.transform.position = anchor;
                        }
                        else // предмет не стакается
                        {
                            AddItemToInventory(item, EquipmentSlot.None);
                            
                            obj.transform.position = anchor;
                        }
                        
                        _playerStats.DecreaseGold(itemConfig.ItemCost * item.ItemAmount);
                    }
                    else
                    {
                        obj.transform.position = anchor;
                    }
                }
                else if (item.InStash) // перемещаем из сундука
                {
                    if (item.IsStackable) // предмет стакается
                    {
                        var flag = false;

                        foreach (var invItem in _inventoryItems) // ищем стакаемый предмет
                        {
                            if (item.ItemId == invItem.ItemId)
                            {
                                flag = true;
                                invItem.ItemAmount += item.ItemAmount;
                                _chestWindow.RemoveFromChest(item);
                            }
                        }

                        if (!flag)
                        {
                            if (item.Type == ItemType.Consumable)
                            {
                                _chestWindow.RemoveFromChest(item);
                                var newItem = AddItemToInventory(item, EquipmentSlot.None);
                                newItem.InStash = false;
                            }
                            else
                            {
                                _chestWindow.RemoveFromChest(item);
                                var newItem = AddItemToInventory(item, EquipmentSlot.None);
                                newItem.InStash = false;
                            }
                        }
                    }
                    else // предмет не стакается
                    {
                        if (item.Type == ItemType.Consumable)
                        {
                            _chestWindow.RemoveFromChest(item);
                            var newItem = AddItemToInventory(item, EquipmentSlot.None);
                            newItem.InStash = false;
                        }
                        else
                        {
                            _chestWindow.RemoveFromChest(item);
                            var newItem = AddItemToInventory(item, EquipmentSlot.None);
                            newItem.InStash = false;
                        }
                    }
                }
                else // перемещаем из слота
                {
                    if (item.InSlot != EquipmentSlot.None)
                    {
                        if (item.Type == ItemType.Consumable) // проверка на понижение статов для экипировки
                        {
                            switch (item.InSlot)
                            {
                                case EquipmentSlot.Cons1:
                                    conSlot1.RemoveItemFromSlot();
                                    break;
                                case EquipmentSlot.Cons2:
                                    conSlot2.RemoveItemFromSlot();
                                    break;
                                case EquipmentSlot.Cons3:
                                    conSlot3.RemoveItemFromSlot();
                                    break;
                                case EquipmentSlot.Cons4:
                                    conSlot4.RemoveItemFromSlot();
                                    break;
                            }
                            
                            item.InSlot = EquipmentSlot.None;
                        }
                        else
                        {
                            switch (item.InSlot)
                            {
                                case EquipmentSlot.Weapon:
                                    weaponSlot.RemoveItemFromSlot();
                                    break;
                                case EquipmentSlot.Necklace:
                                    necklaceSlot.RemoveItemFromSlot();
                                    break;
                                case EquipmentSlot.Ring:
                                    ringSlot.RemoveItemFromSlot();
                                    break;
                                case EquipmentSlot.Armor:
                                    armorSlot.RemoveItemFromSlot();
                                    break;
                                case EquipmentSlot.Cons1:
                                    conSlot1.RemoveItemFromSlot();
                                    break;
                                case EquipmentSlot.Cons2:
                                    conSlot2.RemoveItemFromSlot();
                                    break;
                                case EquipmentSlot.Cons3:
                                    conSlot3.RemoveItemFromSlot();
                                    break;
                                case EquipmentSlot.Cons4:
                                    conSlot4.RemoveItemFromSlot();
                                    break;
                            }
                            
                            item.InSlot = EquipmentSlot.None;
                            _playerStats.DecreaseStats(itemConfig);
                        }
                        
                        RedrawInventory();
                    }
                    else
                    {
                        obj.transform.position = anchor;
                    }
                }
            }
        }

        public ItemSaveData[] GetInventoryData()
        {
            ItemSaveData[] data = new ItemSaveData[_inventoryItems.Count];

            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                data[i] = new ItemSaveData(_inventoryItems[i].ItemId, _inventoryItems[i].ItemAmount,
                    _inventoryItems[i].InSlot);
            }

            return data;
        }

        public void SetInventoryData(ItemSaveData[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                var itemData = new ItemData(data[i].ItemId, _allItemsContainer);
                itemData.BelongToPlayer = true;
                itemData.InSlot = data[i].InSlot;
                itemData.ItemAmount = data[i].ItemAmount;
                _inventoryItems.Add(itemData);
            }
            
            FillSlots();
            RedrawInventory();
        }

        private void FillSlots()
        {
            foreach (var item in _inventoryItems)
            {
                if (item.InSlot != EquipmentSlot.None)
                {
                    switch (item.InSlot)
                    {
                        case EquipmentSlot.Weapon:
                            weaponSlot.AddItemToSlot(item);
                            break;
                        case EquipmentSlot.Necklace:
                            necklaceSlot.AddItemToSlot(item);
                            break;
                        case EquipmentSlot.Ring:
                            ringSlot.AddItemToSlot(item);
                            break;
                        case EquipmentSlot.Armor:
                            armorSlot.AddItemToSlot(item);
                            break;
                        case EquipmentSlot.Cons1:
                            conSlot1.AddItemToSlot(item);
                            break;
                        case EquipmentSlot.Cons2:
                            conSlot2.AddItemToSlot(item);
                            break;
                        case EquipmentSlot.Cons3:
                            conSlot3.AddItemToSlot(item);
                            break;
                        case EquipmentSlot.Cons4:
                            conSlot4.AddItemToSlot(item);
                            break;
                    }
                }
            }
        }

        public void RemoveItem(ItemData item)
        {
            if (item.InSlot != EquipmentSlot.None)
            {
                switch (item.InSlot)
                {
                    case EquipmentSlot.Weapon:
                        weaponSlot.RemoveItemFromSlot();
                        break;
                    case EquipmentSlot.Necklace:
                        necklaceSlot.RemoveItemFromSlot();
                        break;
                    case EquipmentSlot.Ring:
                        ringSlot.RemoveItemFromSlot();
                        break;
                    case EquipmentSlot.Armor:
                        armorSlot.RemoveItemFromSlot();
                        break;
                    case EquipmentSlot.Cons1:
                        conSlot1.RemoveItemFromSlot();
                        break;
                    case EquipmentSlot.Cons2:
                        conSlot2.RemoveItemFromSlot();
                        break;
                    case EquipmentSlot.Cons3:
                        conSlot3.RemoveItemFromSlot();
                        break;
                    case EquipmentSlot.Cons4:
                        conSlot4.RemoveItemFromSlot();
                        break;
                }
            }

            _inventoryItems.Remove(item);
        }
    }
}