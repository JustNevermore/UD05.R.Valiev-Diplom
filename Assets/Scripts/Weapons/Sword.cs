using System;
using Cysharp.Threading.Tasks;
using Markers;
using UnityEngine;

namespace Weapons
{
    public class Sword : Weapon
    {
        private BoxCollider _collider;
        
        private void Awake()
        {
            SetAnimSelector();
            _collider = GetComponent<BoxCollider>();
            _collider.enabled = false;
        }

        protected override void SetAnimSelector()
        {
            _currentAnim = SwordAnim;
        }

        public override async void DoAttack()
        {
            _collider.enabled = true;
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            _collider.enabled = false;
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<HurtBox>())
            {
                col.GetComponent<HurtBox>().GetDamage(attackDamage);
            }
        }
    }
}