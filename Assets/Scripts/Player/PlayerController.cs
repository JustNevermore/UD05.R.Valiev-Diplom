﻿using UnityEngine;
using Weapons;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private PlayerAnimationController _anim;
        private Weapon _weapon;

        [Header("Variables")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotateSpeed = 500f;
        [SerializeField] private float attackSpeed = 2f;
        [SerializeField, Range(0f, 1f)] private float turnDeadZone = 0.4f;

        private const string HorizontalAxis = "Horizontal";
        private const string VerticalAxis = "Vertical";

        private float attackCooldown = 0f;

        private Vector3 input;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _anim = GetComponent<PlayerAnimationController>();
            _weapon = GetComponentInChildren<Weapon>();
        }

        private void Update()
        {
            GetInput();
            Attack();
            AltAttack();
            attackCooldown -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            ChangeRotation();
            Move();
        }

        private void GetInput()
        {
            input = new Vector3(SimpleInput.GetAxis(HorizontalAxis), 0, SimpleInput.GetAxis(VerticalAxis));
        }

        private void ChangeRotation()
        {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
            var isoInput = matrix.MultiplyPoint3x4(input);

            if (input != Vector3.zero)
            {
                var lookOffset = (transform.position + isoInput) - transform.position;
                var rotation = Quaternion.LookRotation(lookOffset, Vector3.up);

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation, rotation, rotateSpeed * Time.deltaTime);
            }
        }

        private void Move()
        {
            if (input.magnitude > turnDeadZone)
            {
                _rigidbody.MovePosition(transform.position + transform.forward *
                    (input.magnitude * moveSpeed * Time.deltaTime));
            }
        }

        private void Attack()
        {
            if (attackCooldown <= 0)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    _weapon.DoAttack();
                    _anim.DoAttack();
                    attackCooldown = 1f / attackSpeed;
                }
            }
        }

        private void AltAttack()
        {
            if (attackCooldown <= 0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _anim.DoAltAttack();
                }
            }
        }

        private void Special()
        {
            
        }
    }
}