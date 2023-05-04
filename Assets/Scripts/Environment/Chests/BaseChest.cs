using System;
using System.Collections.Generic;
using InventorySystem;
using Managers_Controllers;
using Player;
using UnityEngine;
using Zenject;

namespace Environment.Chests
{
    public class BaseChest : MonoBehaviour
    {
        private DiContainer _diContainer;
        private LootDropManager _lootManager;
        
        private bool _firstOpen;
        private List<ItemData> _itemsInChest;

        [Inject]
        private void Construct(DiContainer diContainer, LootDropManager lootDropManager)
        {
            _diContainer = diContainer;
            _lootManager = lootDropManager;
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
                    // подсоединить к окну сундука и отобразить предметы
                }
                else
                {
                    // подсоединить к окну сундука и отобразить предметы
                }
            }
        }

        private void GenerateLoot()
        {
            for (int i = 0; i < 3; i++)
            {
                _itemsInChest. Add(_lootManager.GetRandomItem());
            }
        }
    }
}