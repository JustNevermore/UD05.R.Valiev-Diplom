using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerController : MonoBehaviour
    {
        private Joystick joystick;
        private Rigidbody rb;

        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotateSpeed = 500f;

        private Vector3 input;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            joystick = FindObjectOfType<Joystick>();
        }

        private void Update()
        {
            GetInput();
            ChangeRotation();
        }

        private void FixedUpdate()
        {
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
            rb.MovePosition(transform.position + transform.forward * (input.magnitude * moveSpeed * Time.deltaTime));
        }
    }
}