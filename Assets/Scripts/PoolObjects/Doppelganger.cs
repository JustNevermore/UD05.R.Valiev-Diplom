using System.Collections;
using Managers_Controllers;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace PoolObjects
{
    public class Doppelganger : MonoBehaviour
    {
        private readonly float _disableTime = 5f;
        private float _damage;
        private float _speed;
        private float _interval;
        private bool _ready;
        [SerializeField] private float radius;
        [SerializeField] private LayerMask enemyLayer;

        private PoolManager _pool;

        private Vector3 _attackPoint;

        private int _hitTargets;
        private Collider[] _hitColliders = new Collider[30];

        public void Init(float damage, float speed, float interval, PoolManager pool)
        {
            _damage = damage;
            _speed = speed;
            _interval = interval;
            _pool = pool;

            _attackPoint = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
            
            _ready = true;

            StartCoroutine(ActivateTurret());
            StartCoroutine(DisableTimer());
        }
        
        private IEnumerator ActivateTurret()
        {
            while (_ready)
            {
                _hitTargets = Physics.OverlapSphereNonAlloc(
                    _attackPoint, radius, _hitColliders, enemyLayer);
            
                if (_hitTargets > 0)
                {
                    if (_hitTargets > 1)
                    {
                        var rnd = Random.Range(0, _hitTargets);
                        var enemyPos = _hitColliders[rnd].transform.position;
                        var dir = (enemyPos - _attackPoint).normalized;
                        
                        var spell = _pool.GetSpell();
                        spell.transform.position = _attackPoint;
                        spell.Init(dir, _speed, _damage);
                        spell.Launch();
                    }
                    else
                    {
                        var enemyPos = _hitColliders[0].transform.position;
                        var dir = (enemyPos - _attackPoint).normalized;
                        
                        var spell = _pool.GetSpell();
                        spell.transform.position = _attackPoint;
                        spell.Init(dir, _speed, _damage);
                        spell.Launch();
                    }
                }
                
                yield return new WaitForSeconds(_interval);
            }
        }
        
        private IEnumerator DisableTimer()
        {
            yield return new WaitForSeconds(_disableTime);

            _ready = false;
            gameObject.SetActive(false);
        }
    }
}