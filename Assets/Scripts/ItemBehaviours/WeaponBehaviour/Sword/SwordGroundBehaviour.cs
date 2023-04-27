using System;
using Cysharp.Threading.Tasks;
using Managers_Controllers;
using Markers;
using Player;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace ItemBehaviours.WeaponBehaviour.Sword
{
    public class SwordGroundBehaviour : WeaponBehaviour
    {
        [SerializeField] private float attackCooldownValue;
        [SerializeField] private float specialCooldownValue;
        [SerializeField] private float animTimeoutValue;
        [SerializeField] private float effectRadius;
        [SerializeField] private LayerMask effectLayer;
        
        private static readonly int GroundTrigger = Animator.StringToHash("Ground");

        private int _specialHit;
        private Collider[] _colliders = new Collider[30];
        
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
            
            await UniTask.Delay(TimeSpan.FromSeconds(SwordDamageDelay));

            SwordAttackHit = Physics.OverlapSphereNonAlloc(
                attackPoint, SwordAttackRadius, SwordAttackColliders, effectLayer);

            if (SwordAttackHit > 0)
            {
                for (int i = 0; i < SwordAttackHit; i++)
                {
                    if (SwordAttackColliders[i])
                    {
                        SwordAttackColliders[i].GetComponent<HurtBox>().GetDamage(Stats.TotalAttackDamage);
                        SwordAttackColliders[i] = null;
                    }
                }
            }
        }

        public override async void Special(Vector3 position)
        {
            Anim.SetTrigger(GroundTrigger);
            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout));
            
            _specialHit = Physics.OverlapSphereNonAlloc(position, effectRadius, _colliders, effectLayer);

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