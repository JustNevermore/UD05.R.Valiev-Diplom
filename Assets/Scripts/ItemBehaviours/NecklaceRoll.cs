using UnityEngine;

namespace ItemBehaviours
{
    public class NecklaceRoll : NecklaceBehaviour
    {
        [SerializeField] private float defenceCooldownValue;
        private readonly float _jumpPower = 15f;

        private Rigidbody _rigidbody;
        
        private Animator _anim;
        private static readonly int RollTrigger = Animator.StringToHash("Roll");

        public override void Init(Animator animator, Rigidbody rigidbody)
        {
            _rigidbody = rigidbody;
            _anim = animator;
            defenceCooldown = defenceCooldownValue;
        }

        public override void Defend(Vector3 direction)
        {
            _anim.SetTrigger(RollTrigger);
            _rigidbody.AddForce(direction.normalized * _jumpPower, ForceMode.Impulse);
        }
    }
}