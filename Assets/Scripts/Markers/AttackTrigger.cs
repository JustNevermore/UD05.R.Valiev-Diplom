using System;
using Player;
using UnityEngine;

namespace Markers
{
    public class AttackTrigger : MonoBehaviour
    {
        public event Action OnEnterAttackRange;
        public event Action OnExitAttackRange;
        
        private void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<PlayerController>())
            {
                OnEnterAttackRange?.Invoke();
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.GetComponent<PlayerController>())
            {
                OnExitAttackRange?.Invoke();
            }
        }
    }
}