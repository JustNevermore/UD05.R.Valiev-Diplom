using System;
using System.Security.Cryptography.X509Certificates;
using Enemies;
using InventorySystem;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Managers_Controllers
{
    public class LootDropManager : MonoBehaviour
    {
        private AllItemsContainer _allItemsContainer;
        private PoolManager _poolManager;

        [SerializeField] private int chestSpawnItemsCount;

        private int _totalWeight;
        
        private readonly int _pouchDropChance = 3;
        private readonly int _commonCoinMaxAmount = 20;
        private readonly int _bossCoinMinAmount = 30;
        private readonly int _bossCoinMaxAmount = 100;

        public int ChestSpawnItemsCount => chestSpawnItemsCount;

        
        [Inject]
        private void Construct(AllItemsContainer allItemsContainer, PoolManager poolManager)
        {
            _allItemsContainer = allItemsContainer;
            _poolManager = poolManager;
        }

        private void Start()
        {
            _totalWeight = 0;

            foreach (var item in _allItemsContainer.AllItemsList)
            {
                _totalWeight += item.Rarity;
            }
        }

        public ItemData GetRandomItem()
        {
            var current = 0;
            var rnd = Random.Range(0, _totalWeight);

            foreach (var item in _allItemsContainer.AllItemsList)
            {
                current += item.Rarity;

                if (rnd <= current)
                {
                    var itemData = new ItemData(item.ItemId, _allItemsContainer);
                    return itemData;
                }
            }

            return null;
        }

        public void DropReward(EnemyType type, Vector3 pos)
        {
            if (type == EnemyType.Common)
            {
                var rnd = Random.Range(0, _pouchDropChance);
                if (rnd == 0)
                {
                    var pouch = _poolManager.GetCoinPouch();
                    pouch.SetAmount(Random.Range(1, _commonCoinMaxAmount));
                    pouch.transform.position = pos;
                }
            }
            else
            {
                var pouch = _poolManager.GetCoinPouch();
                pouch.SetAmount(Random.Range(_bossCoinMinAmount, _bossCoinMaxAmount));
                pouch.transform.position = pos;

                var shard = _poolManager.GetReviveShard();
                shard.transform.position = pos;
            }
        }
    }
}