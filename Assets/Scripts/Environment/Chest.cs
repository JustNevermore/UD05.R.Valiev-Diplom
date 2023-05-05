using System;
using System.Collections.Generic;
using InventorySystem;
using Managers_Controllers;
using Player;
using Signals;
using UnityEngine;
using Zenject;

namespace Environment
{
    public class Chest : MonoBehaviour
    {
        private SignalBus _signalBus;
        private LootDropManager _lootManager;
        private ChestWindow _chestWindow;
        
        private List<ItemData> _itemsInChest;

        private int _spawnItemsCount = 3;

        private bool _isActive;
        private bool _firstOpen;


        [Inject]
        private void Construct(SignalBus signalBus, LootDropManager lootDropManager, ChestWindow chestWindow)
        {
            _signalBus = signalBus;
            _lootManager = lootDropManager;
            _chestWindow = chestWindow;

            _spawnItemsCount = _lootManager.ChestSpawnItemsCount;
        }

        private void Start()
        {
            _isActive = false;
            _firstOpen = true;
            _itemsInChest = new List<ItemData>();
        }

        public void ActivateChest()
        {
            //todo добавить эффект вокруг сундука

            _isActive = true;
        }

        private void OnTriggerEnter(Collider col)
        {
            if (!_isActive)
                return;
            
            if (col.GetComponent<PlayerController>())
            {
                if (_firstOpen)
                {
                    GenerateLoot();
                    _firstOpen = false;
                }
                
                _chestWindow.OpenChestWindow(this, _itemsInChest);
                _signalBus.Fire<OpenChestSignal>();
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (!_isActive)
                return;
            
            if (col.GetComponent<PlayerController>())
            {
                _signalBus.Fire<CloseChestSignal>();
            }
        }

        private void GenerateLoot()
        {
            for (int i = 0; i < _spawnItemsCount; i++)
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