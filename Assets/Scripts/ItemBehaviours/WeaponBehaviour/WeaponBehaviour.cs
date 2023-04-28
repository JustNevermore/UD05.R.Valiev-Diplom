using System;
using Cysharp.Threading.Tasks;
using Managers_Controllers;
using Markers;
using Player;
using UnityEngine;
using Zenject;

namespace ItemBehaviours.WeaponBehaviour
{
    public abstract class WeaponBehaviour : MonoBehaviour
    {
        [HideInInspector] public float attackCooldown;
        [HideInInspector] public float specialCooldown;
        [HideInInspector] public float animTimeout;

        protected static readonly int AttackTrigger = Animator.StringToHash("Attack");
        
        protected PlayerController Controller;
        protected PlayerStats Stats;
        protected Animator Anim;
        protected Rigidbody Rb;
        protected PoolManager PoolManager;

        #region SwordProperties
        
        protected readonly float SwordDamageDelay = 0.05f;
        protected readonly float SwordAttackRadius = 2f;
        protected int SwordAttackHit;
        protected Collider[] SwordAttackColliders = new Collider[10];
        
        #endregion

        #region BowProperties

        protected readonly float ShootDelay = 0.1f;
        protected readonly float ProjectileSpeed = 1f;

        #endregion
        
        
        public virtual void Init(PlayerController controller, PlayerStats stats, Animator animator, PoolManager poolManager)
        {
            Controller = controller;
            Stats = stats;
            Anim = animator;
            Rb = controller.Rigbody;
            PoolManager = poolManager;
        }

        public abstract void Attack();

        public abstract void Special();
    }
}