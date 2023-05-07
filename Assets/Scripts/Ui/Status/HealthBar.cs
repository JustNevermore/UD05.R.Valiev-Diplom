using System;
using Player;
using TMPro;
using Ui.MoveHandlers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Ui.Status
{
    public class HealthBar : MonoBehaviour, IPointerClickHandler
    {
        private UiController _uiController;
        private PlayerStats _stats;

        [SerializeField] private Image healthImage;
        [SerializeField] private Image manaImage;

        [SerializeField] private TextMeshProUGUI goldAmount;
        [SerializeField] private TextMeshProUGUI shardsAmount;

        [Inject]
        private void Construct(UiController uiController, PlayerStats playerStats)
        {
            _uiController = uiController;
            _stats = playerStats;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _uiController.ToggleInventory();
        }

        private void Awake()
        {
            _stats.OnHpValueChange += HealthUpdate;
            _stats.OnMpValueChange += ManaUpdate;
            _stats.OnGoldAmountChange += GoldUpdate;
            _stats.OnShardAmountChange += ShardUpdate;
        }

        private void OnDestroy()
        {
            _stats.OnHpValueChange -= HealthUpdate;
            _stats.OnMpValueChange -= ManaUpdate;
            _stats.OnGoldAmountChange -= GoldUpdate;
            _stats.OnShardAmountChange -= ShardUpdate;
        }

        private void HealthUpdate(float amount)
        {
            healthImage.fillAmount = amount;
        }

        private void ManaUpdate(float amount)
        {
            manaImage.fillAmount = amount;
        }

        private void GoldUpdate(int amount)
        {
            goldAmount.text = amount.ToString();
        }

        private void ShardUpdate(int amount)
        {
            shardsAmount.text = amount.ToString();
        }
    }
}