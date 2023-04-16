using Player;
using Ui.InventorySecondaryUi;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace InventorySystem
{
    public class InventoryWindow : MonoBehaviour, IDropHandler
    {
        private PlayerStats _playerStats;

        [Inject]
        private void Construct(PlayerStats playerStats)
        {
            _playerStats = playerStats;
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            var item = eventData.pointerDrag;

            if (item == null)
            {
                return;
            }

            var dragAnchor = item.GetComponent<DragDrop>().DragAnchor;

            if (item.transform.IsChildOf(transform))
            {
                item.transform.position = dragAnchor;
            }
            else
            {
                if (item.GetComponentInParent<EquipmentSlot>())
                {
                    _playerStats.DecreaseStats(item.GetComponent<Item>().ItemStats);
                    item.transform.SetParent(transform);
                }
                else if (item.GetComponentInParent<MerchantWindow>())
                {
                    var itemCost = item.GetComponent<Item>().ItemStats.ItemCost;
                    if (itemCost < _playerStats.CurrentGold)
                    {
                        item.transform.SetParent(transform);
                        _playerStats.DecreaseGold(itemCost);
                    }
                    else
                    {
                        item.transform.position = dragAnchor;
                    }
                }
                else
                {
                    item.transform.SetParent(transform);
                }
            }
        }
    }
}