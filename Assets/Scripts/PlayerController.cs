using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerController : MonoBehaviour
    {
        private Joystick joystick;
        private Rigidbody rb;
        private Animator anim;

        [Header("Variables")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotateSpeed = 500f;
        [SerializeField] private float attackSpeed = 2f;
        [SerializeField, Range(0f, 1f)] private float turnDeadZone = 0.4f;
        
        private float attackCooldown = 0f;

        private Vector3 input;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            joystick = FindObjectOfType<Joystick>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            GetInput();
            Attack();
            attackCooldown -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            ChangeRotation();
            Move();
        }

        private void GetInput()
        {
            input = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
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
                rb.MovePosition(transform.position + transform.forward *
                    (input.magnitude * moveSpeed * Time.deltaTime));
            }
        }

        private void Attack()
        {
            if (attackCooldown <= 0)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    anim.SetTrigger("Attack");
                    attackCooldown = 1f / attackSpeed;
                }
            }
        }
    }
}