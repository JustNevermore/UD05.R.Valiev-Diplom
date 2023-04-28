using System;
using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;

namespace ItemBehaviours.NecklaceBehaviour
{
    public class NecklaceBlink : NecklaceBehaviour
    {
        [SerializeField] private float defenceCooldownValue;
        [SerializeField] private float animTimeoutValue;
        [SerializeField] private LayerMask wallsLayer;
        [SerializeField] private float distance;
        [SerializeField] private float hitDistanceOffset;

        private static readonly int BlinkTrigger = Animator.StringToHash("Blink");

        public override void Init(PlayerController controller, Animator animator)
        {
            base.Init(controller, animator);
            defenceCooldown = defenceCooldownValue;
            animTimeout = animTimeoutValue;
        }

        public override async void Defend()
        {
            Anim.SetTrigger(BlinkTrigger);
            
            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout * 0.5)); // делим длительность анимации на пополам, чтобы половина проигралась до телепортации и половина после

            var startPos = Controller.ZeroPos.transform.position;
            var direction = (Controller.AttackPos.transform.position - startPos).normalized;
            
            Ray ray = new Ray(startPos, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, distance, wallsLayer))
            {
                var endPos = new Vector3(hit.point.x, hit.point.y - 1, hit.point.z);
                Rb.transform.position = endPos - direction * hitDistanceOffset;
            }
            else
            {
                Rb.transform.position += direction * distance;
            }
            
            Rb.isKinematic = true;
            Rb.isKinematic = false;
        }
    }
}