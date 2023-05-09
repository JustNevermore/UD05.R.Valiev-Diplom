using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        [SerializeField] private float jumpForce;
        [SerializeField] private LayerMask effectLayer;
        [SerializeField] private LayerMask wallsLayer;
        [SerializeField] private float hitDistanceOffset;

        private static readonly int SaltoTrigger = Animator.StringToHash("Salto");
        
        private int _specialHit;
        private Collider[] _colliders = new Collider[30];
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
            
            FxPool.PlaySlashEffect(Controller.AttackPos.transform.position, Controller.transform.rotation);
            
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
                        var target = SwordAttackColliders[i].GetComponent<HurtBox>();
                        target.GetDamage(Stats.TotalAttackDamage);
                        if(Stats.HasDot) target.GetDotDamage(Stats.TotalDotDamage);
                        if (Stats.HasSlow) target.GetSlow();
                    }
                }
            }
        }

        public override async void Special()
        {
            Anim.SetTrigger(SaltoTrigger);
            
            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout));

            var attackPos = Controller.AttackPos.transform.position;
            
            FxPool.PlayShocwaveEffect(attackPos);
            
            _specialHit = Physics.OverlapSphereNonAlloc(attackPos, effectRadius, _colliders, effectLayer);

            if (_specialHit > 0)
            {
                for (int i = 0; i < _specialHit; i++)
                {
                    var target = _colliders[i].GetComponent<HurtBox>();
                    target.GetDamage(Stats.TotalSpecialDamage);
                    if (Stats.HasDot) target.GetDotDamage(Stats.TotalDotDamage);
                    if (Stats.HasSlow) target.GetSlow();
                    
                    if (_colliders[i].GetComponent<EnemyBase>())
                    {
                        var dir = (_colliders[i].transform.position - Rb.transform.position).normalized;
                        var pos = Rb.transform.position + dir * effectRadius;
                        
                        var startPos = Controller.ZeroPos.transform.position;

                        Ray ray = new Ray(startPos, dir);

                        if (Physics.Raycast(ray, out RaycastHit hit, effectRadius, wallsLayer))
                        {
                            var endPos = new Vector3(hit.point.x, hit.point.y - 1, hit.point.z);
                            pos = endPos - dir * hitDistanceOffset;
                        }

                        _colliders[i].transform.DOJump(pos, jumpForce, 1, _pushTime);
                    }
                }
            }
        }
    }
}