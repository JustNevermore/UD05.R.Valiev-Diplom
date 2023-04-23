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
        [SerializeField] private float sellMultiplier;

        private DiContainer _diContainer;
        private AllItemsContainer _allItemsContainer;
        private InventoryController _inventoryController;
        private PlayerStats _playerStats;

        [SerializeField] private List<ItemData> _merchantItems = new List<ItemData>();

        [Inject]
        private void Construct(DiContainer diContainer, AllItemsContainer allItemsContainer, InventoryController inventoryController, PlayerStats playerStats)
        {
            _diContainer = diContainer;
            _allItemsContainer = allItemsContainer;
            _inventoryController = inventoryController;
            _playerStats = playerStats;
        }
        
        private void Awake()
        {
            foreach (var item in _allItemsContainer.AllItemsList)
            {
                var itemStatus = new ItemData(item.ItemId, _allItemsContainer);
                _merchantItems.Add(itemStatus);
            }
            
            RedrawInventory();
        }

        private void RedrawInventory()
        {
            foreach (var obj in GetComponentsInChildren<Item>())
            {
                Destroy(obj.gameObject);
            }
            
            foreach (var item in _merchantItems)
            {
                var newItem = _diContainer.InstantiateComponent<Item>(new GameObject("Item"));
                newItem.transform.SetParent(transform);
                newItem.Init(item);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            var obj = eventData.pointerDrag;
            if (obj == null)
                return;
            
            var itemStatus = obj.GetComponent<Item>().Data;
            
            if (itemStatus.BelongToPlayer)
            {
                _playerStats.IncreaseGold( Convert.ToInt32(
                    _allItemsContainer.GetConfigById(itemStatus.ItemId).ItemCost * sellMultiplier));
                Destroy(obj.gameObject);
            }
            else
            {
                obj.transform.position = obj.GetComponent<DragDrop>().DragAnchor;
            }
        }
    }
}