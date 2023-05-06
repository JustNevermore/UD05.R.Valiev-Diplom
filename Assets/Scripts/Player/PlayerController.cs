using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using ItemBehaviours.NecklaceBehaviour;
using ItemBehaviours.WeaponBehaviour;
using Managers_Controllers;
using Markers;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private PlayerAnimationController _anim;
        private HurtBox _hurtBox;

        private AttackPosMarker _attackPos;
        private ZeroPosMarker _zeroPos;
        private RightPosMarker _rightPos;
        
        private SignalBus _signalBus;
        private PlayerStats _playerStats;
        private PoolManager _poolManager;

        [Header("Variables")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotateSpeed = 500f;
        [SerializeField, Range(0f, 1f)] private float turnDeadZone = 0.5f;
        [SerializeField] private GameObject barrierViewPrefab;

        private GameObject _barrierView;

        private const string HorizontalAxis = "Horizontal";
        private const string VerticalAxis = "Vertical";
        private const string AttackButton = "Attack";
        private const string SpecialButton = "Special";
        private const string DefenceButton = "Defence";

        private Vector3 _input;

        private WeaponBehaviour _moveSet;
        private NecklaceBehaviour _defenceSkill;
        
        private float _attackTimeout = 0.3f;
        private float _specialTimeout;
        private float _defenceTimeout;

        private float _attackCooldown;
        private float _specialCooldown;
        private float _defenceCooldown;

        private bool _isMoving;
        private bool _canMove;
        private bool _canAttack;
        private bool _canSpecial;
        private bool _canDefend;
        private bool _attackCd;
        private bool _specialCd;
        private bool _defenceCd;

        public Rigidbody Rigbody => _rigidbody;
        public HurtBox DamageBox => _hurtBox;
        public AttackPosMarker AttackPos => _attackPos;
        public ZeroPosMarker ZeroPos => _zeroPos;
        public RightPosMarker RightPos => _rightPos;
        public GameObject BarrierView => _barrierView;


        [Inject]
        private void Construct(SignalBus signalBus , PlayerStats playerStats, PoolManager poolManager)
        {
            _signalBus = signalBus;
            _playerStats = playerStats;
            _poolManager = poolManager;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _anim = GetComponent<PlayerAnimationController>();
            _hurtBox = GetComponent<HurtBox>();
            _attackPos = GetComponentInChildren<AttackPosMarker>();
            _zeroPos = GetComponentInChildren<ZeroPosMarker>();
            _rightPos = GetComponentInChildren<RightPosMarker>();

            _barrierView = Instantiate(barrierViewPrefab, transform);
            _barrierView.SetActive(false);
        }

        private void Start()
        {
            _canMove = true;
            _canAttack = true;
            _canSpecial = true;
            _canDefend = true;
            
            _signalBus.Subscribe<OnWeaponBehaviourChangeSignal>(UpdateMoveSet);
            _signalBus.Subscribe<OnDefenceSkillChangeSignal>(UpdateDefenceSkill);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnWeaponBehaviourChangeSignal>(UpdateMoveSet);
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

        private void UpdateMoveSet(OnWeaponBehaviourChangeSignal signal)
        {
            _moveSet = signal.Behaviour;
            if (_moveSet != null)
            {
                _moveSet.Init(this, _playerStats, _anim.Anim, _poolManager);
                _attackCooldown = _moveSet.attackCooldown;
                _specialCooldown = _moveSet.specialCooldown;
                _specialTimeout = _moveSet.animTimeout;
            }
        }

        private void UpdateDefenceSkill(OnDefenceSkillChangeSignal signal)
        {
            _defenceSkill = signal.DefenceSkill;
            if (_defenceSkill != null)
            {
                _defenceSkill.Init(this, _anim.Anim);
                _defenceCooldown = _defenceSkill.defenceCooldown;
                _defenceTimeout = _defenceSkill.animTimeout;
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

        private async void GetActionInput()
        {
            if (_moveSet)
            {
                if (Input.GetKeyDown(KeyCode.J) || SimpleInput.GetButtonDown(AttackButton))
                {
                    if (_canAttack && !_attackCd)
                    {
                        _moveSet.Attack();
                        StartCoroutine(AttackCoroutine());
                        _canSpecial = false;
                        _canDefend = false;

                        await UniTask.Delay(TimeSpan.FromSeconds(_attackTimeout));

                        _canSpecial = true;
                        _canDefend = true;
                    }
                }

                if (Input.GetKeyDown(KeyCode.K) || SimpleInput.GetButtonDown(SpecialButton))
                {
                    if (_canSpecial && !_specialCd)
                    {
                        _moveSet.Special();
                        StartCoroutine(SpecialCoroutine());
                        StartCoroutine(MoveTimeoutCoroutine(_specialTimeout));
                        _canAttack = false;
                        _canDefend = false;

                        await UniTask.Delay(TimeSpan.FromSeconds(_specialTimeout));

                        _canAttack = true;
                        _canDefend = true;
                    }
                }
            }

            if (_defenceSkill)
            {
                if (Input.GetKeyDown(KeyCode.L) || SimpleInput.GetButtonDown(DefenceButton))
                {
                    if (_canDefend && !_defenceCd)
                    {
                        _defenceSkill.Defend();
                        StartCoroutine(DefendCoroutine());
                        StartCoroutine(MoveTimeoutCoroutine(_defenceTimeout));
                        _canAttack = false;
                        _canSpecial = false;

                        await UniTask.Delay(TimeSpan.FromSeconds(_defenceTimeout));

                        _canAttack = true;
                        _canSpecial = true;
                    }
                }
            }
        }

        private IEnumerator AttackCoroutine()
        {
            _attackCd = true;
            yield return new WaitForSeconds(_attackCooldown);
            _attackCd = false;
        }
        
        private IEnumerator SpecialCoroutine()
        {
            _specialCd = true;
            yield return new WaitForSeconds(_specialCooldown);
            _specialCd = false;
        }
        
        private IEnumerator DefendCoroutine()
        {
            _defenceCd = true;
            yield return new WaitForSeconds(_defenceCooldown);
            _defenceCd = false;
        }

        private IEnumerator MoveTimeoutCoroutine(float delay)
        {
            _canMove = false;
            yield return new WaitForSeconds(delay);
            _canMove = true;
        }
    }
}