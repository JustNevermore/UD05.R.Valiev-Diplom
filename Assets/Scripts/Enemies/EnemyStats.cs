using System;
using Markers;
using Signals;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class EnemyStats : MonoBehaviour
    {
        private SignalBus _signalBus;
        private HurtBox _hurtBox;

        public event Action OnEnterRageMode; 

        [SerializeField] private EnemyType type;
        [SerializeField] private float maxHp;
        [SerializeField] private float attackDamage;
        [SerializeField] private float specialDamage;
        [SerializeField] private float healthLimit;

        private float _currentHp;

        private float hp
        {
            get => _currentHp;

            set
            {
                _currentHp = value;

                if (type == EnemyType.Boss && _currentHp <= maxHp * healthLimit)
                {
                    OnEnterRageMode?.Invoke();
                }
                
                if (_currentHp <= 0)
                {
                    if (type == EnemyType.Common)
                    {
                        _signalBus.Fire<OnEnemyDeathSignal>();
                    }
                    else
                    {
                        
                    }
                    
                    gameObject.SetActive(false);
                }
            }
        }

        public float AttackDamage => attackDamage;
        public float SpecialDamage => specialDamage;


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
            _currentHp = maxHp;
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