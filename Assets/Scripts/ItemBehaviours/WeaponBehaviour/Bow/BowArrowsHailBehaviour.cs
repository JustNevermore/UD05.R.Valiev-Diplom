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

        public override async void Special(Vector3 position)
        {
            Anim.SetTrigger(ArrowsHailTrigger);
            
            var pos = position;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_fallDelay));
            
            _specialHit = Physics.OverlapSphereNonAlloc(pos, effectRadius, _colliders, effectLayer);

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