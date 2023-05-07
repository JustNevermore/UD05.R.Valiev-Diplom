using System;
using Managers_Controllers;
using Markers;
using Signals;
using Ui;
using Ui.Status;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class EnemyStats : MonoBehaviour
    {
        private SignalBus _signalBus;
        private LootDropManager _dropManager;
        private HurtBox _hurtBox;

        public event Action OnEnterRageMode;
        public event Action<float> OnChangeHpAmount;

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

                var amount = _currentHp / maxHp;
                OnChangeHpAmount?.Invoke(amount);

                if (type == EnemyType.Boss && _currentHp <= maxHp * healthLimit)
                {
                    OnEnterRageMode?.Invoke();
                }
                
                if (_currentHp <= 0)
                {
                    Death();
                }
            }
        }

        public float AttackDamage => attackDamage;
        public float SpecialDamage => specialDamage;


        [Inject]
        private void Construct(SignalBus signalBus, LootDropManager dropManager)
        {
            _signalBus = signalBus;
            _dropManager = dropManager;
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
            hp = maxHp;
            OnChangeHpAmount?.Invoke(hp);
        }

        private void OnDestroy()
        {
            _hurtBox.OnGetDamage -= IncomingDamage;
        }
        
        private void IncomingDamage(float damage)
        {
            hp -= damage;
        }
        
        private void Death()
        {
            if (type == EnemyType.Common)
            {
                _signalBus.Fire<OnEnemyDeathSignal>();
            }
            else
            {
                _signalBus.Fire<OnBossDeathSignal>();
            }
                    
            _dropManager.DropReward(type, transform.position);
            gameObject.SetActive(false);
        }
    }
}