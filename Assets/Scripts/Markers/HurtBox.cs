using System;
using UnityEngine;

namespace Markers
{
    public class HurtBox : MonoBehaviour
    {
        private bool _damageBlock;

        public event Action<float> IsDamaged;

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
    }
}