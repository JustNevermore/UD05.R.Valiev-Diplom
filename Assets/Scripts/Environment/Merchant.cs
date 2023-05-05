using System;
using Player;
using Signals;
using UnityEngine;
using Zenject;

namespace Environment
{
    public class Merchant : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<PlayerController>())
            {
                _signalBus.Fire<OpenMerchantSignal>();
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.GetComponent<PlayerController>())
            {
                _signalBus.Fire<CloseMerchantSignal>();
            }
        }
    }
}