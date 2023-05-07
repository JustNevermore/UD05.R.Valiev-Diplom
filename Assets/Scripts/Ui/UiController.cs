using System;
using Cysharp.Threading.Tasks;
using InventorySystem;
using Signals;
using Ui.InventorySecondaryUi;
using Ui.MoveHandlers;
using Ui.Status;
using UnityEngine;
using Zenject;

namespace Ui
{
    public class UiController : MonoBehaviour
    {
        private SignalBus _signalBus;
        private MerchantWindow _merchantWindow;
        private ChestWindow _chestWindow;
        private ItemStatsWindow _itemStatsWindow;
        
        [SerializeField] private InventoryMoveHandler inventoryMoveHandler;
        [SerializeField] private StorageMoveHandler storageMoveHandler;
        [Space]
        [SerializeField] private float animDuration;
        
        private float _storagePanelWidth;

        private float _iconOffset = 75f;
        private float _canvasHeight;
        private float _canvasWidth;
        private float _statsHeightOffset;
        private float _statsWidthOffset;

        private bool _animBlock;
        private bool _inventoryOpen;
        private bool _storageOpen;
        

        [Inject]
        private void Construct(SignalBus signalBus, MerchantWindow merchantWindow, ChestWindow chestWindow, ItemStatsWindow itemStatsWindow)
        {
            _signalBus = signalBus;
            _merchantWindow = merchantWindow;
            _chestWindow = chestWindow;
            _itemStatsWindow = itemStatsWindow;
        }

        private void Start()
        {
            _canvasHeight = GetComponent<RectTransform>().sizeDelta.y;
            _canvasWidth = GetComponent<RectTransform>().sizeDelta.x;
            _statsHeightOffset = _itemStatsWindow.GetComponent<RectTransform>().sizeDelta.y * 0.5f;
            _statsWidthOffset = _itemStatsWindow.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
            
            _signalBus.Subscribe<OnItemClickForStatsSignal>(ShowItemStatsWindow);
            _signalBus.Subscribe<OpenMerchantSignal>(OpenMerchant);
            _signalBus.Subscribe<CloseMerchantSignal>(CloseMerchant);
            _signalBus.Subscribe<OpenChestSignal>(OpenChest);
            _signalBus.Subscribe<CloseChestSignal>(CloseChest);

            _itemStatsWindow.gameObject.SetActive(false);
            _merchantWindow.gameObject.SetActive(false);
            _chestWindow.gameObject.SetActive(false);
            _storagePanelWidth = storageMoveHandler.GetComponent<RectTransform>().sizeDelta.x;
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnItemClickForStatsSignal>(ShowItemStatsWindow);
            _signalBus.Unsubscribe<OpenMerchantSignal>(OpenMerchant);
            _signalBus.Unsubscribe<CloseMerchantSignal>(CloseMerchant);
            _signalBus.Unsubscribe<OpenChestSignal>(OpenChest);
            _signalBus.Unsubscribe<CloseChestSignal>(CloseChest);
        }

        public void ToggleInventory()
        {
            if (_animBlock)
            {
                return;
            }
            
            BlockAnimation();
            
            if (!_inventoryOpen)
            {
                inventoryMoveHandler.gameObject.SetActive(true);
                inventoryMoveHandler.OpenInventory(animDuration);
                _inventoryOpen = true;
            }
            else
            {
                inventoryMoveHandler.CloseInventory(animDuration);
                _inventoryOpen = false;

                if (_storageOpen)
                {
                    storageMoveHandler.CloseStorage(animDuration);
                    _storageOpen = false;
                }
            }
        }

        private void OpenStorage()
        {
            if (!_inventoryOpen)
            {
                if (!_storageOpen)
                {
                    BlockAnimation();
                    
                    inventoryMoveHandler.gameObject.SetActive(true);
                    inventoryMoveHandler.OpenInventory(animDuration, _storagePanelWidth);
                    _inventoryOpen = true;
                    storageMoveHandler.gameObject.SetActive(true);
                    storageMoveHandler.OpenStorage(animDuration);
                    _storageOpen = true;
                }
            }
            else
            {
                BlockAnimation();
                
                inventoryMoveHandler.PushInventory(_storagePanelWidth, animDuration);
                storageMoveHandler.gameObject.SetActive(true);
                storageMoveHandler.OpenStorage(animDuration);
                _storageOpen = true;
            }
        }

        private void CloseStorage()
        {
            if (_storageOpen)
            {
                BlockAnimation();
            
                inventoryMoveHandler.CloseInventory(animDuration, _storagePanelWidth);
                _inventoryOpen = false;
                storageMoveHandler.CloseStorage(animDuration);
                _storageOpen = false;
            }
        }

        private async void BlockAnimation()
        {
            _animBlock = true;
            await UniTask.Delay(TimeSpan.FromSeconds(animDuration));
            _animBlock = false;
        }

        private void OpenMerchant()
        {
            _merchantWindow.gameObject.SetActive(true);
            OpenStorage();
        }

        private void CloseMerchant()
        {
            _merchantWindow.gameObject.SetActive(false);
            CloseStorage();
        }

        private void OpenChest()
        {
            _chestWindow.gameObject.SetActive(true);
            OpenStorage();
        }
        
        private void CloseChest()
        {
            _chestWindow.gameObject.SetActive(false);
            CloseStorage();
        }
        
        private void ShowItemStatsWindow(OnItemClickForStatsSignal signal)
        {
            _itemStatsWindow.gameObject.SetActive(true);
            
            float xOffset;
            float yOffset;
            
            if (signal.Coord.x > _canvasWidth * 0.5f)
            {
                xOffset = signal.Coord.x - _statsWidthOffset - _iconOffset;
            }
            else
            {
                xOffset = signal.Coord.x + _statsWidthOffset + _iconOffset;
            }
            
            if (signal.Coord.y > _canvasHeight * 0.5f)
            {
                yOffset = signal.Coord.y - _statsHeightOffset + _iconOffset;
            }
            else
            {
                yOffset = signal.Coord.y + _statsHeightOffset - _iconOffset;
            }
            
            var coord = new Vector3(xOffset, yOffset, 0);
            
            _itemStatsWindow.transform.position = coord;
        }
    }
}