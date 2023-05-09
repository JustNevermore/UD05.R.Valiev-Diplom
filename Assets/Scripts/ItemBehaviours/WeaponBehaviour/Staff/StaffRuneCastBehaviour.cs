using System;
using Cysharp.Threading.Tasks;
using Enemies;
using Managers_Controllers;
using Markers;
using Player;
using UnityEngine;

namespace ItemBehaviours.WeaponBehaviour.Staff
{
    public class StaffRuneCastBehaviour : WeaponBehaviour
    {
        [SerializeField] private float attackCooldownValue;
        [SerializeField] private float specialCooldownValue;
        [SerializeField] private float animTimeoutValue;

        public override void Init(PlayerController controller, PlayerStats stats, Animator animator, PoolManager poolManager, FxPoolManager fxPoolManager)
        {
            base.Init(controller, stats, animator, poolManager, fxPoolManager);
            attackCooldown = attackCooldownValue;
            specialCooldown = specialCooldownValue;
            animTimeout = animTimeoutValue;
        }
        
        public override async void Attack()
        {
            Anim.SetTrigger(AttackTrigger);

            await UniTask.Delay(TimeSpan.FromSeconds(CastDelay));

            var attackPos = Controller.AttackPos.transform.position;
            var zeroPos = Controller.ZeroPos.transform.position;
            var dir = (attackPos - zeroPos).normalized;
            
            var spell = Pool.GetSpell();
            spell.transform.position = attackPos;
            spell.Init(dir);
            spell.Launch();
        }

        public override async void Special()
        {
            
            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout));

            var rune = Pool.GetRune();
            rune.transform.position = Controller.transform.position;
            rune.Init();
        }
    }
}