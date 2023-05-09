using System.Collections;
using Enemies;
using Managers_Controllers;
using Markers;
using Player;
using UnityEngine;
using Zenject;

namespace PoolObjects
{
    public class Arrow : MonoBehaviour
    {
        private PlayerStats _stats;
        private FxPoolManager _fxPoolManager;

        private readonly float _disableTime = 4;
        private bool _ready;
        private Vector3 _direction;
        private readonly float _speed = 1f;
        private float _damage;
        private bool _hasDot;
        private float _dotDamage;
        private bool _hasSlow;
        
        
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
            var target = col.GetComponent<HurtBox>();
            target.GetDamage(_damage);
            if(_hasSlow) target.GetSlow();
            if (_hasDot) target.GetDotDamage(_dotDamage);

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