using System;
using System.Collections.Generic;
using InventorySystem;
using Managers_Controllers;
using Player;
using UnityEngine;
using Zenject;

namespace Environment.Chests
{
    public class Chest : MonoBehaviour
    {
        private LootDropManager _lootManager;
        private ChestWindow _chestWindow;
        
        private bool _firstOpen;
        private List<ItemData> _itemsInChest;
        

        [Inject]
        private void Construct(LootDropManager lootDropManager, ChestWindow chestWindow)
        {
            _lootManager = lootDropManager;
            _chestWindow = chestWindow;
        }

        private void OnEnable()
        {
            _firstOpen = true;
            _itemsInChest = new List<ItemData>();
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<PlayerController>())
            {
                if (_firstOpen)
                {
                    GenerateLoot();
                    _firstOpen = false;
                    _chestWindow.OpenChestWindow(this, _itemsInChest);
                }
                else
                {
                    _chestWindow.OpenChestWindow(this, _itemsInChest);
                }
            }
        }

        private void OnTriggerExit(Collider col)
        {
            _chestWindow.CloseChestWindow();
        }

        private void GenerateLoot()
        {
            for (int i = 0; i < 5; i++)
            {
                _itemsInChest.Add(_lootManager.GetRandomItem());
            }
        }

        public void UpdateChestLoot(List<ItemData> items)
        {
            _itemsInChest = items;
        }
    }
}