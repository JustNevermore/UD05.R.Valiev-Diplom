using System.Collections;
using Managers_Controllers;
using Markers;
using Player;
using UnityEngine;
using Zenject;

namespace PoolObjects
{
    public class Rune : MonoBehaviour
    {
        private PlayerStats _stats;
        private FxManager _fxManager;
        
        private readonly float _disableTime = 10f;
        private float _damage;
        private bool _hasDot;
        private float _dotDamage;
        private bool _hasSlow;
        [SerializeField] private float radius;

        private int _hitTargets;
        private Collider[] _hitColliders = new Collider[30];
        [SerializeField] private LayerMask effectLayer;

        
        [Inject]
        private void Construct(PlayerStats stats, FxManager fxManager)
        {
            _stats = stats;
            _fxManager = fxManager;
        }
        
        public void Init()
        {
            _damage = _stats.TotalSpecialDamage;
            
            if (_stats.HasDot)
            {
                _hasDot = true;
                _dotDamage = _stats.TotalDotDamage;
            }

            if (_stats.HasSlow)
            {
                _hasSlow = true;
            }

            StartCoroutine(DisableTimer());
        }

        private void OnTriggerEnter(Collider col)
        {
            var attackPoint = transform.position;

            _hitTargets = Physics.OverlapSphereNonAlloc(
                attackPoint, radius, _hitColliders, effectLayer);
            
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
            
            gameObject.SetActive(false);
        }
        
        private IEnumerator DisableTimer()
        {
            yield return new WaitForSeconds(_disableTime);
            
            gameObject.SetActive(false);
        }
    }
}