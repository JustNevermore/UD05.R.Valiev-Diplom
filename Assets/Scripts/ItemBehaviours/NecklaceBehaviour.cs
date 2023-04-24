using UnityEngine;

namespace ItemBehaviours
{
    public abstract class NecklaceBehaviour : MonoBehaviour
    {
        [HideInInspector]
        public float defenceCooldown;

        public abstract void Init(Animator animator, Rigidbody rigidbody);

        public abstract void Defend(Vector3 direction);
    }
}