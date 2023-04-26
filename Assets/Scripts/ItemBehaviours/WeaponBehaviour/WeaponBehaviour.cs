using System;
using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;

namespace ItemBehaviours.WeaponBehaviour
{
    public abstract class WeaponBehaviour : MonoBehaviour
    {
        [HideInInspector] public float attackCooldown;
        [HideInInspector] public float specialCooldown;
        [HideInInspector] public float animTimeout;

        protected PlayerStats Stats;
        protected Animator Anim;
        protected static readonly int AttackTrigger = Animator.StringToHash("Attack");

        #region SwordProperties
        
        protected readonly float SwordDamageDelay = 0.05f;
        protected readonly float SwordAttackRadius = 2f;
        protected int SwordAttackHit;
        protected Collider[] SwordAttackColliders = new Collider[10];
        
        #endregion

        public virtual void Init(PlayerStats stats, Animator animator)
        {
            Stats = stats;
            Anim = animator;
        }

        public abstract void Attack(Vector3 attackPoint);

        public abstract void Special(Vector3 position);
    }
}