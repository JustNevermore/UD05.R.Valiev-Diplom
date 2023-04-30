using System;
using Markers;
using UnityEngine;

namespace Enemies
{
    public class StatsSystem : MonoBehaviour
    {
        private HurtBox _hurtBox;

        private void Awake()
        {
            _hurtBox = GetComponent<HurtBox>();
        }

        private void Start()
        {
            _hurtBox.OnGetDamage += IncomingDamage;
        }

        private void OnDestroy()
        {
            _hurtBox.OnGetDamage -= IncomingDamage;
        }
        
        private void IncomingDamage(float damage)
        {
            Debug.Log($"Enemy get {damage} damage");
        }
    }
}