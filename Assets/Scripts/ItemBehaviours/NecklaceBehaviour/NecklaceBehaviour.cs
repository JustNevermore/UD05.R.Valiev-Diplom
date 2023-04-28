using Markers;
using Player;
using UnityEngine;

namespace ItemBehaviours.NecklaceBehaviour
{
    public abstract class NecklaceBehaviour : MonoBehaviour
    {
        [HideInInspector] public float defenceCooldown;
        [HideInInspector] public float animTimeout;

        protected PlayerController Controller;
        protected Rigidbody Rb;
        protected Animator Anim;
        
        public virtual void Init(PlayerController controller, Animator animator)
        {
            Controller = controller;
            Rb = controller.Rigbody;
            Anim = animator;
        }

        public abstract void Defend();
    }
}