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

        private readonly float _specialDelay = 1f;
        
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
            Anim.SetTrigger(GroundTrigger);

            var attackPos = Controller.AttackPos.transform.position;

            await UniTask.Delay(TimeSpan.FromSeconds(_specialDelay));

            _specialHit = Physics.OverlapSphereNonAlloc(attackPos, effectRadius, _colliders, effectLayer);

            if (_specialHit > 0)
            {
                for (int i = 0; i < _specialHit; i++)
                {
                    _colliders[i].GetComponent<HurtBox>().GetDamage(Stats.TotalSpecialDamage);
                }
            }
        }
    }
}