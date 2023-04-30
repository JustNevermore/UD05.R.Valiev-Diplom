using System;
using UnityEngine;

namespace Markers
{
    public class HurtBox : MonoBehaviour
    {
        private bool _damageBlock;

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
            
            
        }

        public void GetSlow()
        {
            if (_damageBlock)
                return;
            
            OnGetSlow?.Invoke();
        }
    }
}