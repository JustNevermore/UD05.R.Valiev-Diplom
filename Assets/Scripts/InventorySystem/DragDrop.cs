using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace InventorySystem
{
    public class DragDrop : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private Vector3 _dragAnchor;

        public Vector3 DragAnchor => _dragAnchor;

        //Инициализация вызывается на Start для решения проблемы с гонкой Awake
        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var item = eventData.pointerDrag.GetComponent<Item>();
            item.ShowStats();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragAnchor = transform.position;
            _canvasGroup.blocksRaycasts = false;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
        }
    }
}