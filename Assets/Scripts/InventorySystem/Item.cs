using System;
using Managers_Controllers;
using Player;
using Signals;
using TMPro;
using Ui;
using Ui.InventorySecondaryUi;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace InventorySystem
{
    public class Item : MonoBehaviour
    {
        private SignalBus _signalBus;
        private ConsumableManager _consumableManager;
        private AllItemsContainer _allItemsContainer;
        private ItemStatsWindow _itemStatsWindow;
        private InventoryWindow _inventoryWindow;
        private UiController _uiController;
        
        private ItemData _itemData;
        private ItemConfig _itemConfig;
        private readonly Vector2 _textAnchorMin = new Vector2(0.95f, 0.05f);
        private readonly Vector2 _textAnchorMax = new Vector2(0.9f, 0.1f);
        private readonly Vector2 _pivotPos = new Vector2(1, 0);
        private readonly Vector2 _borderAnchorMin = new Vector2(0, 0);
        private readonly Vector2 _borderAnchorMax = new Vector2(1, 1);
        private readonly float _borderOffset = 100f;

        public ItemData Data => _itemData;

        private TextMeshProUGUI _amountText;

        [Inject]
        private void Construct(SignalBus signalBus, ConsumableManager consumableManager, AllItemsContainer allItemsContainer, ItemStatsWindow itemStatsWindow, InventoryWindow inventoryWindow, UiController uiController)
        {
            _signalBus = signalBus;
            _consumableManager = consumableManager;
            _allItemsContainer = allItemsContainer;
            _itemStatsWindow = itemStatsWindow;
            _inventoryWindow = inventoryWindow;
            _uiController = uiController;
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
                gameObject.AddComponent<DragDrop>().canvasScale = _uiController.transform.localScale;
            }

            if (!GetComponent<CanvasGroup>())
            {
                gameObject.AddComponent<CanvasGroup>();
            }
            
            if (transform.GetComponentsInChildren<Image>().Length <= 1)
            {
                var borderObj = new GameObject("Border");
                borderObj.transform.SetParent(transform);
                var borderImage = borderObj.AddComponent<Image>();
                borderImage.sprite = _allItemsContainer.BorderImage;
                borderImage.rectTransform.anchorMax = _borderAnchorMax;
                borderImage.rectTransform.anchorMin = _borderAnchorMin;
                borderImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _borderOffset);
                borderImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _borderOffset);
                
                //todo добавить изменение цвета рамки в соответствии с редкостью предмета
                // добавить бэкграунд ячейки, зависящий от типа предмета
            }

            if (_itemConfig.IsStackable)
            {
                if (transform.GetComponentsInChildren<TextMeshProUGUI>().Length == 0)
                {
                    var amountObj = new GameObject("AmountText");
                    amountObj.transform.SetParent(transform);
                    _amountText = amountObj.AddComponent<TextMeshProUGUI>();
                    _amountText.alignment = TextAlignmentOptions.Right;
                    _amountText.rectTransform.anchorMin = _textAnchorMin;
                    _amountText.rectTransform.anchorMax = _textAnchorMax;
                    _amountText.rectTransform.pivot = _pivotPos;
                    _amountText.color = _allItemsContainer.TextColor;
                    _amountText.fontSize = _allItemsContainer.FontSize;
                    _amountText.fontStyle = FontStyles.Bold;
                    _amountText.text = _itemData.ItemAmount.ToString();
                }
            }
        }

        private void UpdateAmountCounter()
        {
            _amountText.text = _itemData.ItemAmount.ToString();

            if (_itemData.ItemAmount == 0)
            {
                _inventoryWindow.RemoveItemFromInventory(_itemData);
                Destroy(gameObject);
            }
        }

        public void UseItem()
        {
            _consumableManager.UseConsumable(_itemConfig, this);
        }

        public void ShowStats()
        {
            if (_itemData.InSlot is EquipmentSlot.Cons1 or EquipmentSlot.Cons2 or EquipmentSlot.Cons3 or EquipmentSlot.Cons4)
                return;
            
            var coord = new Vector2(transform.position.x, transform.position.y);
            _signalBus.Fire(new OnItemClickForStatsSignal(coord));
            _itemStatsWindow.DisplayItemStats(_itemConfig);
        }

        private void OnDestroy()
        {
            _itemData.AmountChange -= UpdateAmountCounter;
        }
    }
}
