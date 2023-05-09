using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        [SerializeField] private float recoilDistance;
        [SerializeField] private float jumpForce;
        [SerializeField] private float pushDistance;
        [SerializeField] private LayerMask wallsLayer;
        [SerializeField] private float hitDistanceOffset;

        private static readonly int PowerShotTrigger = Animator.StringToHash("PowerShot");
        
        private int _raycastHits;
        private RaycastHit[] _hitObjects = new RaycastHit[30];
        
        private readonly float _pushTime = 0.1f;


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
                    var target = _hitObjects[i].collider.GetComponent<HurtBox>();
                    target.GetDamage(Stats.TotalSpecialDamage);
                    if (Stats.HasDot) target.GetDotDamage(Stats.TotalDotDamage);
                    if (Stats.HasSlow) target.GetSlow();
                    
                    if (_hitObjects[i].collider.GetComponent<EnemyBase>())
                    {
                        var currentPos = _hitObjects[i].transform.position;
                        var targetPos = currentPos + dir * pushDistance;
                        var startPos = new Vector3(currentPos.x, currentPos.y + 1, currentPos.z);

                        Ray ray = new Ray(startPos, dir);

                        if (Physics.Raycast(ray, out RaycastHit hit, pushDistance, wallsLayer))
                        {
                            var endPos = new Vector3(hit.point.x, hit.point.y - 1, hit.point.z);
                            targetPos = endPos - dir * hitDistanceOffset;
                        }

                        _hitObjects[i].transform.DOJump(targetPos, jumpForce, 1, _pushTime);
                    }
                }
            }
            
            var playerPos = Controller.ZeroPos.transform.position;
            
            var recoiledPos = Rb.transform.position + -dir * recoilDistance;

            Ray recoilRay = new Ray(playerPos, -dir);
            
            if (Physics.Raycast(recoilRay, out RaycastHit recoilHit, recoilDistance, wallsLayer))
            {
                var endPos = new Vector3(recoilHit.point.x, recoilHit.point.y - 1, recoilHit.point.z);
                recoiledPos = endPos - -dir * hitDistanceOffset;
            }
            
            Controller.transform.DOJump(recoiledPos, jumpForce, 1, _pushTime);

            await UniTask.Delay(TimeSpan.FromSeconds(_pushTime));

            Rb.isKinematic = true;
            Rb.isKinematic = false;
        }
    }
}