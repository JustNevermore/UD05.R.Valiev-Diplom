using UnityEngine;

namespace InventorySystem
{
    public class StackableItemMarker : MonoBehaviour
    {
        private string _itemName;

        public string MarkerName => _itemName;

        private void Awake()
        {
            _itemName = GetComponent<Item>().ItemStats.ItemName;
        }
    }
}