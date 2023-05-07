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
    public abstract class Chest : MonoBehaviour
    {
        protected SignalBus SignalBus;
        protected AllItemsContainer Container;
        protected LootDropManager LootManager;
        protected ChestWindow ChestWindow;
        
        protected List<ItemData> ItemsInChest;
        
        
        [Inject]
        private void Construct(SignalBus signalBus, AllItemsContainer allItemsContainer, LootDropManager lootDropManager, ChestWindow chestWindow)
        {
            SignalBus = signalBus;
            Container = allItemsContainer;
            LootManager = lootDropManager;
            ChestWindow = chestWindow;
        }
        
        public virtual void ActivateChest()
        {
        }

        public abstract void UpdateChestLoot(List<ItemData> items);
    }
}