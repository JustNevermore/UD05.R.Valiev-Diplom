using System;
using Cysharp.Threading.Tasks;
using Markers;
using UnityEngine;

namespace Weapons
{
    public class Sword : WeaponBehaviour
    {
        private BoxCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<HurtBox>())
            {
                col.GetComponent<HurtBox>().GetDamage(10);
            }
        }
    }
}