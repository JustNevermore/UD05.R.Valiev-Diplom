using UnityEngine;

namespace ItemBehaviours
{
    public class SwordGroundBehaviour : WeaponBehaviour
    {
        private Animator _anim;
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        private static readonly int GroundTrigger = Animator.StringToHash("Ground");

        public override void Init(Animator animator)
        {
            _anim = animator;
        }

        public override void Attack()
        {
            _anim.SetTrigger(AttackTrigger);
        }

        public override void Special()
        {
            _anim.SetTrigger(GroundTrigger);
        }
    }
}