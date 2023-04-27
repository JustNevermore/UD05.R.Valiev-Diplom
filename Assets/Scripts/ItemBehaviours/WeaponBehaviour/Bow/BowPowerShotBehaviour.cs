using System;
using Cysharp.Threading.Tasks;
using Enemies;
using Managers_Controllers;
using Markers;
using Player;
using UnityEngine;
using Zenject;

namespace ItemBehaviours.WeaponBehaviour.Bow
{
    public class BowPowerShotBehaviour : WeaponBehaviour
    {
        [SerializeField] private float attackCooldownValue;
        [SerializeField] private float specialCooldownValue;
        [SerializeField] private float animTimeoutValue;
        [SerializeField] private float specialRadius;
        [SerializeField] private float specialDistance;
        [SerializeField] private LayerMask effectLayer;
        [SerializeField] private float recoilValue;
        [SerializeField] private float pushForce;
        
        private static readonly int PowerShotTrigger = Animator.StringToHash("PowerShot");
        
        private int _raycastHits;
        private RaycastHit[] _hitObjects = new RaycastHit[30];

        private readonly float _recoilDelay = 0.1f;


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
            Anim.SetTrigger(PowerShotTrigger);
            
            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout));
            
            var pos = new Vector3(Rb.transform.position.x, 
                Rb.transform.position.y + 1, Rb.transform.position.z);

            var dir = (position - pos).normalized;

            _raycastHits = Physics.SphereCastNonAlloc(position, specialRadius, dir, _hitObjects, specialDistance, effectLayer);
            
            if (_raycastHits > 0)
            {
                for (int i = 0; i < _raycastHits; i++)
                {
                    _hitObjects[i].collider.GetComponent<HurtBox>().GetDamage(Stats.TotalSpecialDamage);
                    if (_hitObjects[i].collider.GetComponent<EnemyBase>())
                    {
                        var rb = _hitObjects[i].collider.GetComponent<Rigidbody>();
                        rb.AddForce(dir * pushForce);
                    }
                }
            }
            
            Rb.AddForce(-dir * recoilValue);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_recoilDelay));

            Rb.isKinematic = true;
            Rb.isKinematic = false;
            
            for (int i = 0; i < _raycastHits; i++)
            {
                if (_hitObjects[i].collider.GetComponent<EnemyBase>())
                {
                    var rb = _hitObjects[i].collider.GetComponent<Rigidbody>();
                    rb.isKinematic = true;
                    rb.isKinematic = false;
                }
            }
        }
    }
}