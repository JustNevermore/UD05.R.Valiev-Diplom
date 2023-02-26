using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerController : MonoBehaviour
    {
        private Joystick joystick;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotateSpeed = 2f;

        private void Awake()
        {
            joystick = FindObjectOfType<Joystick>();
        }

        private void Update()
        {
            if (joystick.Direction != Vector2.zero)
            {
                float mag = joystick.Direction.magnitude;

                Vector3 dir = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotateSpeed * Time.deltaTime); Quaternion.LookRotation(dir);

                transform.position += transform.forward * (moveSpeed * Time.deltaTime * mag);
            }
        }
    }
}