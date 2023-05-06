using System.Collections;
using Markers;
using Player;
using UnityEngine;

namespace PoolObjects
{
    public class BossSpell : MonoBehaviour
    {
        private readonly float _disableTime = 6;
        private bool _ready;
        private Vector3 _direction;
        private readonly float _speed = 0.4f;
        private float _damage;
        

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
            if (col.GetComponent<PlayerController>())
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