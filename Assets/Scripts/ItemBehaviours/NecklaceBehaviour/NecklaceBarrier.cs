using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enemies;
using Markers;
using Player;
using UnityEngine;

namespace ItemBehaviours.NecklaceBehaviour
{
    public class NecklaceBarrier : NecklaceBehaviour
    {
        [SerializeField] private float defenceCooldownValue;
        [SerializeField] private float animTimeoutValue;
        [SerializeField] private float pushRadius;
        [SerializeField] private float pushForce;
        [SerializeField] private LayerMask effectLayer;
        
        private static readonly int BarrierTrigger = Animator.StringToHash("Barrier");
        
        private readonly float _scaleFactor = 10f;
        private readonly float _scaleTime = 0.1f;

        private int _pushTargets;
        private Collider[] _pushColliders = new Collider[30];

        public override void Init(PlayerController controller, Animator animator)
        {
            base.Init(controller, animator);
            defenceCooldown = defenceCooldownValue;
            animTimeout = animTimeoutValue;
        }

        public override async void Defend()
        {
            Anim.SetTrigger(BarrierTrigger);
            Controller.DamageBox.EnableBlock();
            
            var barrier = Controller.BarrierView;
            var startScale = barrier.transform.localScale;
            barrier.SetActive(true);

            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout));
            
            Controller.DamageBox.DisableBlock();

            _pushTargets = Physics.OverlapSphereNonAlloc(
                Controller.ZeroPos.transform.position, pushRadius, _pushColliders, effectLayer);
            
            
            if (_pushTargets > 0)
            {
                for (int i = 0; i < _pushTargets; i++)
                {
                    if (_pushColliders[i].GetComponent<EnemyBase>())
                    {
                        var rb = _pushColliders[i].GetComponent<Rigidbody>();
                        rb.AddExplosionForce(pushForce, Rb.transform.position, pushRadius);
                    }
                }
            }

            await barrier.transform.DOScale(_scaleFactor, _scaleTime);

            barrier.SetActive(false);
            barrier.transform.localScale = startScale;
            
            Rb.isKinematic = true;
            Rb.isKinematic = false;

            await UniTask.Delay(TimeSpan.FromSeconds(_scaleTime));

            if (_pushTargets > 0)
            {
                for (int i = 0; i < _pushTargets; i++)
                {
                    var rb = _pushColliders[i].GetComponent<Rigidbody>();
                    rb.isKinematic = true; // гасим инерцию
                    rb.isKinematic = false;
                }
            }
        }
    }
}