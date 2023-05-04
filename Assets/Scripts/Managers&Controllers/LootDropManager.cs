using System;
using InventorySystem;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Managers_Controllers
{
    public class LootDropManager : MonoBehaviour
    {
        private AllItemsContainer _allItemsContainer;

        private int _totalWeight;
        private int _artifactWeight;

        [Inject]
        private void Construct(AllItemsContainer allItemsContainer)
        {
            _allItemsContainer = allItemsContainer;
        }

        private void Start()
        {
            _totalWeight = 0;

            foreach (var item in _allItemsContainer.AllItemsList)
            {
                _totalWeight += item.Rarity;
            }

            _artifactWeight = 0;

            foreach (var item in _allItemsContainer.AllItemsList)
            {
                if (item.Type != ItemType.Consumable)
                {
                    _artifactWeight += item.Rarity;
                }
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
    }
}