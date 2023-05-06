using Ui.MoveHandlers;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Ui.Status
{
    public class HealthBar : MonoBehaviour, IPointerClickHandler
    {
        private UiController _uiController;

        [Inject]
        private void Construct(UiController uiController)
        {
            _uiController = uiController;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _uiController.ToggleInventory();
        }
    }
}