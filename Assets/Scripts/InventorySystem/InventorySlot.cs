using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace InventorySystem
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] private EquipmentSlot slotType;
        [SerializeField] private ItemType itemType;
        
        private InventoryController _inventoryController;
        private AllItemsContainer _allItemsContainer;
        private InventoryWindow _inventoryWindow;
        private PlayerStats _playerStats;

        public EquipmentSlot SlotType => slotType;

        [Inject]
        private void Construct(InventoryController inventoryController, AllItemsContainer allItemsContainer, InventoryWindow inventoryWindow, PlayerStats playerStats)
        {
            _inventoryController = inventoryController;
            _allItemsContainer = allItemsContainer;
            _inventoryWindow = inventoryWindow;
            _playerStats = playerStats;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var obj = eventData.pointerDrag;
            if (obj == null)
                return;

            var anchor = obj.GetComponent<DragDrop>().DragAnchor;
            var item = obj.GetComponent<Item>().Data;
            
            if (item.BelongToPlayer) // принадлежит нам
            {
                if (item.Type == itemType) // подходит ли тип предмета для ячейки
                {
                    if (transform.childCount == 0) // есть ли в текущий момент предмет в ячейке
                    {
                        if (item.Type == ItemType.Consumable)
                        {
                            _inventoryController.RemoveFromInventory(item);
                            _inventoryController.AddToSlot(slotType, item);
                            obj.transform.SetParent(transform);
                        }
                        else
                        {
                            _inventoryController.RemoveFromInventory(item);
                            _inventoryController.AddToSlot(slotType, item);
                            obj.transform.SetParent(transform);
                            _playerStats.IncreaseStats(_allItemsContainer.GetConfigById(item.ItemId));
                        }
                    }
                    else
                    {
                        if (item.Type == ItemType.Consumable)
                        {
                            _inventoryController.RemoveFromSlot(slotType);
                            _inventoryController.AddToInventory(transform.GetComponentInChildren<Item>().Data);
                            transform.GetComponentInChildren<Item>().transform
                                .SetParent(_inventoryWindow.transform);
                            
                            _inventoryController.RemoveFromInventory(item);
                            _inventoryController.AddToSlot(slotType, item);
                            obj.transform.SetParent(transform);
                        }
                        else
                        {
                            _playerStats.DecreaseStats(_allItemsContainer.GetConfigById(transform.
                                GetComponentInChildren<Item>().Data.ItemId));
                            _inventoryController.RemoveFromSlot(slotType);
                            _inventoryController.AddToInventory(transform.GetComponentInChildren<Item>().Data);
                            transform.GetComponentInChildren<Item>().transform.SetParent(_inventoryWindow.transform);
                            
                            _inventoryController.RemoveFromInventory(item);
                            _inventoryController.AddToSlot(slotType, item);
                            obj.transform.SetParent(transform);
                            _playerStats.IncreaseStats(_allItemsContainer.GetConfigById(item.ItemId));
                        }
                    }
                }
                else
                {
                    obj.transform.position = anchor;
                }
            }
            else // перетаскивание из магазина сразу в ячейку блокировано
            {
                obj.transform.position = anchor;
            }
        }
    }
}