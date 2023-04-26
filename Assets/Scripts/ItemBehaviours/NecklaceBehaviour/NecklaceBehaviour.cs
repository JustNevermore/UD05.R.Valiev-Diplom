using Markers;
using UnityEngine;

namespace ItemBehaviours.NecklaceBehaviour
{
    public abstract class NecklaceBehaviour : MonoBehaviour
    {
        [HideInInspector] public float defenceCooldown;
        [HideInInspector] public float animTimeout;

        protected Rigidbody Rb;
        protected Animator Anim;
        
        public virtual void Init(Animator animator, Rigidbody rigidbody, GameObject barrier)
        {
            Rb = rigidbody;
            Anim = animator;
        }

        public abstract void Defend(Vector3 direction);
    }
}