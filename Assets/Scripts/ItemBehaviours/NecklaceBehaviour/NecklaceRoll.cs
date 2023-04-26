using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ItemBehaviours.NecklaceBehaviour
{
    public class NecklaceRoll : NecklaceBehaviour
    {
        [SerializeField] private float defenceCooldownValue;
        [SerializeField] private float animTimeoutValue;

        private static readonly int RollTrigger = Animator.StringToHash("Roll");
        
        private readonly float _jumpPower = 15f;

        public override void Init(Animator animator, Rigidbody rigidbody, GameObject barrier)
        {
            base.Init(animator, rigidbody, barrier);
            defenceCooldown = defenceCooldownValue;
            animTimeout = animTimeoutValue;
        }

        public override async void Defend(Vector3 direction)
        {
            Anim.SetTrigger(RollTrigger);
            Rb.AddForce(direction.normalized * _jumpPower, ForceMode.Impulse);
            
            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout));
            
            Rb.isKinematic = true; // гасим инерцию
            Rb.isKinematic = false;
        }
    }
}