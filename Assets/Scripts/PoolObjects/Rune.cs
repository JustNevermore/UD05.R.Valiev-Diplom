using System.Collections;
using Markers;
using Player;
using UnityEngine;

namespace PoolObjects
{
    public class Rune : MonoBehaviour
    {
        private readonly float _disableTime = 10f;
        private float _damage;
        [SerializeField] private float radius;

        private int _hitTargets;
        private Collider[] _hitColliders = new Collider[30];
        [SerializeField] private LayerMask effectLayer;

        public void Init(float damage)
        {
            _damage = damage;

            StartCoroutine(DisableTimer());
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<PlayerController>())
                return;

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
        
        private IEnumerator DisableTimer()
        {
            yield return new WaitForSeconds(_disableTime);
            
            gameObject.SetActive(false);
        }
    }
}