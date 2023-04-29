using System;
using UnityEngine;

namespace Markers
{
    public class HurtBox : MonoBehaviour
    {
        private bool _damageBlock;

        public event Action<float> IsDamaged;
        public event Action IsSlowed;

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
            
            IsDamaged?.Invoke(damage);
        }

        public void GetDotDamage(float damage)
        {
            if (_damageBlock)
                return;
            
            
        }

        public void GetSlow()
        {
            if (_damageBlock)
                return;
            
            IsSlowed?.Invoke();
        }
    }
}