using System;
using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;

namespace ItemBehaviours.NecklaceBehaviour
{
    public class NecklaceRoll : NecklaceBehaviour
    {
        [SerializeField] private float defenceCooldownValue;
        [SerializeField] private float animTimeoutValue;

        private static readonly int RollTrigger = Animator.StringToHash("Roll");
        
        private readonly float _jumpPower = 15f;

        public override void Init(PlayerController controller, Animator animator)
        {
            base.Init(controller, animator);
            defenceCooldown = defenceCooldownValue;
            animTimeout = animTimeoutValue;
        }

        public override async void Defend()
        {
            var direction = (Controller.AttackPos.transform.position - Controller.ZeroPos.transform.position).normalized;
            
            Anim.SetTrigger(RollTrigger);
            Rb.AddForce(direction * _jumpPower, ForceMode.Impulse);
            
            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout));
            
            Rb.isKinematic = true; // гасим инерцию
            Rb.isKinematic = false;
        }
    }
}