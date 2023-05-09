using System.Collections;
using Managers_Controllers;
using Markers;
using Player;
using UnityEngine;
using Zenject;

namespace PoolObjects
{
    public class Spell : MonoBehaviour
    {
        private PlayerStats _stats;
        private FxPoolManager _fxPoolManager;
        
        private readonly float _disableTime = 5;
        private readonly float _effectRadius = 3;
        private bool _ready;
        private Vector3 _direction;
        private readonly float _speed = 0.5f;
        private float _damage;
        private bool _hasDot;
        private float _dotDamage;
        private bool _hasSlow;
        
        private int _hitTargets;
        private Collider[] _hitColliders = new Collider[30];
        [SerializeField] private LayerMask effectLayer;
        

        [Inject]
        private void Construct(PlayerStats stats, FxPoolManager fxPoolManager)
        {
            _stats = stats;
            _fxPoolManager = fxPoolManager;
        }
        
        public void Init(Vector3 direction)
        {
            _direction = direction;
            _damage = _stats.TotalAttackDamage;
            
            if (_stats.HasDot)
            {
                _hasDot = true;
                _dotDamage = _stats.TotalDotDamage;
            }

            if (_stats.HasSlow)
            {
                _hasSlow = true;
            }
        }
        
        private void FixedUpdate()
        {
            if (!_ready)
                return;

            transform.position += _direction * _speed;
        }

        public void Launch()
        {
            _ready = true;

            StartCoroutine(DisableTimer());
        }

        private void OnTriggerEnter(Collider col)
        {
            var attackPoint = transform.position;

            _hitTargets = Physics.OverlapSphereNonAlloc(
                attackPoint, _effectRadius, _hitColliders, effectLayer);
            
            if (_hitTargets > 0)
            {
                for (int i = 0; i < _hitTargets; i++)
                {
                    var target = _hitColliders[i].GetComponent<HurtBox>();
                    target.GetDamage(_damage);
                    if(_hasSlow) target.GetSlow();
                    if (_hasDot) target.GetDotDamage(_dotDamage);
                }
            }

            DisableProjectile();
        }

        private IEnumerator DisableTimer()
        {
            yield return new WaitForSeconds(_disableTime);
            
            DisableProjectile();
        }
        
        private void DisableProjectile()
        {
            _hasDot = false;
            _hasSlow = false;
            _ready = false;
            gameObject.SetActive(false);
        }
    }
}