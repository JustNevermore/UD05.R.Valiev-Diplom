﻿using System.Collections;
using Managers_Controllers;
using Markers;
using UnityEngine;
using Zenject;

namespace PoolObjects
{
    public class Spell : MonoBehaviour
    {
        private FxManager _fxManager;
        
        private readonly float _disableTime = 5;
        private readonly float _effectRadius = 3;
        private bool _ready;
        private Vector3 _direction;
        private readonly float _speed = 0.5f;
        private float _damage;
        
        private int _hitTargets;
        private Collider[] _hitColliders = new Collider[30];
        [SerializeField] private LayerMask effectLayer;

        [Inject]
        private void Construct(FxManager fxManager)
        {
            _fxManager = fxManager;
        }
        
        private void FixedUpdate()
        {
            if (!_ready)
                return;

            transform.position += _direction * _speed;
        }

        public void Init(Vector3 direction, float damage)
        {
            _direction = direction;
            _damage = damage;
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
                    _hitColliders[i].GetComponent<HurtBox>().GetDamage(_damage);
                }
            }

            _ready = false;
            gameObject.SetActive(false);
        }

        private IEnumerator DisableTimer()
        {
            yield return new WaitForSeconds(_disableTime);
            
            _ready = false;
            gameObject.SetActive(false);
        }
    }
}