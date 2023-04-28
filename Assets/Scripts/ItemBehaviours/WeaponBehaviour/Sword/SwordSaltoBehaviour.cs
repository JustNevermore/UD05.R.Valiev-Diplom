using System;
using Cysharp.Threading.Tasks;
using Enemies;
using Managers_Controllers;
using Markers;
using Player;
using UnityEngine;

namespace ItemBehaviours.WeaponBehaviour.Sword
{
    public class SwordSaltoBehaviour : WeaponBehaviour
    {
        [SerializeField] private float attackCooldownValue;
        [SerializeField] private float specialCooldownValue;
        [SerializeField] private float animTimeoutValue;
        [SerializeField] private float effectRadius;
        [SerializeField] private LayerMask effectLayer;
        [SerializeField] private float pushForce;

        private static readonly int SaltoTrigger = Animator.StringToHash("Salto");
        
        private int _specialHit;
        private Collider[] _colliders = new Collider[30];
        private float _pushTime = 0.1f;

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
            
            await UniTask.Delay(TimeSpan.FromSeconds(SwordDamageDelay));
            
            var attackPoint = Controller.AttackPos.transform.position;

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

        public override async void Special()
        {
            Anim.SetTrigger(SaltoTrigger);
            
            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout));
            
            var attackPos = Controller.AttackPos.transform.position;
            
            _specialHit = Physics.OverlapSphereNonAlloc(attackPos, effectRadius, _colliders, effectLayer);

            if (_specialHit > 0)
            {
                for (int i = 0; i < _specialHit; i++)
                {
                    _colliders[i].GetComponent<HurtBox>().GetDamage(Stats.TotalSpecialDamage);
                    if (_colliders[i].GetComponent<EnemyBase>())
                    {
                        var rb = _colliders[i].GetComponent<Rigidbody>();
                        rb.AddExplosionForce(pushForce, Rb.transform.position, effectRadius);
                    }
                }
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(_pushTime));

            if (_specialHit > 0)
            {
                for (int i = 0; i < _specialHit; i++)
                {
                    if (_colliders[i].GetComponent<EnemyBase>())
                    {
                        var rb = _colliders[i].GetComponent<Rigidbody>();
                        rb.isKinematic = true; // гасим инерцию
                        rb.isKinematic = false;
                    }
                }
            }
        }
    }
}