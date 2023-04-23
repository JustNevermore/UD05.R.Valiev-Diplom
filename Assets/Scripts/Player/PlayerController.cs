using System;
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
        private SignalBus _signalBus;
        private PlayerStats _playerStats;

        [Header("Variables")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotateSpeed = 500f;
        [SerializeField, Range(0f, 1f)] private float turnDeadZone = 0.4f;

        private const string HorizontalAxis = "Horizontal";
        private const string VerticalAxis = "Vertical";

        private Vector3 _input;

        private WeaponBehaviour _moveSet;

        private bool _isMoving;


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
        }

        private void Start()
        {
            _signalBus.Subscribe<OnWeaponBehawiourChange>(UpdateMoveSet);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnWeaponBehawiourChange>(UpdateMoveSet);
        }

        private void Update()
        {
            GetActionInput();
            GetMoveInput();

            _anim.Move(_isMoving);
        }

        private void FixedUpdate()
        {
            ChangeRotation();
            Move();
        }

        private void UpdateMoveSet(OnWeaponBehawiourChange signal)
        {
            _moveSet = signal.Behaviour;
            if (_moveSet != null)
            {
                _moveSet.Init(_anim.Anim);
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

        private void GetActionInput()
        {
            if (_moveSet == null)
                return;

            if (Input.GetKeyDown(KeyCode.J))
            {
                _moveSet.Attack();
            }
            
            if (Input.GetKeyDown(KeyCode.K))
            {
                _moveSet.Special();
            }
            
            if (Input.GetKeyDown(KeyCode.L))
            {
                
            }
        }
    }
}