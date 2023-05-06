using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Managers_Controllers;
using Markers;
using Player;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Enemies
{
    public abstract class EnemyBase : Enemy
    {
        private AttackTrigger _attackTrigger;
        private EscapeTrigger _escapeTrigger;
        protected AttackPosMarker AttackPos;

        protected readonly float RotateSpeed = 200f;
        [SerializeField] private bool canEscape;
        [SerializeField] private float moveUpdateTime;
        [SerializeField] private float attackCooldown;
        [SerializeField] protected float attackDelay;

        private readonly float _targetUpdateTime = 0.5f;

        private readonly float _rndOffset = 10f;

        private Vector3 _direction;

        private Coroutine _actionCor;
        private Coroutine _slowCor;

        protected bool CanMove;
        protected bool CanAttack;
        private bool _tryEscape;
        private bool _attackCd;

        
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
            
            _attackTrigger = GetComponentInChildren<AttackTrigger>();
            _escapeTrigger = GetComponentInChildren<EscapeTrigger>();
            AttackPos = GetComponentInChildren<AttackPosMarker>();

            AdditionalAwake();
        }

        protected abstract void AdditionalAwake();

        private void Start()
        {
            DamageBox.OnGetSlow += GetSlow;
            
            _attackTrigger.OnEnterAttackRange += StartAttack;
            _attackTrigger.OnExitAttackRange += StopAttack;

            if (canEscape)
            {
                _escapeTrigger.OnEnterEscapeRange += StartEscape;
                _escapeTrigger.OnExitEscapeRange += StopEscape;
            }
        }

        private void OnDestroy()
        {
            DamageBox.OnGetSlow -= GetSlow;
            
            _attackTrigger.OnEnterAttackRange -= StartAttack;
            _attackTrigger.OnExitAttackRange -= StopAttack;

            if (canEscape)
            {
                _escapeTrigger.OnEnterEscapeRange -= StartEscape;
                _escapeTrigger.OnExitEscapeRange -= StopEscape;
            }
        }

        private void OnEnable()
        {
            _actionCor = StartCoroutine(ActionUpdateCoroutine());
            
            CanMove = true;
            CanAttack = false;
            _attackCd = false;
            _tryEscape = false;
        }

        private void OnDisable()
        {
            StopCoroutine(_actionCor);
        }

        private void StartAttack()
        {
            CanAttack = true;
        }

        private void StopAttack()
        {
            CanAttack = false;
        }
        
        private void StartEscape()
        {
            _tryEscape = true;
        }
        
        private void StopEscape()
        {
            _tryEscape = false;
        }

        private IEnumerator ActionUpdateCoroutine()
        {
            while (true)
            {
                if (!CanAttack && !_tryEscape)
                {
                    ChaseTarget();
                }
                else if(CanAttack && !_tryEscape)
                {
                    RandomMove();
                }
                else
                {
                    Escape();
                }

                yield return new WaitForSeconds(moveUpdateTime);
            }
        }
        
        private IEnumerator ChaseTargetCoroutine()
        {
            while (true)
            {
                var target = Player.transform.position;
                _direction = (target - transform.position).normalized;

                yield return new WaitForSeconds(_targetUpdateTime);
            }
        }

        private async void ChaseTarget()
        {
            var cor = StartCoroutine(ChaseTargetCoroutine());
            await UniTask.Delay(TimeSpan.FromSeconds(moveUpdateTime));
            StopCoroutine(cor);
        }

        private void RandomMove()
        {
            var pos = transform.position;
            var x = Random.Range(pos.x - _rndOffset, pos.x + _rndOffset);
            var z = Random.Range(pos.z - _rndOffset, pos.z + _rndOffset);

            var target = new Vector3(x, pos.y, z);

            _direction = (target - pos).normalized;
        }
        
        private void Escape()
        {
            var target = Player.transform.position;
            
            _direction = (transform.position - target).normalized;
        }

        private IEnumerator AttackCooldownCoroutine()
        {
            _attackCd = true;
            yield return new WaitForSeconds(attackCooldown);
            _attackCd = false;
        }

        private void FixedUpdate()
        {
            AttackCheck();

            Special();
            
            if (!CanMove)
                return;
            
            ChangeRotation();
            Move();
        }

        private async void AttackCheck()
        {
            if (CanAttack && !_attackCd)
            {
                CanMove = false;
                Attack();
                StartCoroutine(AttackCooldownCoroutine());
                
                await UniTask.Delay(TimeSpan.FromSeconds(attackDelay));

                CanMove = true;
            }
        }

        protected abstract void Attack();

        private void ChangeRotation()
        {
            var rotation = Quaternion.LookRotation(_direction, Vector3.up);
            
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, rotation, RotateSpeed * Time.deltaTime);
        }
        
        protected abstract void Special();
        
        private void Move()
        {
            Rb.MovePosition(transform.position + transform.forward * (moveSpeed * Time.deltaTime));
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