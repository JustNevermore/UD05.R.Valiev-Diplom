using System;
using Cysharp.Threading.Tasks;
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
        
        private Vector3 _raycastStartPoint;

        public override void Init(Animator animator, Rigidbody rigidbody, GameObject barrier)
        {
            base.Init(animator, rigidbody, barrier);
            defenceCooldown = defenceCooldownValue;
            animTimeout = animTimeoutValue;
        }

        public override async void Defend(Vector3 direction)
        {
            Anim.SetTrigger(BlinkTrigger);
            
            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout * 0.5)); // делим длительность анимации
                                                                          //  на пополам, чтобы половина
            var pos = Rb.transform.position;                        // проигралась до телепортации
            _raycastStartPoint = new Vector3(pos.x, pos.y + 1, pos.z);   //  и половина после
            Ray ray = new Ray(_raycastStartPoint, direction.normalized);

            if (Physics.Raycast(ray, out RaycastHit hit, distance, wallsLayer))
            {
                var endPos = new Vector3(hit.point.x, hit.point.y - 1, hit.point.z);
                Rb.isKinematic = true;
                Rb.isKinematic = false;
                Rb.transform.position = endPos - direction.normalized * hitDistanceOffset;
            }
            else
            {
                Rb.transform.position += direction.normalized * distance;
            }
        }
    }
}