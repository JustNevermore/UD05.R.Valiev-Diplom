using System.Collections.Generic;
using InventorySystem;
using Managers_Controllers;
using Player;
using Signals;
using UnityEngine;
using Zenject;

namespace Environment
{
    public class DungeonChest : Chest
    {
        private int _spawnItemsCount = 3;

        private bool _isActive;
        
        private void Start()
        {
            _spawnItemsCount = LootManager.ChestSpawnItemsCount;
            _isActive = false;
            ItemsInChest = new List<ItemData>();
        }

        public override void ActivateChest()
        {
            //todo добавить эффект вокруг сундука

            GenerateLoot();
            _isActive = true;
        }

        private void OnTriggerEnter(Collider col)
        {
            if (!_isActive)
                return;
            
            if (col.GetComponent<PlayerController>())
            {
                ChestWindow.OpenChestWindow(this, ItemsInChest);
                SignalBus.Fire<OpenChestSignal>();
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (!_isActive)
                return;
            
            if (col.GetComponent<PlayerController>())
            {
                SignalBus.Fire<CloseChestSignal>();
            }
        }

        private void GenerateLoot()
        {
            for (int i = 0; i < _spawnItemsCount; i++)
            {
                ItemsInChest.Add(LootManager.GetRandomItem());
            }
        }

        public override void UpdateChestLoot(List<ItemData> items)
        {
            ItemsInChest = items;
        }
    }
}