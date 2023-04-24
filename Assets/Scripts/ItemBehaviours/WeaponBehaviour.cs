using Player;
using UnityEditor.Animations;
using UnityEngine;

namespace ItemBehaviours
{
    public abstract class WeaponBehaviour : MonoBehaviour
    {
        [HideInInspector]
        public float attackCooldown;
        [HideInInspector]
        public float specialCooldown;

        public abstract void Init(PlayerStats stats, Animator animator, float animDelay);
        
        public abstract void Attack();

        public abstract void Special(Vector3 position);
    }
}