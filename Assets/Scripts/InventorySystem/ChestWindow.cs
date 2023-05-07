using System;
using System.Collections.Generic;
using Environment;
using Player;
using Signals;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace InventorySystem
{
    public class ChestWindow : MonoBehaviour, IDropHandler
    {
        private DiContainer _diContainer;
        private SignalBus _signalBus;
        private AllItemsContainer _allItemsContainer;
        private PlayerStats _playerStats;
        private InventoryWindow _inventoryWindow;
        
        [SerializeField] private GameObject contentWindow;
        private List<ItemData> _currentChestItems;
        private Chest _currentChest;
        private bool _chestOpened;

        [Inject]
        private void Construct(DiContainer diContainer, SignalBus signalBus, AllItemsContainer allItemsContainer, PlayerStats playerStats, InventoryWindow inventoryWindow)
        {
            _diContainer = diContainer;
            _signalBus = signalBus;
            _allItemsContainer = allItemsContainer;
            _playerStats = playerStats;
            _inventoryWindow = inventoryWindow;
        }

        private void Awake()
        {
            _signalBus.Subscribe<CloseChestSignal>(CloseChestWindow);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<CloseChestSignal>(CloseChestWindow);
        }

        public void OpenChestWindow(Chest chest, List<ItemData> items)
        {
            _currentChest = chest;
            _currentChestItems = items;
            _chestOpened = true;
            RedrawWindow();
        }

        private void CloseChestWindow()
        {
            if (_chestOpened)
            {
                _currentChest.UpdateChestLoot(_currentChestItems);
                _chestOpened = false;
            }
        }

        public void RemoveFromChest(ItemData item)
        {
            _currentChestItems.Remove(item);
            
            RedrawWindow();
        }

        private void RedrawWindow()
        {
            foreach (var item in contentWindow.GetComponentsInChildren<Item>())
            {
                Destroy(item.gameObject);
            }
            
            foreach (var item in _currentChestItems)
            {
                var newItem = _diContainer.InstantiateComponent<Item>(new GameObject("Item"));
                newItem.transform.SetParent(contentWindow.transform);
                newItem.Init(item);
                newItem.Data.InStash = true;
                newItem.Data.BelongToPlayer = true;
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
                
                if (!item.InStash)
                {
                    if (item.InSlot == EquipmentSlot.None)
                    {
                        if (item.IsStackable) // предмет стакается
                        {
                            var flag = false;
                            
                            foreach (var chestItem in _currentChestItems) // ищем стакаемый предмет
                            {
                                if (item.ItemId == chestItem.ItemId)
                                {
                                    flag = true;
                                    chestItem.ItemAmount += item.ItemAmount;
                                    _inventoryWindow.DeleteItemFromInventory(item);
                                }
                            }

                            if (!flag)
                            {
                                _inventoryWindow.DeleteItemFromInventory(item);
                                
                                _currentChestItems.Add(item);
                                item.InStash = true;
                            }
                        }
                        else // предмет не стакается
                        {
                            _inventoryWindow.DeleteItemFromInventory(item);
                            
                            _currentChestItems.Add(item);
                            item.InSlot = EquipmentSlot.None;
                            item.InStash = true;
                        }
                    }
                    else
                    {
                        if (item.IsStackable) // предмет стакается
                        {
                            var flag = false;
                            
                            foreach (var chestItem in _currentChestItems) // ищем стакаемый предмет
                            {
                                if (item.ItemId == chestItem.ItemId)
                                {
                                    flag = true;
                                    chestItem.ItemAmount += item.ItemAmount;
                                    _inventoryWindow.DeleteItemFromInventory(item);

                                    RemoveFromInventorySlot(item);
                                }
                            }

                            if (!flag)
                            {
                                _inventoryWindow.DeleteItemFromInventory(item);
                                
                                RemoveFromInventorySlot(item);
                                
                                _currentChestItems.Add(item);
                                item.InSlot = EquipmentSlot.None;
                                item.InStash = true;
                            }
                        }
                        else // предмет не стакается
                        {
                            _playerStats.DecreaseStats(itemConfig);
                            _inventoryWindow.DeleteItemFromInventory(item);
                            
                            RemoveFromInventorySlot(item);
                            
                            _currentChestItems.Add(item);
                            item.InSlot = EquipmentSlot.None;
                            item.InStash = true;
                        }
                    }
                    
                    RedrawWindow();
                }
                else
                {
                    obj.transform.position = anchor;
                }
            }
        }

        private void RemoveFromInventorySlot(ItemData item)
        {
            switch (item.InSlot)
            {
                case EquipmentSlot.Weapon:
                    _inventoryWindow.WeaponSlot.RemoveItemFromSlot();
                    break;
                case EquipmentSlot.Necklace:
                    _inventoryWindow.NecklaceSlot.RemoveItemFromSlot();
                    break;
                case EquipmentSlot.Ring:
                    _inventoryWindow.RingSlot.RemoveItemFromSlot();
                    break;
                case EquipmentSlot.Armor:
                    _inventoryWindow.ArmorSlot.RemoveItemFromSlot();
                    break;
                case EquipmentSlot.Cons1:
                    _inventoryWindow.ConSlot1.RemoveItemFromSlot();
                    break;
                case EquipmentSlot.Cons2:
                    _inventoryWindow.ConSlot2.RemoveItemFromSlot();
                    break;
                case EquipmentSlot.Cons3:
                    _inventoryWindow.ConSlot3.RemoveItemFromSlot();
                    break;
                case EquipmentSlot.Cons4:
                    _inventoryWindow.ConSlot4.RemoveItemFromSlot();
                    break;
            }
        }
    }
}