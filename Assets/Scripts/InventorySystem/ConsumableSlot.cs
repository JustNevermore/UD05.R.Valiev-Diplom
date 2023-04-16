using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace InventorySystem
{
    public class ConsumableSlot : ItemSlot
    {
        public override void OnDrop(PointerEventData eventData)
        {
            var item = eventData.pointerDrag;
            
            if (item == null)
            {
                return;
            }
            
            var dragAnchor = item.GetComponent<DragDrop>().DragAnchor;

            if (itemType == item.GetComponent<Item>().ItemStats.Type)
            {
                if (transform.childCount == 0)
                {
                    BuyItem(item);
                    EquipInSlot(item);
                }
                else if (item.transform.IsChildOf(transform))
                {
                    item.transform.position = dragAnchor;
                }
                else
                {
                    GetComponentInChildren<Item>().transform.SetParent(_inventoryWindow.transform);
                    BuyItem(item);
                    EquipInSlot(item);
                }
            }
            else
            {
                item.transform.position = dragAnchor;
            }
        }
    }
}