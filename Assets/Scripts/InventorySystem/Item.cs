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
        private int _itemAmount;
        
        [SerializeField] private ItemConfig itemStats;
        private readonly Vector2 _anchorMin = new Vector2(1, 0);
        private readonly Vector2 _anchorMax = new Vector2(1, 0);
        private readonly Vector2 _pivotPos = new Vector2(1, 0);
        [SerializeField] private float fontSize = 50;
        [SerializeField] private Color textColor = Color.red;

        public ItemConfig ItemStats => itemStats;

        public int ItemAmount => _itemAmount;

        private PlayerStats _playerStats;
        private ItemStatsWindow _itemStatsWindow;

        [Inject]
        private void Construct(PlayerStats playerStats, ItemStatsWindow itemStatsWindow)
        {
            _playerStats = playerStats;
            _itemStatsWindow = itemStatsWindow;
        }

        private void Awake()
        {
            if (!GetComponent<Image>())
            {
                gameObject.AddComponent<Image>();
            }
            
            gameObject.GetComponent<Image>().sprite = ItemStats.ItemIcon;

            if (!GetComponent<DragDrop>())
            {
                gameObject.AddComponent<DragDrop>();
            }

            if (!GetComponent<CanvasGroup>())
            {
                gameObject.AddComponent<CanvasGroup>();
            }
            
            if (itemStats.IsStackable)
            {
                if (_itemAmount < 1)
                {
                    _itemAmount = 1;
                }
                
                if (!GetComponent<StackableItemMarker>())
                {
                    gameObject.AddComponent<StackableItemMarker>();
                }

                if (transform.childCount == 0)
                {
                    var amountText = Instantiate(new GameObject("AmountText"), transform);
                    var textParam = amountText.AddComponent<TextMeshProUGUI>();
                    textParam.alignment = TextAlignmentOptions.Right;
                    textParam.rectTransform.anchorMin = _anchorMin;
                    textParam.rectTransform.anchorMax = _anchorMax;
                    textParam.rectTransform.pivot = _pivotPos;
                    textParam.color = textColor;
                    textParam.fontSize = fontSize;
                    textParam.fontStyle = FontStyles.Bold;
                    textParam.text = _itemAmount.ToString();
                }
            }
        }

        public void IncreaseAmount(int amount)
        {
            _itemAmount += amount;
            GetComponentInChildren<TextMeshProUGUI>().text = _itemAmount.ToString();
        }

        public void DecreaseAmount(int amount)
        {
            _itemAmount -= amount;
            if (_itemAmount == 0)
            {
                Destroy(gameObject);
            }
            GetComponentInChildren<TextMeshProUGUI>().text = _itemAmount.ToString();
        }
        
        public void UseItem()
        {
            if (itemStats.Type == ItemType.Consumable)
            {
                _playerStats.UseConsumable(itemStats);
                DecreaseAmount(1);
            }
            else
            {
                
            }
        }

        public void ShowStats()
        {
            _itemStatsWindow.DisplayItemStats(itemStats);
        }
    }
}
