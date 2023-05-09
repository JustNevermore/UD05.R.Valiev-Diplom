using System;
using System.Collections;
using Managers_Controllers;
using UnityEngine;
using Zenject;

namespace Markers
{
    public class HurtBox : MonoBehaviour
    {
        private FxPoolManager _fxManager;
        
        private bool _damageBlock;
        private readonly float _dotTickDelay = 1f;
        private readonly int _dotTickCount = 3;
        private Coroutine _dotCoroutine;

        public event Action<float> OnGetDamage;
        public event Action OnGetSlow;


        [Inject]
        private void Construct(FxPoolManager fxPoolManager)
        {
            _fxManager = fxPoolManager;
        }

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
            _fxManager.PlayHitEffect(transform.position);
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
                _fxManager.PlayDotEffect(transform.position);
            
                yield return new WaitForSeconds(_dotTickDelay);
            }

            _dotCoroutine = null;
        }
    }
}