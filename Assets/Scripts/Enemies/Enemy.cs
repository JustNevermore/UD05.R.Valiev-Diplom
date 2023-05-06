using Managers_Controllers;
using Markers;
using Player;
using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        protected PlayerController Player;
        protected Rigidbody Rb;
        protected PoolManager Pool;

        protected EnemyStats Stats;
        protected HurtBox DamageBox;
        
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected LayerMask playerLayer;
        
        protected readonly float SlowDuration = 2f;
        protected readonly float SlowValue = 2f;
    }
}