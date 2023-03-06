using Markers;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private HurtBox _hurtBox;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _hurtBox = GetComponent<HurtBox>();
        }

        private void Start()
        {
            _hurtBox.IsDamaged += IncomingDamage;
        }

        private void OnDestroy()
        {
            _hurtBox.IsDamaged -= IncomingDamage;
        }

        private void IncomingDamage(float damage)
        {
            Debug.Log($"Enemy get {damage} damage");
            _rigidbody.AddForce(Vector3.up * 3, ForceMode.Impulse);
        }
    }
}