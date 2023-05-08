using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace InventorySystem
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        private AllItemsContainer _allItemsContainer;
        private InventoryWindow _inventoryWindow;
        private PlayerStats _playerStats;
        private ChestWindow _chestWindow;
        
        [SerializeField] private EquipmentSlot slotType;
        [SerializeField] private ItemType itemType;
        
        private ItemData _containItem;

        public EquipmentSlot SlotType => slotType;
        public ItemType Type => itemType;
        public ItemData ContainItem => _containItem;


        [Inject]
        private void Construct(AllItemsContainer allItemsContainer, InventoryWindow inventoryWindow, PlayerStats playerStats, ChestWindow chestWindow)
        {
            _allItemsContainer = allItemsContainer;
            _inventoryWindow = inventoryWindow;
            _playerStats = playerStats;
            _chestWindow = chestWindow;
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
            
                if (item.BelongToPlayer && !item.InStash) // принадлежит нам
                {
                    if (item.Type == itemType) // подходит ли тип предмета для ячейки
                    {
                        if (_containItem == null) // есть ли в текущий момент предмет в ячейке
                        {
                            if (item.Type == ItemType.Consumable)
                            {
                                item.InSlot = slotType;
                                _containItem = item;
                            }
                            else
                            {
                                item.InSlot = slotType;
                                _containItem = item;
                                _playerStats.IncreaseStats(itemConfig);
                            }
                        }
                        else
                        {
                            if (item.Type == ItemType.Consumable)
                            {
                                _containItem.InSlot = EquipmentSlot.None;
                                item.InSlot = slotType;
                                _containItem = item;
                            }
                            else
                            {
                                var config = _allItemsContainer.GetConfigById(_containItem.ItemId);
                                _playerStats.DecreaseStats(config);
                                _containItem.InSlot = EquipmentSlot.None;
                                item.InSlot = slotType;
                                _containItem = item;
                                _playerStats.IncreaseStats(itemConfig);
                            }
                        }
                        
                        _inventoryWindow.RedrawInventory();
                    }
                    else
                    {
                        obj.transform.position = anchor;
                    }
                }
                else if (item.InStash) // из сундука
                {
                    if (item.Type == itemType) // подходит ли тип предмета для ячейки
                    {
                        if (_containItem == null) // есть ли в текущий момент предмет в ячейке
                        {
                            if (item.IsStackable) // предмет стакается
                            {
                                var flag = false;
                            
                                foreach (var invItem in _inventoryWindow.InventoryItems) // ищем стакаемый предмет
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
                                        item.InStash = false;
                                        _containItem = _inventoryWindow.AddItemToInventory(item, slotType);
                                    }
                                    else
                                    {
                                        _chestWindow.RemoveFromChest(item);
                                        item.InStash = false;
                                        _containItem = _inventoryWindow.AddItemToInventory(item, slotType);
                                        _playerStats.IncreaseStats(itemConfig);
                                    }
                                }
                            }
                            else // предмет не стакается
                            {
                                if (item.Type == ItemType.Consumable)
                                {
                                    _chestWindow.RemoveFromChest(item);
                                    item.InStash = false;
                                    _containItem = _inventoryWindow.AddItemToInventory(item, slotType);
                                }
                                else
                                {
                                    _chestWindow.RemoveFromChest(item);
                                    item.InStash = false;
                                    _containItem = _inventoryWindow.AddItemToInventory(item, slotType);
                                    _playerStats.IncreaseStats(itemConfig);
                                }
                            }
                        }
                        else
                        {
                            if (item.IsStackable) // предмет стакается
                            {
                                var flag = false;
                            
                                foreach (var invItem in _inventoryWindow.InventoryItems) // ищем стакаемый предмет
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
                                        item.InStash = false;
                                        _containItem.InSlot = EquipmentSlot.None;
                                        _containItem = _inventoryWindow.AddItemToInventory(item, slotType);
                                    }
                                    else
                                    {
                                        var config = _allItemsContainer.GetConfigById(_containItem.ItemId);
                                        _playerStats.DecreaseStats(config);
                                        _chestWindow.RemoveFromChest(item);
                                        item.InStash = false;
                                        _containItem.InSlot = EquipmentSlot.None;
                                        _containItem = _inventoryWindow.AddItemToInventory(item, slotType);
                                        _playerStats.IncreaseStats(itemConfig);
                                    }
                                }
                            }
                            else // предмет не стакается
                            {
                                if (item.Type == ItemType.Consumable)
                                {
                                    _chestWindow.RemoveFromChest(item);
                                    item.InStash = false;
                                    _containItem.InSlot = EquipmentSlot.None;
                                    _containItem = _inventoryWindow.AddItemToInventory(item, slotType);
                                }
                                else
                                {
                                    var config = _allItemsContainer.GetConfigById(_containItem.ItemId);
                                    _playerStats.DecreaseStats(config);
                                    _chestWindow.RemoveFromChest(item);
                                    item.InStash = false;
                                    _containItem.InSlot = EquipmentSlot.None;
                                    _containItem = _inventoryWindow.AddItemToInventory(item, slotType);
                                    _playerStats.IncreaseStats(itemConfig);
                                }
                            }
                        }
                        
                        _inventoryWindow.RedrawInventory();
                    }
                    else
                    {
                        obj.transform.position = anchor;
                    }
                }
                else // покупка сразу в слот
                {
                    if (_playerStats.CurrentGold >= itemConfig.ItemCost) // хватает ли золота
                    {
                        if (item.Type == itemType) // подходит ли тип предмета для ячейки
                        {
                            if (_containItem == null) // есть ли в текущий момент предмет в ячейке
                            {
                                if (item.IsStackable) // предмет стакается
                                {
                                    var flag = false;

                                    foreach (var invItem in _inventoryWindow.InventoryItems) // ищем стакаемый предмет
                                    {
                                        if (item.ItemId == invItem.ItemId)
                                        {
                                            flag = true;
                                            invItem.ItemAmount += item.ItemAmount;
                                        }
                                    }

                                    if (!flag)
                                    {
                                        if (item.Type == ItemType.Consumable)
                                        {
                                            _containItem = _inventoryWindow.AddItemToInventory(item, slotType);
                                        }
                                        else
                                        {
                                            _containItem = _inventoryWindow.AddItemToInventory(item, slotType);
                                            _playerStats.IncreaseStats(itemConfig);
                                        }
                                    }

                                    obj.transform.position = anchor;
                                }
                                else // предмет не стакается
                                {
                                    if (item.Type == ItemType.Consumable)
                                    {
                                        _containItem = _inventoryWindow.AddItemToInventory(item, slotType);
                                    }
                                    else
                                    {
                                        _containItem = _inventoryWindow.AddItemToInventory(item, slotType);
                                        _playerStats.IncreaseStats(itemConfig);
                                    }

                                    obj.transform.position = anchor;
                                }
                            }
                            else
                            {
                                if (item.IsStackable) // предмет стакается
                                {
                                    var flag = false;

                                    foreach (var invItem in _inventoryWindow.InventoryItems) // ищем стакаемый предмет
                                    {
                                        if (item.ItemId == invItem.ItemId)
                                        {
                                            flag = true;
                                            invItem.ItemAmount += item.ItemAmount;
                                        }
                                    }

                                    if (!flag)
                                    {
                                        if (item.Type == ItemType.Consumable)
                                        {
                                            _containItem.InSlot = EquipmentSlot.None;
                                            _containItem = _inventoryWindow.AddItemToInventory(item, slotType);
                                        }
                                        else
                                        {
                                            var config = _allItemsContainer.GetConfigById(_containItem.ItemId);
                                            _playerStats.DecreaseStats(config);
                                            _containItem.InSlot = EquipmentSlot.None;
                                            _containItem = _inventoryWindow.AddItemToInventory(item, slotType);
                                            _playerStats.IncreaseStats(itemConfig);
                                        }
                                    }

                                    obj.transform.position = anchor;
                                }
                                else // предмет не стакается
                                {
                                    if (item.Type == ItemType.Consumable)
                                    {
                                        _containItem.InSlot = EquipmentSlot.None;
                                        _containItem = _inventoryWindow.AddItemToInventory(item, slotType);
                                    }
                                    else
                                    {
                                        var config = _allItemsContainer.GetConfigById(_containItem.ItemId);
                                        _playerStats.DecreaseStats(config);
                                        _containItem.InSlot = EquipmentSlot.None;
                                        _containItem = _inventoryWindow.AddItemToInventory(item, slotType);
                                        _playerStats.IncreaseStats(itemConfig);
                                    }

                                    obj.transform.position = anchor;
                                }
                            }

                            _inventoryWindow.RedrawInventory();
                            _playerStats.DecreaseGold(itemConfig.ItemCost * item.ItemAmount);
                        }
                        else
                        {
                            obj.transform.position = anchor;
                        }
                    }
                    else
                    {
                        obj.transform.position = anchor;
                    }
                }
            }
        }

        public void AddItemToSlot(ItemData item)
        {
            _containItem = item;
        }

        public void RemoveItemFromSlot()
        {
            _containItem = null;
        }
    }
}