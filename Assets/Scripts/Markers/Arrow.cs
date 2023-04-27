using System;
using System.Collections;
using System.Security.Cryptography;
using Enemies;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Markers
{
    public class Arrow : MonoBehaviour
    {
        private readonly float _disableTime = 4;
        private bool _ready;
        private Vector3 _direction;
        private float _speed;
        private float _damage;
        

        private void FixedUpdate()
        {
            if (!_ready)
                return;

            transform.position += _direction * _speed;
        }

        public void Init(Vector3 direction, float speed, float damage)
        {
            _direction = direction;
            _speed = speed;
            _damage = damage;
        }

        public void Launch()
        {
            _ready = true;

            StartCoroutine(DisableTimer());
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<EnemyBase>())
            {
                col.GetComponent<HurtBox>().GetDamage(_damage);
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