using Ui.MoveHandlers;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Ui.Status
{
    public class HealthBar : MonoBehaviour, IPointerClickHandler
    {
        private UiAnimationManager _uiAnimationManager;

        [Inject]
        private void Construct(UiAnimationManager uiAnimationManager)
        {
            _uiAnimationManager = uiAnimationManager;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _uiAnimationManager.ToggleInventory();
        }
    }
}