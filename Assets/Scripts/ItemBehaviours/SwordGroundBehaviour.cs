using System;
using System.Numerics;
using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace ItemBehaviours
{
    public class SwordGroundBehaviour : WeaponBehaviour
    {
        [SerializeField] private float attackCooldownValue;
        [SerializeField] private float specialCooldownValue;
        private float _animDelay;
        
        private Animator _anim;
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        private static readonly int GroundTrigger = Animator.StringToHash("Ground");

        public override void Init(PlayerStats stats, Animator animator, float animDelay)
        {
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
            _anim.SetTrigger(GroundTrigger);
            await UniTask.Delay(TimeSpan.FromSeconds(_animDelay));
        }
    }
}