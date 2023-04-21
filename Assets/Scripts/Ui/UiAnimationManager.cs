﻿using System;
using Cysharp.Threading.Tasks;
using Ui.MoveHandlers;
using UnityEngine;
using Zenject;

namespace Ui
{
    public class UiAnimationManager : MonoBehaviour
    {
        private InventoryMoveHandler _inventoryMoveHandler;
        private StorageMoveHandler _storageMoveHandler;
        
        [SerializeField] private float animDuration;
        private float _storagePanelWidth;

        private bool _animBlock = false;
        private bool _inventoryOpen = false;
        private bool _merchantOpen = false;

        [Inject]
        private void Construct(InventoryMoveHandler inventoryMoveHandler, StorageMoveHandler storageMoveHandler)
        {
            _inventoryMoveHandler = inventoryMoveHandler;
            _storageMoveHandler = storageMoveHandler;
        }

        private void Start()
        {
            _storagePanelWidth = _storageMoveHandler.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        }

        public void ToggleInventory()
        {
            if (_animBlock || _merchantOpen)
            {
                return;
            }
            
            BlockAnimation();
            
            if (!_inventoryOpen)
            {
                _inventoryMoveHandler.gameObject.SetActive(true);
                _inventoryMoveHandler.OpenInventory(animDuration);
                _inventoryOpen = true;
            }
            else
            {
                _inventoryMoveHandler.CloseInventory(animDuration);
                _inventoryOpen = false;
            }
        }

        public void ToggleStorage()
        {
            if (_animBlock)
            {
                return;
            }
            
            BlockAnimation();
            
            if (!_inventoryOpen)
            {
                if (!_merchantOpen)
                {
                    _inventoryMoveHandler.gameObject.SetActive(true);
                    _inventoryMoveHandler.OpenInventory(animDuration, _storagePanelWidth);
                    _inventoryOpen = true;
                    _storageMoveHandler.gameObject.SetActive(true);
                    _storageMoveHandler.OpenStorage(animDuration);
                    _merchantOpen = true;
                }
            }
            else
            {
                if (!_merchantOpen)
                {
                    _inventoryMoveHandler.PushInventory(_storagePanelWidth, animDuration);
                    _storageMoveHandler.gameObject.SetActive(true);
                    _storageMoveHandler.OpenStorage(animDuration);
                    _merchantOpen = true;
                }
                else
                {
                    _inventoryMoveHandler.CloseInventory(animDuration, _storagePanelWidth);
                    _inventoryOpen = false;
                    _storageMoveHandler.CloseStorage(animDuration);
                    _merchantOpen = false;
                }
            }
        }

        private async void BlockAnimation()
        {
            _animBlock = true;
            await UniTask.Delay(TimeSpan.FromSeconds(animDuration));
            _animBlock = false;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                ToggleStorage();
            }
        }
    }
}