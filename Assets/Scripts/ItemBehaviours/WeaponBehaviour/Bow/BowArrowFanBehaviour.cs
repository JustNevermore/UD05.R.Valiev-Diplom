using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Managers_Controllers;
using Player;
using UnityEngine;

namespace ItemBehaviours.WeaponBehaviour.Bow
{
    public class BowArrowFanBehaviour : WeaponBehaviour
    {
        [SerializeField] private float attackCooldownValue;
        [SerializeField] private float specialCooldownValue;
        [SerializeField] private float animTimeoutValue;

        private List<Vector3> _directions = new List<Vector3>(7);
        private float _fanShootDelay = 0.0001f;

        public override void Init(PlayerController controller, PlayerStats stats, Animator animator, PoolManager poolManager)
        {
            base.Init(controller, stats, animator, poolManager);
            attackCooldown = attackCooldownValue;
            specialCooldown = specialCooldownValue;
            animTimeout = animTimeoutValue;
        }
        
        public override async void Attack()
        {
            Anim.SetTrigger(AttackTrigger);

            await UniTask.Delay(TimeSpan.FromSeconds(ShootDelay));

            var attackPos = Controller.AttackPos.transform.position;
            var zeroPos = Controller.ZeroPos.transform.position;
            var dir = (attackPos - zeroPos).normalized;
            
            var arrow = Pool.GetArrow();
            arrow.transform.position = attackPos;
            arrow.Init(dir);
            arrow.Launch();
        }

        public override async void Special()
        {
            
            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout));

            var vector0 = (Controller.AttackPos.transform.position - Controller.ZeroPos.transform.position).normalized;
            var right90 = (Controller.RightPos.transform.position - Controller.ZeroPos.transform.position).normalized;
            var right45 = (vector0 + right90).normalized;
            var right23 = (vector0 + right45).normalized;
            var right12 = (vector0 + right23).normalized;
            var right34 = (right23 + right45).normalized;

            var left90 = -right90;
            var left45 = (vector0 + left90).normalized;
            var left23 = (vector0 + left45).normalized;
            var left12 = (vector0 + left23).normalized;
            var left34 = (left23 + left45).normalized;
            
            _directions.Add(left34);
            _directions.Add(left23);
            _directions.Add(left12);
            _directions.Add(vector0);
            _directions.Add(right12);
            _directions.Add(right23);
            _directions.Add(right34);
            
            var attackPos = Controller.AttackPos.transform.position;

            foreach (var direction in _directions)
            {
                var arrow = Pool.GetArrow();
                arrow.transform.position = attackPos;
                arrow.Init(direction);
                arrow.Launch();

                await UniTask.Delay(TimeSpan.FromSeconds(_fanShootDelay));
            }
            
            _directions.Clear();
        }
    }
}