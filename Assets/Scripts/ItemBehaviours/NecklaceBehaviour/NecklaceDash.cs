using System;
using Cysharp.Threading.Tasks;
using Markers;
using Player;
using UnityEngine;

namespace ItemBehaviours.NecklaceBehaviour
{
    public class NecklaceDash : NecklaceBehaviour
    {
        [SerializeField] private float defenceCooldownValue;
        [SerializeField] private float animTimeoutValue;
        [SerializeField] private int playerLayer;
        [SerializeField] private int enemyLayer;
        private readonly float _jumpPower = 110f;

        private static readonly int DashTrigger = Animator.StringToHash("Dash");

        public override void Init(PlayerController controller, Animator animator)
        {
            base.Init(controller, animator);
            defenceCooldown = defenceCooldownValue;
            animTimeout = animTimeoutValue;
        }

        public override async void Defend()
        {
            var direction = (Controller.AttackPos.transform.position - Controller.ZeroPos.transform.position).normalized;
            
            Physics.IgnoreLayerCollision(playerLayer, enemyLayer, true);
            Anim.SetTrigger(DashTrigger);
            Controller.DamageBox.EnableBlock();
            Rb.AddForce(direction * _jumpPower, ForceMode.Impulse);
            
            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout));
            
            Physics.IgnoreLayerCollision(playerLayer,enemyLayer, false);
            Controller.DamageBox.DisableBlock();
            Rb.isKinematic = true;  // гасим инерцию
            Rb.isKinematic = false;
        }
    }
}