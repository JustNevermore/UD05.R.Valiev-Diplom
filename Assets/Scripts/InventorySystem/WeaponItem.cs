using System;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "ItemConfigs/Weapon", order = 0)]
    public class WeaponItem : ItemConfig
    {
        [SerializeField] private float damage;
        [SerializeField] private GameObject weaponView;
    }
}