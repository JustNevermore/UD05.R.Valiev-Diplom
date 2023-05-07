using Cysharp.Threading.Tasks;
using DG.Tweening;
using Player;
using UnityEngine;

namespace PoolObjects
{
    public class ReviveShard : MonoBehaviour
    {
        private readonly float _jumpPower = 4f;
        private readonly float _jumpTime = 0.3f;

        private async void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<PlayerController>())
            {
                await transform.DOJump(col.GetComponent<PlayerController>().transform.position, _jumpPower, 1, _jumpTime);
                col.GetComponent<PlayerStats>().AddReviveShards(1);
                
                gameObject.SetActive(false);
            }
        }
    }
}