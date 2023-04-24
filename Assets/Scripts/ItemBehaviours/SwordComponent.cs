using Markers;
using Player;
using UnityEngine;

namespace ItemBehaviours
{
    public class SwordComponent : WeaponComponent
    {
        private PlayerStats _stats;

        public override void Init(PlayerStats stats)
        {
            _stats = stats;
        }
        
        private void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<HurtBox>())
            {
                col.GetComponent<HurtBox>().GetDamage(_stats.TotalAttackDamage);
            }
        }
    }
}