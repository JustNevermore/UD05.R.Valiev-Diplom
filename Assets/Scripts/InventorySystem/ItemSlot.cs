using Player;
using Ui.InventorySecondaryUi;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace InventorySystem
{
    public class ItemSlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] protected ItemType itemType;
        
        protected InventoryWindow _inventoryWindow;
        protected InventoryController _inventoryController;
        protected PlayerStats _playerStats;

        [Inject]
        private void Construct(InventoryWindow inventoryWindow, InventoryController inventoryController, PlayerStats playerStats)
        {
            _inventoryWindow = inventoryWindow;
            _inventoryController = inventoryController;
            _playerStats = playerStats;
        }


        public virtual void OnDrop(PointerEventData eventData)
        {
            
        }
        
        protected virtual void EquipInSlot(GameObject item)
        {
            item.transform.SetParent(transform);
            item.GetComponent<Transform>().position =
                GetComponent<Transform>().position;
        }
        
        protected void BuyItem(GameObject item)
        {
            if (item.GetComponentInParent<MerchantWindow>())
            {
                var itemCost = item.GetComponent<Item>().ItemStats.ItemCost;
                if (itemCost < _playerStats.CurrentGold)
                {
                    item.transform.SetParent(transform);
                    _playerStats.DecreaseGold(itemCost);
                }
                else
                {
                    item.transform.position = item.GetComponent<DragDrop>().DragAnchor;
                }
            }
        }
    }
}