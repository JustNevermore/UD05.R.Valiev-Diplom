using System.Collections;
using Managers_Controllers;
using Markers;
using Player;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class BossLich : Enemy
    {
        private AttackPosMarker _attackPos;

        private bool _rageMod;
        
        private readonly float _rotateSpeed = 50f;

        private Coroutine _actionCor;
        private Coroutine _attackCor;
        private Coroutine _specialCor;
        private Coroutine _triggerCor;
        private Coroutine _slowCor;

        private float _triggerDamage;
        private readonly float _triggerLimit = 50f;
        private readonly int _triggerChance = 3;
        private readonly float _damageTriggerTime = 2f;
        private readonly float _damageTriggerTick = 0.5f;

        private readonly float _actionDelay = 2f;
        private readonly int _attackCharge = 3;
        private readonly float _attackCd = 3f;
        
        private readonly int _specialCharge = 3;
        private readonly int _specialCount = 10;
        private readonly float _runeRndOffset = 15f;
        private readonly float _specialDelay = 2f;
        
        private readonly float _rageAttackCd = 0.3f;
        private readonly int _rageAttackCharge = 5;
        private readonly float _rageSpecialDelay = 0.3f;


        [Inject]
        private void Construct(PlayerController playerController, PoolManager poolManager)
        {
            Player = playerController;
            Pool = poolManager;
        }
        
        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
            Stats = GetComponent<EnemyStats>();
            DamageBox = GetComponent<HurtBox>();
            _attackPos = GetComponentInChildren<AttackPosMarker>();
        }
        
        private void Start()
        {
            DamageBox.OnGetSlow += GetSlow;
            DamageBox.OnGetDamage += DamageTrigger;
            Stats.OnEnterRageMode += ActivateRageMode;
        }

        private void OnDestroy()
        {
            DamageBox.OnGetSlow -= GetSlow;
            DamageBox.OnGetDamage -= DamageTrigger;
            Stats.OnEnterRageMode += ActivateRageMode;
        }

        private void OnEnable()
        {
            _actionCor = StartCoroutine(ActionSelectCor());
        }

        private void OnDisable()
        {
            _rageMod = false;
        }

        private IEnumerator ActionSelectCor()
        {
            yield return new WaitForSeconds(_actionDelay);
            
            var rnd = Random.Range(0, 2);
            if (rnd == 0)
            {
                _attackCor = StartCoroutine(AttackCor());
            }
            else
            {
                _specialCor = StartCoroutine(SpecialCor());
            }
        }
        
        private IEnumerator AttackCor()
        {
            if (!_rageMod)
            {
                for (int i = 0; i < _attackCharge; i++)
                {
                    var dir = (Player.transform.position - _attackPos.transform.position).normalized;
                    var spell = Pool.GetBossSpell();
                    spell.transform.position = _attackPos.transform.position;
                    spell.Init(dir, Stats.AttackDamage);
                    spell.Launch();

                    yield return new WaitForSeconds(_attackCd);
                }
            }
            else
            {
                for (int i = 0; i < _attackCharge; i++)
                {
                    for (int j = 0; j < _rageAttackCharge; j++)
                    {
                        var dir = (Player.transform.position - _attackPos.transform.position).normalized;
                        var spell = Pool.GetBossSpell();
                        spell.transform.position = _attackPos.transform.position;
                        spell.Init(dir, Stats.AttackDamage);
                        spell.Launch();

                        yield return new WaitForSeconds(_rageAttackCd);
                    }

                    yield return new WaitForSeconds(_attackCd);
                }
            }
            
            _actionCor = StartCoroutine(ActionSelectCor());
        }
        
        private IEnumerator SpecialCor()
        {
            if (!_rageMod)
            {
                for (int i = 0; i < _specialCharge; i++)
                {
                    for (int j = 0; j < _specialCount; j++)
                    {
                        if (j == 0)
                        {
                            var target = Player.transform.position;
                            var rune = Pool.GetBossRune();
                            rune.transform.position = new Vector3(target.x, transform.position.y, target.z);
                            rune.Init(Stats.SpecialDamage);
                        }
                        else
                        {
                            var target = Player.transform.position;

                            var rndX = Random.Range(target.x - _runeRndOffset, target.x + _runeRndOffset);
                            var rndZ = Random.Range(target.z - _runeRndOffset, target.z + _runeRndOffset);

                            var rune = Pool.GetBossRune();
                            rune.transform.position = new Vector3(rndX, transform.position.y, rndZ);
                            rune.Init(Stats.SpecialDamage);
                        }
                    }

                    yield return new WaitForSeconds(_specialDelay);
                }
            }
            else
            {
                for (int i = 0; i < _specialCharge; i++)
                {
                    for (int j = 0; j < _specialCount; j++)
                    {
                        var target = Player.transform.position;
                        var rune = Pool.GetBossRune();
                        rune.transform.position = new Vector3(target.x, transform.position.y, target.z);
                        rune.Init(Stats.SpecialDamage);

                        yield return new WaitForSeconds(_rageSpecialDelay);
                    }

                    yield return new WaitForSeconds(_specialDelay);
                }
            }

            _actionCor = StartCoroutine(ActionSelectCor());
        }

        private void CounterAttack()
        {
            StopActions();

            var target = Player.transform.position;
            var rune = Pool.GetBossRune();
            rune.transform.position = new Vector3(target.x, transform.position.y, target.z);
            rune.Init(Stats.SpecialDamage);
            
            _actionCor = StartCoroutine(ActionSelectCor());
        }

        private void StopActions()
        {
            StopCoroutine(_actionCor);
            StopCoroutine(_attackCor);
            StopCoroutine(_specialCor);
        }
        
        private void ActivateRageMode()
        {
            _rageMod = true;
        }

        private void FixedUpdate()
        {
            ChangeRotation();
        }
        
        private void ChangeRotation()
        {
            var direction = (Player.transform.position - transform.position).normalized;
            
            var rotation = Quaternion.LookRotation(direction, Vector3.up);
            
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, rotation, _rotateSpeed * Time.deltaTime);
        }
        
        private void DamageTrigger(float damage)
        {
            if (gameObject.activeInHierarchy)
            {
                if (_triggerCor == null)
                {
                    _triggerDamage = damage;
                    _triggerCor = StartCoroutine(DamageTriggerCor());
                }
                else
                {
                    _triggerDamage += damage;
                }
            }
        }

        private IEnumerator DamageTriggerCor()
        {
            var counter = _damageTriggerTime / _damageTriggerTick;
            
            while (counter > 0)
            {
                if (_triggerDamage >= _triggerLimit)
                {
                    var rnd = Random.Range(0, _triggerChance);
                    if (rnd == 0)
                    {
                        CounterAttack();
                        _triggerCor = null;
                        yield break;
                    }
                }

                counter -= _damageTriggerTick;

                yield return new WaitForSeconds(_damageTriggerTick);
            }
            
            _triggerCor = null;
        }

        private void GetSlow()
        {
            if (gameObject.activeInHierarchy)
            {
                if (_slowCor == null)
                {
                    _slowCor = StartCoroutine(SlowCoroutine());
                }
                else
                {
                    StopCoroutine(_slowCor);
                    moveSpeed *= SlowValue;
                    _slowCor = StartCoroutine(SlowCoroutine());
                }
            }
        }

        private IEnumerator SlowCoroutine()
        {
            moveSpeed /= SlowValue;
            
            yield return new WaitForSeconds(SlowDuration);

            moveSpeed *= SlowValue;

            _slowCor = null;
        }
    }
}