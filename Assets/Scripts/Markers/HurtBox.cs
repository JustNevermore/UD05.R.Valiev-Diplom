using System;
using UnityEngine;

namespace Markers
{
    public class HurtBox : MonoBehaviour
    {
        public event Action<float> IsDamaged;
            
        public void GetDamage(float damage)
        {
            IsDamaged?.Invoke(damage);
        }
    }
}