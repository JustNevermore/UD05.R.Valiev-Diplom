using System;
using InventorySystem;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Ui.InventorySecondaryUi
{
    public class MerchantWindow : MonoBehaviour, IDropHandler
    {
        [SerializeField] private float sellMultiplier;
        
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
                _playerStats.IncreaseGold(
                    Convert.ToInt32(item.GetComponent<Item>().ItemStats.ItemCost * sellMultiplier));
                Destroy(item);
            }
        }
    }
}