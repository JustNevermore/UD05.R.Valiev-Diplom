using Cysharp.Threading.Tasks;
using DG.Tweening;
using Player;
using UnityEngine;

namespace PoolObjects
{
    public class CoinPouch : MonoBehaviour
    {
        private readonly float _jumpPower = 4f;
        private readonly float _jumpTime = 0.3f;

        private int _amount;
        
        private bool _isActive;

        public void SetAmount(int value)
        {
            _amount = value;

            _isActive = true;
        }
        
        private async void OnTriggerEnter(Collider col)
        {
            if (!_isActive)
                return;
            
            if (col.GetComponent<PlayerController>())
            {
                await transform.DOJump(col.GetComponent<PlayerController>().transform.position, _jumpPower, 1, _jumpTime);
                col.GetComponent<PlayerStats>().IncreaseGold(_amount);

                _isActive = false;
                gameObject.SetActive(false);
            }
        }
    }
}