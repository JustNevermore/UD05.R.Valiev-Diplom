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
            arrow.Init(dir, ArrowSpeed, Stats.TotalAttackDamage);
            arrow.Launch();
        }

        public override async void Special()
        {
            Anim.SetTrigger(PowerShotTrigger);
            
            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout));
            
            var attackPos = Controller.AttackPos.transform.position;
            var zeroPos = Controller.ZeroPos.transform.position;

            var dir = (attackPos - zeroPos).normalized;

            _raycastHits = Physics.SphereCastNonAlloc(attackPos, specialRadius, dir, _hitObjects, specialDistance, effectLayer);
            
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