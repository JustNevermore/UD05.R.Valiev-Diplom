using System;
using Cysharp.Threading.Tasks;
using Managers_Controllers;
using Markers;
using Player;
using UnityEngine;
using Zenject;

namespace ItemBehaviours.WeaponBehaviour.Bow
{
    public class BowArrowsHailBehaviour : WeaponBehaviour
    {
        [SerializeField] private float attackCooldownValue;
        [SerializeField] private float specialCooldownValue;
        [SerializeField] private float animTimeoutValue;
        [SerializeField] private float effectRadius;
        [SerializeField] private LayerMask effectLayer;

        private static readonly int ArrowsHailTrigger = Animator.StringToHash("ArrowsHail");
        
        private int _specialHit;
        private Collider[] _colliders = new Collider[30];
        private readonly float _fallDelay = 1f;

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
            arrow.Init(dir, Stats.TotalAttackDamage);
            arrow.Launch();
        }

        public override async void Special()
        {
            Anim.SetTrigger(ArrowsHailTrigger);
            
            var attackPos = Controller.AttackPos.transform.position;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_fallDelay));
            
            _specialHit = Physics.OverlapSphereNonAlloc(attackPos, effectRadius, _colliders, effectLayer);

            if (_specialHit > 0)
            {
                for (int i = 0; i < _specialHit; i++)
                {
                    if (_colliders[i])
                    {
                        _colliders[i].GetComponent<HurtBox>().GetDamage(Stats.TotalSpecialDamage);
                    }
                }
            }
        }
    }
}