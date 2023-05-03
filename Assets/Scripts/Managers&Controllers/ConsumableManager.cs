using System.Collections;
using InventorySystem;
using Player;
using UnityEngine;
using Zenject;

namespace Managers_Controllers
{
    public class ConsumableManager : MonoBehaviour
    {
        private readonly float _consumableTick = 0.5f;

        private PlayerStats _playerStats;

        private Coroutine _hpCoroutine;
        private Coroutine _mpCoroutine;


        [Inject]
        private void Construct(PlayerStats playerStats)
        {
            _playerStats = playerStats;
        }

        public void UseConsumable(ItemConfig config, Item item)
        {
            switch (config.TypeOfConsumable)
            {
                case ConsumableType.HpPotion:
                    if (_hpCoroutine == null)
                    {
                        _hpCoroutine = StartCoroutine(HpCoroutine(config.RestoreTime, config.RestoreAmount));
                        item.Data.ItemAmount -= 1;
                    }
                    break;
                case ConsumableType.MpPotion:
                    if (_hpCoroutine == null)
                    {
                        _mpCoroutine = StartCoroutine(MpCoroutine(config.RestoreTime, config.RestoreAmount));
                        item.Data.ItemAmount -= 1;
                    }
                    break;
                case ConsumableType.HpCrystal:
                    _playerStats.IncreaseCurrentHp(config.RestoreAmount);
                    item.Data.ItemAmount -= 1;
                    break;
                case ConsumableType.MpCrystal:
                    _playerStats.IncreaseCurrentMp(config.RestoreAmount);
                    item.Data.ItemAmount -= 1;
                    break;
            }
        }
        
        private IEnumerator HpCoroutine(float time, float value)
        {
            var useTime = 0f;
            var tickValue = value / time * _consumableTick;
            while (useTime < time)
            {
                _playerStats.IncreaseCurrentHp(tickValue);
                useTime += _consumableTick;
                yield return new WaitForSeconds(_consumableTick);
            }

            _hpCoroutine = null;
        }
        
        private IEnumerator MpCoroutine(float time, float value)
        {
            var useTime = 0f;
            var tickValue = value / time * _consumableTick;
            while (useTime < time)
            {
                _playerStats.IncreaseCurrentMp(tickValue);
                useTime += _consumableTick;
                yield return new WaitForSeconds(_consumableTick);
            }

            _mpCoroutine = null;
        }
    }
}