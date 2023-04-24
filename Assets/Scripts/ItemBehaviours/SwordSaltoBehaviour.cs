using System;
using Cysharp.Threading.Tasks;
using Markers;
using Player;
using UnityEngine;

namespace ItemBehaviours
{
    public class SwordSaltoBehaviour : WeaponBehaviour
    {
        [SerializeField] private float attackCooldownValue;
        [SerializeField] private float specialCooldownValue;
        [SerializeField] private float effectRadius;
        [SerializeField] private LayerMask effectLayer;
        private float _animDelay;
        private PlayerStats _stats;

        private Animator _anim;
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        private static readonly int SaltoTrigger = Animator.StringToHash("Salto");

        public override void Init(PlayerStats stats, Animator animator, float animDelay)
        {
            _stats = stats;
            _anim = animator;
            attackCooldown = attackCooldownValue;
            specialCooldown = specialCooldownValue;
            _animDelay = animDelay;
        }

        public override void Attack()
        {
            _anim.SetTrigger(AttackTrigger);
        }

        public override async void Special(Vector3 position)
        {
            _anim.SetTrigger(SaltoTrigger);
            await UniTask.Delay(TimeSpan.FromSeconds(_animDelay));
            Collider[] colliders = Physics.OverlapSphere(position, effectRadius, effectLayer);

            foreach (var col in colliders)
            {
                col.GetComponent<HurtBox>().GetDamage(_stats.TotalSpecialDamage);
            }
        }
    }
}