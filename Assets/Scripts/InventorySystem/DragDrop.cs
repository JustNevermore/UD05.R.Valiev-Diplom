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

        private bool _dropItem;

        public Vector3 canvasScale;

        public Vector3 DragAnchor => _dragAnchor;

        //Инициализация вызывается на Start для решения проблемы с гонкой Awake
        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetDropFlag()
        {
            _dropItem = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var itemObj = eventData.pointerClick;
            var item = itemObj.GetComponent<Item>();

            if (itemObj.transform.GetComponentInParent<InventorySlot>())
            {
                if (itemObj.transform.GetComponentInParent<InventorySlot>().Type == ItemType.Consumable)
                {
                    item.UseItem();
                }
            }

            item.ShowStats();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragAnchor = transform.position;
            _canvasGroup.blocksRaycasts = false;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / canvasScale;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;

            if (!_dropItem)
            {
                transform.position = _dragAnchor;
            }

            _dropItem = false;
        }
    }
}