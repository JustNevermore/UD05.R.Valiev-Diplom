using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace InventorySystem
{
    public class InventoryWindow : MonoBehaviour, IDropHandler
    {
        private DiContainer _diContainer;
        private InventoryController _inventoryController;
        private AllItemsContainer _allItemsContainer;
        private PlayerStats _playerStats;

        [Inject]
        private void Construct(DiContainer diContainer, InventoryController inventoryController, AllItemsContainer allItemsContainer, PlayerStats playerStats)
        {
            _diContainer = diContainer;
            _inventoryController = inventoryController;
            _allItemsContainer = allItemsContainer;
            _playerStats = playerStats;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var obj = eventData.pointerDrag;
            if (obj == null)
                return;

            var anchor = obj.GetComponent<DragDrop>().DragAnchor;
            var item = obj.GetComponent<Item>().Data;

            // покупаем
            if (!item.BelongToPlayer)
            {
                if (item.IsStackable) // предмет стакается
                {
                    var flag = false;
                    ItemData stackItem = null;
                    foreach (var invItem in _inventoryController.InventoryItems) // ищем стакаемый предмет
                    {
                        if (invItem.ItemId == item.ItemId)
                        {
                            flag = true;
                            stackItem = invItem;
                        }
                    }

                    if (_inventoryController.Weapon != null)
                    {
                        if (_inventoryController.Weapon.ItemId == item.ItemId)
                        {
                            flag = true;
                            stackItem = _inventoryController.Weapon;
                        }
                    }

                    if (_inventoryController.Necklace != null)
                    {
                        if (_inventoryController.Necklace.ItemId == item.ItemId)
                        {
                            flag = true;
                            stackItem = _inventoryController.Necklace;
                        }
                    }

                    if (_inventoryController.Ring != null)
                    {
                        if (_inventoryController.Ring.ItemId == item.ItemId)
                        {
                            flag = true;
                            stackItem = _inventoryController.Ring;
                        }
                    }

                    if (_inventoryController.Armor != null)
                    {
                        if (_inventoryController.Armor.ItemId == item.ItemId)
                        {
                            flag = true;
                            stackItem = _inventoryController.Armor;
                        }
                    }

                    if (_inventoryController.Cons1 != null)
                    {
                        if (_inventoryController.Cons1.ItemId == item.ItemId)
                        {
                            flag = true;
                            stackItem = _inventoryController.Cons1;
                        }
                    }

                    if (_inventoryController.Cons2 != null)
                    {
                        if (_inventoryController.Cons2.ItemId == item.ItemId)
                        {
                            flag = true;
                            stackItem = _inventoryController.Cons2;
                        }
                    }

                    if (_inventoryController.Cons3 != null)
                    {
                        if (_inventoryController.Cons3.ItemId == item.ItemId)
                        {
                            flag = true;
                            stackItem = _inventoryController.Cons3;
                        }
                    }

                    if (_inventoryController.Cons4 != null)
                    {
                        if (_inventoryController.Cons4.ItemId == item.ItemId)
                        {
                            flag = true;
                            stackItem = _inventoryController.Cons4;
                        }
                    }

                    if (flag)
                    {
                        _playerStats.DecreaseGold(_allItemsContainer.GetConfigById(item.ItemId).ItemCost * item.ItemAmount);
                        stackItem.ItemAmount += item.ItemAmount;
                        obj.transform.position = anchor;
                    }
                    else
                    {
                        _playerStats.DecreaseGold(_allItemsContainer.GetConfigById(item.ItemId).ItemCost * item.ItemAmount);
                        var itemDuplicate = new ItemData();
                        itemDuplicate.CopyData(obj.GetComponent<Item>().Data);
                        itemDuplicate.BelongToPlayer = true;
                        _inventoryController.AddToInventory(itemDuplicate);
                        var newItem = _diContainer.InstantiateComponent<Item>(new GameObject("Item"));
                        newItem.gameObject.transform.SetParent(transform);
                        newItem.Init(itemDuplicate);
                        obj.transform.position = anchor;
                    }
                }
                else // предмет не стакается
                {
                    _playerStats.DecreaseGold(_allItemsContainer.GetConfigById(item.ItemId).ItemCost * item.ItemAmount);
                    var itemDuplicate = new ItemData();
                    itemDuplicate.CopyData(obj.GetComponent<Item>().Data);
                    itemDuplicate.BelongToPlayer = true;
                    _inventoryController.AddToInventory(itemDuplicate);
                    var newItem = _diContainer.InstantiateComponent<Item>(new GameObject("Item"));
                    newItem.gameObject.transform.SetParent(transform);
                    newItem.Init(itemDuplicate);
                    obj.transform.position = anchor;
                }
            }
            else // перемещаем из слота
            {
                if (!obj.transform.IsChildOf(transform))
                {
                    if (item.Type == ItemType.Consumable) // проверка на понижение статов для экипировки
                    {
                        _inventoryController.RemoveFromSlot(obj.GetComponentInParent<InventorySlot>().SlotType);
                        _inventoryController.AddToInventory(item);
                        obj.transform.SetParent(transform);
                    }
                    else
                    {
                        _inventoryController.RemoveFromSlot(obj.GetComponentInParent<InventorySlot>().SlotType);
                        _inventoryController.AddToInventory(item);
                        obj.transform.SetParent(transform);
                        _playerStats.DecreaseStats(_allItemsContainer.GetConfigById(item.ItemId));
                    }
                }
                else
                {
                    obj.transform.position = anchor;
                }
            }
        }
    }
}