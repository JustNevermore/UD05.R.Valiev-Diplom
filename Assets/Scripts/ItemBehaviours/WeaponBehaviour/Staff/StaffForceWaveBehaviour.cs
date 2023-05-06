using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enemies;
using Managers_Controllers;
using Markers;
using Player;
using UnityEngine;

namespace ItemBehaviours.WeaponBehaviour.Staff
{
    public class StaffForceWaveBehaviour : WeaponBehaviour
    {
        [SerializeField] private float attackCooldownValue;
        [SerializeField] private float specialCooldownValue;
        [SerializeField] private float animTimeoutValue;
        [SerializeField] private float effectRadius;
        [SerializeField] private float jumpForce;
        [SerializeField] private LayerMask effectLayer;
        [SerializeField] private LayerMask wallsLayer;
        [SerializeField] private float hitDistanceOffset;
        
        private int _specialHit;
        private Collider[] _colliders = new Collider[30];
        private readonly float _pushTime = 0.1f;


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

            await UniTask.Delay(TimeSpan.FromSeconds(CastDelay));

            var attackPos = Controller.AttackPos.transform.position;
            var zeroPos = Controller.ZeroPos.transform.position;
            var dir = (attackPos - zeroPos).normalized;
            
            var spell = Pool.GetSpell();
            spell.transform.position = attackPos;
            spell.Init(dir);
            spell.Launch();
        }

        public override async void Special()
        {
            
            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout));
            
            var attackPos = Controller.ZeroPos.transform.position;

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