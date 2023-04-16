using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Ui.MoveHandlers
{
    public class StorageMoveHandler : MonoBehaviour
    {
        private float _endPosOffsetX;

        private Vector3 _showPos;
        private Vector3 _hidePos;

        private void Start()
        {
            _endPosOffsetX = Screen.width * 0.5f;
            var tp = transform.position;
            _showPos = tp;
            _hidePos = new Vector3(tp.x + _endPosOffsetX, tp.y, tp.z);
            
            transform.position = _hidePos;
            gameObject.SetActive(false);
        }
        
        public void OpenStorage(float duration)
        {
            transform.DOMove(_showPos, duration);
        }
        
        public async void CloseStorage(float duration)
        {
            await transform.DOMove(_hidePos, duration);
            gameObject.SetActive(false);
        }
    }
}