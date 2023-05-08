using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Ui
{
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI fpsText;
        private int _fpsValue;

        private void Update()
        {
            if (Time.timeScale == 0)
                return;
            
            
            var a = Mathf.RoundToInt(1f / Time.deltaTime);
            
            DOTween.To(() => _fpsValue, x => _fpsValue = x, a, 0.1f).
                OnUpdate(() => fpsText.text = _fpsValue.ToString());
        }
    }
}