using System;
using Cysharp.Threading.Tasks;
using Markers;
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
        
        private HurtBox _hurtBox;
        
        private static readonly int DashTrigger = Animator.StringToHash("Dash");

        public override void Init(Animator animator, Rigidbody rigidbody, GameObject barrier)
        {
            base.Init(animator, rigidbody, barrier);
            defenceCooldown = defenceCooldownValue;
            animTimeout = animTimeoutValue;
            _hurtBox = Rb.GetComponent<HurtBox>();
        }

        public override async void Defend(Vector3 direction)
        {
            Physics.IgnoreLayerCollision(playerLayer, enemyLayer, true);
            Anim.SetTrigger(DashTrigger);
            _hurtBox.EnableBlock();
            Rb.AddForce(direction.normalized * _jumpPower, ForceMode.Impulse);
            
            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout));
            
            Physics.IgnoreLayerCollision(playerLayer,enemyLayer, false);
            _hurtBox.DisableBlock();
            Rb.isKinematic = true;  // гасим инерцию
            Rb.isKinematic = false;
        }
    }
}