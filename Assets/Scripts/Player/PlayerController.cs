using System;
using System.Collections;
using ItemBehaviours;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private PlayerAnimationController _anim;
        private SpecialPositionMarker _specialPoint;
        private Vector3 _specialPointPos;
        
        private SignalBus _signalBus;
        private PlayerStats _playerStats;

        [Header("Variables")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotateSpeed = 500f;
        [SerializeField, Range(0f, 1f)] private float turnDeadZone = 0.4f;
        [SerializeField] private float moveTimeout;
        private readonly float _animDelay = 0.3f;

        private const string HorizontalAxis = "Horizontal";
        private const string VerticalAxis = "Vertical";

        private Vector3 _input;
        private Vector3 _direction;

        private WeaponBehaviour _moveSet;
        private NecklaceBehaviour _defenceSkill;

        private float _attackCooldown;
        private float _specialCooldown;
        private float _defenceCooldown;

        private bool _isMoving;
        private bool _canMove;
        private bool _canAttack;
        private bool _canSpecial;
        private bool _canDefend;


        [Inject]
        private void Construct(SignalBus signalBus , PlayerStats playerStats)
        {
            _signalBus = signalBus;
            _playerStats = playerStats;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _anim = GetComponent<PlayerAnimationController>();
            _specialPoint = GetComponentInChildren<SpecialPositionMarker>();
        }

        private void Start()
        {
            _canMove = true;
            _canAttack = true;
            _canSpecial = true;
            _canDefend = true;
            
            _signalBus.Subscribe<OnWeaponBehawiourChangeSignal>(UpdateMoveSet);
            _signalBus.Subscribe<OnDefenceSkillChangeSignal>(UpdateDefenceSkill);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnWeaponBehawiourChangeSignal>(UpdateMoveSet);
            _signalBus.Unsubscribe<OnDefenceSkillChangeSignal>(UpdateDefenceSkill);
        }

        private void Update()
        {
            GetActionInput();
            GetMoveInput();

            _anim.Move(_isMoving);
        }

        private void FixedUpdate()
        {
            if (!_canMove)
                return;
            
            ChangeRotation();
            Move();
        }

        private void UpdateMoveSet(OnWeaponBehawiourChangeSignal signal)
        {
            _moveSet = signal.Behaviour;
            if (_moveSet != null)
            {
                _moveSet.Init(_playerStats, _anim.Anim, _animDelay);
                _attackCooldown = _moveSet.attackCooldown;
                _specialCooldown = _moveSet.specialCooldown;
            }
        }

        private void UpdateDefenceSkill(OnDefenceSkillChangeSignal signal)
        {
            _defenceSkill = signal.DefenceSkill;
            if (_defenceSkill != null)
            {
                _defenceSkill.Init(_anim.Anim, _rigidbody);
                _defenceCooldown = _defenceSkill.defenceCooldown;
            }
        }

        private void GetMoveInput()
        {
            _input = new Vector3(SimpleInput.GetAxis(HorizontalAxis), 0, SimpleInput.GetAxis(VerticalAxis));
        }

        private void ChangeRotation()
        {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
            var isoInput = matrix.MultiplyPoint3x4(_input);
            
            if (isoInput.x != 0 && isoInput.z != 0)
            {
                _direction = isoInput;
            }

            if (_input != Vector3.zero)
            {
                var lookOffset = (transform.position + isoInput) - transform.position;
                var rotation = Quaternion.LookRotation(lookOffset, Vector3.up);

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation, rotation, rotateSpeed * Time.deltaTime);
            }
        }

        private void Move()
        {
            if (_input.magnitude > turnDeadZone)
            {
                _rigidbody.MovePosition(transform.position + transform.forward *
                    (_input.magnitude * moveSpeed * Time.deltaTime));

                _isMoving = true;
            }
            else
            {
                _isMoving = false;
            }
        }

        private void GetActionInput()
        {
            if (!_moveSet)
                return;

            if (Input.GetKeyDown(KeyCode.J) && _canAttack)
            {
                _moveSet.Attack();
                StartCoroutine(AttackCoroutine());
            }
            
            if (Input.GetKeyDown(KeyCode.K) && _canSpecial)
            {
                _specialPointPos = _specialPoint.transform.position;
                _moveSet.Special(_specialPointPos);
                StartCoroutine(SpecialCoroutine());
                StartCoroutine(MoveTimeoutCoroutine());
            }

            if (!_defenceSkill)
                return;
            
            if (Input.GetKeyDown(KeyCode.L) && _canDefend)
            {
                _defenceSkill.Defend(_direction);
                StartCoroutine(DefendCoroutine());
                StartCoroutine(MoveTimeoutCoroutine());
            }
        }

        private IEnumerator AttackCoroutine()
        {
            _canAttack = false;
            yield return new WaitForSeconds(_attackCooldown);
            _canAttack = true;
        }
        
        private IEnumerator SpecialCoroutine()
        {
            _canSpecial = false;
            yield return new WaitForSeconds(_specialCooldown);
            _canSpecial = true;
        }
        
        private IEnumerator DefendCoroutine()
        {
            _canDefend = false;
            yield return new WaitForSeconds(_defenceCooldown);
            _canDefend = true;
        }

        private IEnumerator MoveTimeoutCoroutine()
        {
            _canMove = false;
            yield return new WaitForSeconds(moveTimeout);
            _canMove = true;
            _rigidbody.isKinematic = true;
            _rigidbody.isKinematic = false;
        }
    }
}