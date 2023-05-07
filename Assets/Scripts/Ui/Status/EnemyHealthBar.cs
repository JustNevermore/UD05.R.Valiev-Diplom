using System;
using DG.Tweening;
using Enemies;
using Markers;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Status
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Image health;
        private EnemyStats _stats;
        private LookAtMarker _lookTarget;

        private void Awake()
        {
            _stats = GetComponentInParent<EnemyStats>();
            _lookTarget = FindObjectOfType<LookAtMarker>();
            GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();
        }

        private void Start()
        {
            _stats.OnChangeHpAmount += UpdateBar;
        }

        private void OnDestroy()
        {
            _stats.OnChangeHpAmount -= UpdateBar;
        }

        private void UpdateBar(float amount)
        {
            health.fillAmount = amount;
        }

        private void LateUpdate()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - _lookTarget.transform.position);
        }
    }
}