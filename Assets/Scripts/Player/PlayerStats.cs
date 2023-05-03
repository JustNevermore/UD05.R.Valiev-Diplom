using System;
using System.Collections;
using InventorySystem;
using ItemBehaviours;
using ItemBehaviours.NecklaceBehaviour;
using ItemBehaviours.WeaponBehaviour;
using Markers;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        private HurtBox _hurtBox;
        
        private readonly float _percentMpReg = 0.02f;
        
        private SignalBus _signalBus;
        private WeaponHolder _weaponHolder;

        private int _currentGold;

        private int _gold
        {
            get => _currentGold;
            set
            {
                _currentGold = value;
                if (_currentGold < 0)
                {
                    _currentGold = 0;
                }
            }
        }

        private int _currentReviveShards;
        
        private int _reviveShards
        {
            get => _currentReviveShards;
            set
            {
                _currentReviveShards = value;
                if (_currentReviveShards < 0)
                {
                    _currentReviveShards = 0;
                }
            }
        }

        [SerializeField] private float baseMaxHp;
        private float _playerMaxHp;
        private float _currentHp;
        private float _playerCurrentHp
        {
            get => _currentHp;
            set
            {
                _currentHp = value;
                if (_currentHp > _playerMaxHp)
                {
                    _currentHp = _playerMaxHp;
                }
            }
        }

        [SerializeField] private float baseMaxMp;
        private float _playerMaxMp;
        private float _currentMp;
        private float _playerCurrentMp
        {
            get => _currentMp;
            set
            {
                _currentMp = value;
                if (value > _playerMaxMp)
                {
                    _currentMp = _playerMaxMp;
                }
            }
        }

        private float _bonusHp;
        private float _bonusMp;
        private float _percentBonusDamage;
        private float _percentProtection;

        private WeaponType _weaponType;
        private float _attackDamage;
        private float _attackMpCost;
        private int _spellChargeCount;
        private float _specialDamage;
        private float _specialMpCost;
        private WeaponBehaviour _moveSet;

        private NecklaceBehaviour _defenceSkill;
        
        private Element _elementOfDamage;
        private float _bonusDamage;
        private bool _hasDot;
        private float _dotDamage;
        private bool _hasSlow;

        private float _totalAttackDamage;
        private float _totalSpecialDamage;
        private float _totalDotDamage;
        
        public float CurrentGold => _currentGold;
        public float ReviveShards => _currentReviveShards;

        public float PlayerHp => _currentHp;
        public float PlayerMaxHp => _playerMaxHp;
        public float PlayerMp => _currentMp;
        public float PlayerMaxMp => _playerMaxMp;
        public float Protection => _percentProtection;
        
        public float TotalAttackDamage => _totalAttackDamage;
        public float AttackMpCost => _attackMpCost;
        public int SpellChargeCount => _spellChargeCount;
        public float TotalSpecialDamage => _totalSpecialDamage;
        public float SpecialMpCost => _specialMpCost;

        public Element ElementOfDamage => _elementOfDamage;
        public bool HasDot => _hasDot;
        public float TotalDotDamage => _totalDotDamage;
        public bool HasSlow => _hasSlow;
        

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
            _weaponHolder = GetComponentInChildren<WeaponHolder>();
            _hurtBox.OnGetDamage += DecreaseCurrentHp;
            
            //todo не забыть убрать золото
            _gold = 1000000;
            
            ApplyStats();
            StartCoroutine(MpRegCoroutine());
        }

        private void OnDestroy()
        {
            _hurtBox.OnGetDamage -= DecreaseCurrentHp;
        }

        private IEnumerator MpRegCoroutine()
        {
            while (true)
            {
                IncreaseCurrentMp(_playerMaxMp * _percentMpReg);
                yield return new WaitForSeconds(1);
            }
        }

        public void IncreaseStats(ItemConfig item)
        {
            _bonusHp += item.BonusHp;
            _bonusMp += item.BonusMp;
            _percentBonusDamage += item.PercentBonusDamage;
            _percentProtection += item.PercentProtection;

            if (item.WeapType != WeaponType.None)
            {
                _weaponType = item.WeapType;
                _signalBus.Fire(new OnChangeWeaponTypeSignal(_weaponType));
            }
            
            _attackDamage += item.AttackDamage;
            _attackMpCost += item.AttackMpCost;
            _specialDamage += item.SpecialDamage;
            _specialMpCost += item.SpecialMpCost;

            if (item.MoveSet != null)
            {
                _moveSet = item.MoveSet;
                var weapon = Instantiate(item.WeaponView, _weaponHolder.transform);
                _signalBus.Fire(new OnWeaponBehaviourChangeSignal(_moveSet));
            }

            if (item.DefenceSkill != null)
            {
                _defenceSkill = item.DefenceSkill;
                _signalBus.Fire(new OnDefenceSkillChangeSignal(_defenceSkill));
            }

            if (item.ElementOfDamage != Element.None)
            {
                _elementOfDamage = item.ElementOfDamage;
            }
            
            _bonusDamage += item.BonusDamage;

            if (item.HasDot)
            {
                _hasDot = item.HasDot;
            }
            
            _dotDamage += item.DotDamage;

            if (item.HasSlow)
            {
                _hasSlow = item.HasSlow;
            }

            ApplyStats();
        }

        public void DecreaseStats(ItemConfig item)
        {
            _bonusHp -= item.BonusHp;
            _bonusMp -= item.BonusMp;
            _percentBonusDamage -= item.PercentBonusDamage;
            _percentProtection -= item.PercentProtection;
            
            if (item.WeapType != WeaponType.None)
            {
                _weaponType = WeaponType.None;
                _signalBus.Fire(new OnChangeWeaponTypeSignal(_weaponType));
            }
            
            _attackDamage -= item.AttackDamage;
            _attackMpCost -= item.AttackMpCost;
            _specialDamage -= item.SpecialDamage;
            _specialMpCost -= item.SpecialMpCost;

            if (item.MoveSet != null)
            {
                _moveSet = null;
                
                if (_weaponHolder.transform.childCount != 0)
                {
                    foreach (Transform child in _weaponHolder.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }
                
                _signalBus.Fire(new OnWeaponBehaviourChangeSignal(_moveSet));
            }

            if (item.DefenceSkill != null)
            {
                _defenceSkill = null;
                _signalBus.Fire(new OnDefenceSkillChangeSignal(_defenceSkill));
            }
            
            if (item.ElementOfDamage != Element.None)
            {
                _elementOfDamage = Element.None;
            }
            
            _bonusDamage -= item.BonusDamage;
            
            if (item.HasDot)
            {
                _hasDot = false;
            }
            
            _dotDamage -= item.DotDamage;
            
            if (item.HasSlow)
            {
                _hasSlow = false;
            }

            ApplyStats();
        }
        
        public void IncreaseGold(int amount)
        {
            _gold += amount;
        }

        public void DecreaseGold(int amount)
        {
            _gold -= amount;
        }

        private void AddReviveShards(int amount)
        {
            _reviveShards += amount;
        }

        private void RemoveReviveShards(int amount)
        {
            _reviveShards -= amount;
        }

        private void ApplyStats()
        {
            _playerMaxHp = baseMaxHp + _bonusHp;
            _playerMaxMp = baseMaxMp + _bonusMp;

            _totalAttackDamage = _attackDamage + _bonusDamage + _attackDamage * 0.01f * _percentBonusDamage;
            _totalSpecialDamage = _specialDamage + _bonusDamage + _specialDamage * 0.01f * _percentBonusDamage;
            _totalDotDamage = _dotDamage + _dotDamage * 0.01f * _percentBonusDamage;
        }

        public void IncreaseCurrentHp(float amount)
        {
            _playerCurrentHp += amount;
        }
        
        public void DecreaseCurrentHp(float amount)
        {
            _playerCurrentHp -= amount;
            // Debug.Log($"Player get {amount} damage");
        }
        
        public void IncreaseCurrentMp(float amount)
        {
            _playerCurrentMp += amount;
        }
        
        public void DecreaseCurrentMp(float amount)
        {
            _playerCurrentMp -= amount;
        }
    }
}