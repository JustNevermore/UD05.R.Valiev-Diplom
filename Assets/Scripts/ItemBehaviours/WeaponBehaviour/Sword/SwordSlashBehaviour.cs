using System;
using Cysharp.Threading.Tasks;
using Managers_Controllers;
using Markers;
using Player;
using UnityEngine;

namespace ItemBehaviours.WeaponBehaviour.Sword
{
    public class SwordSlashBehaviour : WeaponBehaviour
    {
        [SerializeField] private float attackCooldownValue;
        [SerializeField] private float specialCooldownValue;
        [SerializeField] private float animTimeoutValue;
        [SerializeField] private LayerMask effectLayer;
        [SerializeField] private int playerLayer;
        [SerializeField] private int enemyLayer;

        private static readonly int SlashTrigger = Animator.StringToHash("Slash");

        private int _raycastHits;
        private RaycastHit[] _hitObjects = new RaycastHit[30];
        
        private readonly float _jumpPower = 110f;
        private readonly float _slashTime = 0.075f;
        
        private HurtBox _hurtBox;

        public override void Init(PlayerStats stats, Animator animator, Rigidbody rigidbody, PoolManager poolManager)
        {
            base.Init(stats, animator, rigidbody, poolManager);
            attackCooldown = attackCooldownValue;
            specialCooldown = specialCooldownValue;
            animTimeout = animTimeoutValue;
            _hurtBox = Rb.GetComponent<HurtBox>();
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
            Anim.SetTrigger(SlashTrigger);
            
            var posStart = new Vector3(Rb.transform.position.x, 
                Rb.transform.position.y + 1, Rb.transform.position.z);
            var dir = (position - posStart).normalized;

            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout - _slashTime));

            Physics.IgnoreLayerCollision(playerLayer, enemyLayer, true);
            _hurtBox.EnableBlock();
            Rb.AddForce(dir * _jumpPower, ForceMode.Impulse);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_slashTime));

            Rb.isKinematic = true;
            Rb.isKinematic = false;

            Physics.IgnoreLayerCollision(playerLayer, enemyLayer, false);
            _hurtBox.DisableBlock();
            
            var posFinish = new Vector3(Rb.transform.position.x, 
                Rb.transform.position.y + 1, Rb.transform.position.z);
            var distance = (posStart - posFinish).magnitude;
            
            _raycastHits = Physics.SphereCastNonAlloc(posFinish, SwordAttackRadius, -dir, _hitObjects, distance, effectLayer);

            if (_raycastHits > 0)
            {
                for (int i = 0; i < _raycastHits; i++)
                {
                    _hitObjects[i].collider.GetComponent<HurtBox>().GetDamage(Stats.TotalSpecialDamage);
                }
            }
        }
    }
}