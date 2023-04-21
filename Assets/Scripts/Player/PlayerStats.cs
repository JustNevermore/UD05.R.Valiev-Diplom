using System;
using System.Collections;
using InventorySystem;
using ItemBehaviours;
using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        private float consumableTick = 0.5f;

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

        private WeaponBehaviour _moveSet;
        private float _attackDamage;
        private float _attackMpCost;
        private int _spellChargeCount;
        private float _specialDamage;
        private float _specialMpCost;
        private GameObject _weaponView;

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

        private void Start()
        {
            //todo не забыть убрать золото
            _gold = 1000000;
            
            RecalculateStats();
        }

        public void IncreaseStats(ItemConfig item)
        {
            _bonusHp += item.BonusHp;
            _bonusMp += item.BonusMp;
            _percentBonusDamage += item.PercentBonusDamage;
            _percentProtection += item.PercentProtection;
            _attackDamage += item.AttackDamage;
            _attackMpCost += item.AttackMpCost;
            _spellChargeCount += item.SpellChargeCount;
            _specialDamage += item.SpecialDamage;
            _specialMpCost += item.SpecialMpCost;
            _elementOfDamage = item.ElementOfDamage;
            _bonusDamage += item.BonusDamage;
            _hasDot = item.HasDot;
            _dotDamage += item.DotDamage;
            _hasSlow = item.HasSlow;
            
            RecalculateStats();
        }

        public void DecreaseStats(ItemConfig item)
        {
            _bonusHp -= item.BonusHp;
            _bonusMp -= item.BonusMp;
            _percentBonusDamage -= item.PercentBonusDamage;
            _percentProtection -= item.PercentProtection;
            _attackDamage -= item.AttackDamage;
            _attackMpCost -= item.AttackMpCost;
            _spellChargeCount -= item.SpellChargeCount;
            _specialDamage -= item.SpecialDamage;
            _specialMpCost -= item.SpecialMpCost;
            _elementOfDamage = item.ElementOfDamage;
            _bonusDamage -= item.BonusDamage;
            _hasDot = item.HasDot;
            _dotDamage -= item.DotDamage;
            _hasSlow = item.HasSlow;
            
            RecalculateStats();
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

        private void RecalculateStats()
        {
            _playerMaxHp = baseMaxHp + _bonusHp;

            _playerMaxMp = baseMaxMp + _bonusMp;

            _totalAttackDamage = _attackDamage + _bonusDamage +
                                 (_attackDamage + _bonusDamage) * 0.01f * _percentBonusDamage;
            _totalSpecialDamage = _specialDamage + _bonusDamage + 
                                  (_specialDamage + _bonusDamage) * 0.01f * _percentBonusDamage;
            _totalDotDamage = _dotDamage + _dotDamage * 0.01f * _percentBonusDamage;
        }

        public void UseConsumable(ItemConfig item)
        {
            //todo подумать над тем как запретить использовать два зелья одновременно
            
            switch (item.TypeOfConsumable)
            {
                case ConsumableType.HpPotion:
                    StartCoroutine(HpCoroutine(item.RestoreTime, item.RestoreAmount));
                    break;
                case ConsumableType.MpPotion:
                    StartCoroutine(MpCoroutine(item.RestoreTime, item.RestoreAmount));
                    break;
                case ConsumableType.HpCrystal:
                    _playerCurrentHp += item.RestoreAmount;
                    break;
                case ConsumableType.MpCrystal:
                    _playerCurrentMp += item.RestoreAmount;
                    break;
            }
        }
        
        // Не могу использовать ref в итераторе, поэтому пришлось разбить логику на две разные корутины
        private IEnumerator HpCoroutine(float time, float value)
        {
            var useTime = 0f;
            var tickValue = value / time * consumableTick;
            while (useTime < time)
            {
                _playerCurrentHp += tickValue;
                useTime += consumableTick;
                yield return new WaitForSeconds(consumableTick);
            }
        }
        
        private IEnumerator MpCoroutine(float time, float value)
        {
            var useTime = 0f;
            var tickValue = value / time * consumableTick;
            while (useTime < time)
            {
                _playerCurrentMp += tickValue;
                useTime += consumableTick;
                yield return new WaitForSeconds(consumableTick);
            }
        }
    }
}