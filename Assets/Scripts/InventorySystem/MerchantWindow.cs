using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace InventorySystem
{
    public class MerchantWindow : MonoBehaviour, IDropHandler
    {
        private DiContainer _diContainer;
        private AllItemsContainer _allItemsContainer;
        private PlayerStats _playerStats;
        private InventoryWindow _inventoryWindow;
        
        [SerializeField] private float sellMultiplier;
        [SerializeField] private GameObject contentWindow;

        private List<ItemData> _merchantItems;
        

        [Inject]
        private void Construct(DiContainer diContainer, AllItemsContainer allItemsContainer, PlayerStats playerStats, InventoryWindow inventoryWindow)
        {
            _diContainer = diContainer;
            _allItemsContainer = allItemsContainer;
            _playerStats = playerStats;
            _inventoryWindow = inventoryWindow;
        }
        
        private void Start()
        {
            _merchantItems = new List<ItemData>();
            
            foreach (var item in _allItemsContainer.AllItemsList)
            {
                var itemData = new ItemData(item.ItemId, _allItemsContainer);
                itemData.BelongToPlayer = false;
                _merchantItems.Add(itemData);
            }
            
            RedrawMerchant();
        }

        private void RedrawMerchant()
        {
            foreach (var item in contentWindow.GetComponentsInChildren<Item>())
            {
                Destroy(item.gameObject);
            }
            
            foreach (var item in _merchantItems)
            {
                var newItem = _diContainer.InstantiateComponent<Item>(new GameObject("Item"));
                newItem.transform.SetParent(contentWindow.transform);
                newItem.Init(item);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            var obj = eventData.pointerDrag;
            if (obj == null)
                return;

            if (obj.GetComponent<Item>())
            {
                var item = obj.GetComponent<Item>().Data;

                var dragdrop = obj.GetComponent<DragDrop>();
                
                dragdrop.SetDropFlag();


                if (item.BelongToPlayer)
                {
                    if (item.InSlot == EquipmentSlot.None || item.Type == ItemType.Consumable)
                    {
                        _playerStats.IncreaseGold(Convert.ToInt32(_allItemsContainer
                            .GetConfigById(item.ItemId).ItemCost * sellMultiplier * item.ItemAmount));
                        _inventoryWindow.DeleteItemFromInventory(item);
                        Destroy(obj.gameObject);
                        
                        // todo звук покупки
                    }
                    else
                    {
                        _playerStats.DecreaseStats(_allItemsContainer.GetConfigById(item.ItemId));
                        
                        _playerStats.IncreaseGold(Convert.ToInt32(_allItemsContainer
                            .GetConfigById(item.ItemId).ItemCost * sellMultiplier * item.ItemAmount));
                        _inventoryWindow.DeleteItemFromInventory(item);
                        Destroy(obj.gameObject);
                        
                        // todo звук покупки
                    }
                }
                else
                {
                    obj.transform.position = dragdrop.DragAnchor;
                }
            }
        }
    }
}