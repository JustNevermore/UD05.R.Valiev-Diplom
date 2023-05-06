using System.Collections;
using Managers_Controllers;
using Markers;
using Player;
using UnityEngine;
using Zenject;

namespace PoolObjects
{
    public class BossRune : MonoBehaviour
    {
        private FxManager _fxManager;
        
        private readonly float _disableTime = 1f;
        private float _damage;
        [SerializeField] private float radius;

        private int _hitTargets;
        private Collider[] _hitColliders = new Collider[5];
        [SerializeField] private LayerMask effectLayer;

        
        [Inject]
        private void Construct(FxManager fxManager)
        {
            _fxManager = fxManager;
        }
        
        public void Init(float damage)
        {
            _damage = damage;

            StartCoroutine(DisableTimer());
        }

        private IEnumerator DisableTimer()
        {
            yield return new WaitForSeconds(_disableTime);
            
            var attackPoint = transform.position;

            _hitTargets = Physics.OverlapSphereNonAlloc(
                attackPoint, radius, _hitColliders, effectLayer);
            
            if (_hitTargets > 0)
            {
                for (int i = 0; i < _hitTargets; i++)
                {
                    _hitColliders[i].GetComponent<HurtBox>().GetDamage(_damage);
                }
            }
            
            gameObject.SetActive(false);
        }
    }
}