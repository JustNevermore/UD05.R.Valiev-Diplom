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
        private PlayerStats _playerStats;

        private List<ItemData> _merchantItems;

        [Inject]
        private void Construct(DiContainer diContainer, AllItemsContainer allItemsContainer, PlayerStats playerStats)
        {
            _diContainer = diContainer;
            _allItemsContainer = allItemsContainer;
            _playerStats = playerStats;

            _merchantItems = new List<ItemData>();
        }
        
        private void Start()
        {
            foreach (var item in _allItemsContainer.AllItemsList)
            {
                var itemData = new ItemData(item.ItemId, _allItemsContainer);
                _merchantItems.Add(itemData);
            }
            
            FillMerchant();
        }

        private void FillMerchant()
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
            
            var item = obj.GetComponent<Item>().Data;
            
            obj.GetComponent<DragDrop>().SetDropFlag();
            
            if (!obj.GetComponentInParent<InventorySlot>()) // экипированные в слоты предметы продавать нельзя
            {
                if (item.BelongToPlayer)
                {
                    _playerStats.IncreaseGold( Convert.ToInt32(
                        _allItemsContainer.GetConfigById(item.ItemId).ItemCost * sellMultiplier * item.ItemAmount));
                    Destroy(obj.gameObject);
                }
                else
                {
                    obj.transform.position = obj.GetComponent<DragDrop>().DragAnchor;
                }
            }
            else
            {
                obj.transform.position = obj.GetComponent<DragDrop>().DragAnchor;
            }
        }
    }
}