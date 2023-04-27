using System;
using Cysharp.Threading.Tasks;
using Managers_Controllers;
using Markers;
using Player;
using UnityEngine;
using Zenject;

namespace ItemBehaviours.WeaponBehaviour.Bow
{
    public class BowArrowFanBehaviour : WeaponBehaviour
    {
        [SerializeField] private float attackCooldownValue;
        [SerializeField] private float specialCooldownValue;
        [SerializeField] private float animTimeoutValue;

        public override void Init(PlayerStats stats, Animator animator, Rigidbody rigidbody, PoolManager poolManager)
        {
            base.Init(stats, animator, rigidbody, poolManager);
            attackCooldown = attackCooldownValue;
            specialCooldown = specialCooldownValue;
            animTimeout = animTimeoutValue;
        }
        
        public override async void Attack(Vector3 attackPoint)
        {
            Anim.SetTrigger(AttackTrigger);

            await UniTask.Delay(TimeSpan.FromSeconds(ShootDelay));

            var pos = new Vector3(Rb.transform.position.x, Rb.transform.position.y + 1, Rb.transform.position.z);
            var dir = attackPoint - pos;
            
            var arrow = PoolManager.GetArrow();
            arrow.transform.position = attackPoint;
            arrow.Init(dir, ProjectileSpeed, Stats.TotalAttackDamage);
            arrow.Launch();
        }

        public override void Special(Vector3 position)
        {
            
        }
    }
}