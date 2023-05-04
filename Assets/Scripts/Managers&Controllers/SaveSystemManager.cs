using System;
using InventorySystem;
using SaveSystem;
using UnityEngine;
using Zenject;

namespace Managers_Controllers
{
    public class SaveSystemManager: IInitializable, IDisposable, ITickable
    {
        private readonly InventoryWindow _inventoryWindow;
        private SaveSystemJson _saveSystem;

        public SaveSystemManager(InventoryWindow inventoryWindow, SaveSystemJson saveSystem)
        {
            _inventoryWindow = inventoryWindow;
            _saveSystem = saveSystem;
        }
        
        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                SaveData();
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                LoadData();
            }
        }

        private void SaveData()
        {
            Debug.Log("Save data");
            var inventoryData = _inventoryWindow.GetInventoryData();
            var gameData = new GameSaveData(inventoryData.Length);
            gameData.SaveItemDatas = inventoryData;
            _saveSystem.SaveData(gameData);
        }

        private void LoadData()
        {
            Debug.Log("Load data");
            _saveSystem.LoadData();
            _inventoryWindow.SetInventoryData(_saveSystem.GameData.SaveItemDatas);
        }
    }
}