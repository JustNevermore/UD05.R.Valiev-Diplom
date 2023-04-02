using System;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "RingConfig", menuName = "ItemConfigs/Ring", order = 0)]
    public class RingItem : ItemConfig
    {
        [SerializeField] private float damage;
    }
}