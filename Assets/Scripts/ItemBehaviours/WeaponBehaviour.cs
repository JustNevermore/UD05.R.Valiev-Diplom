using UnityEditor.Animations;
using UnityEngine;

namespace ItemBehaviours
{
    public abstract class WeaponBehaviour : MonoBehaviour
    {
        public abstract void Init(Animator animator);
        
        public abstract void Attack();

        public abstract void Special();
    }
}