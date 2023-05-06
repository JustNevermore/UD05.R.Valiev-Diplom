using System;
using System.Collections;
using UnityEngine;

namespace Markers
{
    public class HurtBox : MonoBehaviour
    {
        private bool _damageBlock;
        private readonly float _dotTickDelay = 0.3f;
        private readonly int _dotTickCount = 3;
        private Coroutine _dotCoroutine;

        public event Action<float> OnGetDamage;
        public event Action OnGetSlow;

        public void EnableBlock()
        {
            _damageBlock = true;
        }

        public void DisableBlock()
        {
            _damageBlock = false;
        }
        
        public void GetDamage(float damage)
        {
            if (_damageBlock)
                return;
            
            OnGetDamage?.Invoke(damage);
        }

        public void GetDotDamage(float damage)
        {
            if (_damageBlock)
                return;

            if (gameObject.activeInHierarchy)
            {
                if (_dotCoroutine == null)
                {
                    _dotCoroutine = StartCoroutine(DotDamageCoroutine(damage));
                }
                else
                {
                    StopCoroutine(_dotCoroutine);
                    _dotCoroutine = StartCoroutine(DotDamageCoroutine(damage));
                }
            }
        }

        public void GetSlow()
        {
            if (_damageBlock)
                return;
            
            OnGetSlow?.Invoke();
        }

        private IEnumerator DotDamageCoroutine(float damage)
        {
            var limit = _dotTickCount;
            while (limit > 0)
            {
                limit--;
                OnGetDamage?.Invoke(damage);
            
                yield return new WaitForSeconds(_dotTickDelay);
            }

            _dotCoroutine = null;
        }
    }
}