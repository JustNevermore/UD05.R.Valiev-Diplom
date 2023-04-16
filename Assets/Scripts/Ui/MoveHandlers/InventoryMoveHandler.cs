using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Ui.MoveHandlers
{
    public class InventoryMoveHandler : MonoBehaviour
    {
        private float _endPosOffsetY;
        
        private Vector3 _showPos;
        private Vector3 _hidePos;
        private Vector3 _pushPos;

        private void Start()
        {
            _endPosOffsetY = Screen.height;
            var tp = transform.position;
            _showPos = tp;
            _hidePos = new Vector3(tp.x, tp.y - _endPosOffsetY, tp.z);
            
            transform.position = _hidePos;
            gameObject.SetActive(false);
        }

        public void OpenInventory(float duration)
        {
            transform.DOMove(_showPos, duration);
        }
        
        public void OpenInventory(float duration, float distance)
        {
            transform.DOMove(new Vector3(_showPos.x - distance, _showPos.y, _showPos.z), duration);
        }
        
        public async void CloseInventory(float duration)
        {
            await transform.DOMove(_hidePos, duration);
            gameObject.SetActive(false);
        }
        
        public async void CloseInventory(float duration, float distance)
        {
            await transform.DOMove(_hidePos, duration);
            gameObject.SetActive(false);
        }
        
        public async void PushInventory(float distance, float duration)
        {
            await transform.DOMove(new Vector3(_showPos.x - distance, _showPos.y, _showPos.z), duration);
        }
    }
}