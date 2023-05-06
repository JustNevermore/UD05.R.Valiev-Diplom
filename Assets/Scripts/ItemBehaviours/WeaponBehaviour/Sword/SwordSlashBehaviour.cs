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
            Anim.SetTrigger(SlashTrigger);

            var attackPos = Controller.AttackPos.transform.position;
            var startPos = Controller.ZeroPos.transform.position;
            var dir = (attackPos - startPos).normalized;

            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout - _slashTime));

            Physics.IgnoreLayerCollision(playerLayer, enemyLayer, true);
            Controller.DamageBox.EnableBlock();
            Rb.AddForce(dir * _jumpPower, ForceMode.Impulse);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_slashTime));

            Rb.isKinematic = true;
            Rb.isKinematic = false;

            Physics.IgnoreLayerCollision(playerLayer, enemyLayer, false);
            Controller.DamageBox.DisableBlock();

            var finishPos = Controller.ZeroPos.transform.position;
            var distance = (startPos - finishPos).magnitude;
            
            _raycastHits = Physics.SphereCastNonAlloc(finishPos, SwordAttackRadius, -dir, _hitObjects, distance, effectLayer);

            if (_raycastHits > 0)
            {
                for (int i = 0; i < _raycastHits; i++)
                {
                    var target = _hitObjects[i].collider.GetComponent<HurtBox>();
                    target.GetDamage(Stats.TotalSpecialDamage);
                    if (Stats.HasDot) target.GetDotDamage(Stats.TotalDotDamage);
                    if (Stats.HasSlow) target.GetSlow();
                }
            }
        }
    }
}