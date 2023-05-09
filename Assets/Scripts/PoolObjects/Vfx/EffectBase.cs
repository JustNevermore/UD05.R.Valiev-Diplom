using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PoolObjects.Vfx
{
    public class EffectBase : MonoBehaviour
    {
        [SerializeField] private float duration;

        private async void OnEnable()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            
            gameObject.SetActive(false);
        }
    }
}