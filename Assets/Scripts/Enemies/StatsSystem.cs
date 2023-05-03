using System;
using Markers;
using Signals;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class StatsSystem : MonoBehaviour
    {
        private SignalBus _signalBus;
        private HurtBox _hurtBox;

        private float _currentHp;

        private float hp
        {
            get => _currentHp;

            set
            {
                _currentHp = value;
                if (_currentHp <= 0)
                {
                    _signalBus.Fire<OnEnemyDeathSignal>();
                    gameObject.SetActive(false);
                }
            }
        }

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void Awake()
        {
            _hurtBox = GetComponent<HurtBox>();
        }

        private void Start()
        {
            _hurtBox.OnGetDamage += IncomingDamage;
        }
        
        private void OnEnable()
        {
            _currentHp = 100f;
        }

        private void OnDestroy()
        {
            _hurtBox.OnGetDamage -= IncomingDamage;
        }
        
        private void IncomingDamage(float damage)
        {
            // Debug.Log($"Enemy get {damage} damage");
            hp -= damage;
        }
    }
}