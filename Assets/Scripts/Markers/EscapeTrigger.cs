using System;
using Player;
using UnityEngine;

namespace Markers
{
    public class EscapeTrigger : MonoBehaviour
    {
        public event Action OnEnterEscapeRange;
        public event Action OnExitEscapeRange;
        
        private void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<PlayerController>())
            {
                OnEnterEscapeRange?.Invoke();
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.GetComponent<PlayerController>())
            {
                OnExitEscapeRange?.Invoke();
            }
        }
    }
}