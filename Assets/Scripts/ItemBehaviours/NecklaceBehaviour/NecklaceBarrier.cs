using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enemies;
using Markers;
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

        private HurtBox _hurtBox;
        
        private GameObject _barrierView;
        private readonly Vector3 _startScale = new Vector3(3, 3, 3);
        private readonly float _scaleFactor = 10f;
        private readonly float _scaleTime = 0.1f;

        private int _pushTargets;
        private Collider[] _pushColliders = new Collider[30];

        public override void Init(Animator animator, Rigidbody rigidbody,GameObject barrier)
        {
            base.Init(animator, rigidbody, barrier);
            defenceCooldown = defenceCooldownValue;
            animTimeout = animTimeoutValue;
            _hurtBox = rigidbody.GetComponent<HurtBox>();
            _barrierView = barrier;
        }

        public override async void Defend(Vector3 direction)
        {
            Anim.SetTrigger(BarrierTrigger);
            _hurtBox.EnableBlock();
            _barrierView.SetActive(true);

            await UniTask.Delay(TimeSpan.FromSeconds(animTimeout));
            
            _hurtBox.DisableBlock();

            _pushTargets = Physics.OverlapSphereNonAlloc(
                _barrierView.transform.position, pushRadius, _pushColliders, effectLayer);
            
            
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

            await _barrierView.transform.DOScale(_scaleFactor, _scaleTime);

            _barrierView.SetActive(false);
            _barrierView.transform.localScale = _startScale;
            
            Rb.isKinematic = true;
            Rb.isKinematic = false;

            await UniTask.Delay(TimeSpan.FromSeconds(_scaleTime));
            
            foreach (var col in _pushColliders)
            {
                if (col)
                {
                    var rb = col.GetComponent<Rigidbody>();
                    rb.isKinematic = true; // гасим инерцию
                    rb.isKinematic = false;
                }
            }
        }
    }
}