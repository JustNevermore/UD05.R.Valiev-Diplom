using System;
using Player;
using TMPro;
using Ui.InventorySecondaryUi;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace InventorySystem
{
    public class Item : MonoBehaviour
    {
        private ItemData _itemData;
        private ItemConfig _itemConfig;
        private readonly Vector2 _anchorMin = new Vector2(1, 0);
        private readonly Vector2 _anchorMax = new Vector2(1, 0);
        private readonly Vector2 _pivotPos = new Vector2(1, 0);
        [SerializeField] private float fontSize = 50;
        [SerializeField] private Color textColor = Color.red;

        public ItemData Data => _itemData;

        private PlayerStats _playerStats;
        private AllItemsContainer _allItemsContainer;
        private ItemStatsWindow _itemStatsWindow;

        private TextMeshProUGUI _amountText;

        [Inject]
        private void Construct(PlayerStats playerStats, AllItemsContainer allItemsContainer, ItemStatsWindow itemStatsWindow)
        {
            _playerStats = playerStats;
            _allItemsContainer = allItemsContainer;
            _itemStatsWindow = itemStatsWindow;
        }

        public void Init(ItemData data)
        {
            _itemData = data;
            _itemConfig = _allItemsContainer.GetConfigById(_itemData.ItemId);

            _itemData.AmountChange += UpdateAmountCounter;

            if (!GetComponent<Image>())
            {
                gameObject.AddComponent<Image>();
            }
            
            gameObject.GetComponent<Image>().sprite = _itemConfig.ItemIcon;

            if (!GetComponent<DragDrop>())
            {
                gameObject.AddComponent<DragDrop>();
            }

            if (!GetComponent<CanvasGroup>())
            {
                gameObject.AddComponent<CanvasGroup>();
            }
            
            if (_itemConfig.IsStackable)
            {
                if (transform.childCount == 0)
                {
                    var amountObj = new GameObject("AmountText");
                    amountObj.transform.SetParent(transform);
                    _amountText = amountObj.AddComponent<TextMeshProUGUI>();
                    _amountText.alignment = TextAlignmentOptions.Right;
                    _amountText.rectTransform.anchorMin = _anchorMin;
                    _amountText.rectTransform.anchorMax = _anchorMax;
                    _amountText.rectTransform.pivot = _pivotPos;
                    _amountText.color = textColor;
                    _amountText.fontSize = fontSize;
                    _amountText.fontStyle = FontStyles.Bold;
                    _amountText.text = _itemData.ItemAmount.ToString();
                }
            }
        }

        private void UpdateAmountCounter()
        {
            _amountText.text = _itemData.ItemAmount.ToString();
        }

        public void UseItem()
        {
            _playerStats.UseConsumable(_itemConfig);
        }

        public void ShowStats()
        {
            _itemStatsWindow.DisplayItemStats(_itemConfig);
        }

        private void OnDestroy()
        {
            _itemData.AmountChange -= UpdateAmountCounter;
        }
    }
}
