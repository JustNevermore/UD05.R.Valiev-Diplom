using System;
using InventorySystem;
using SaveSystem;
using UnityEngine;
using Zenject;

namespace Managers_Controllers
{
    public class SaveSystemManager: IInitializable, IDisposable, ITickable
    {
        private SaveSystemJson _saveSystem;
        private InventoryController _inventoryController;

        public SaveSystemManager(SaveSystemJson saveSystem, InventoryController inventoryController)
        {
            _saveSystem = saveSystem;
            _inventoryController = inventoryController;
        }
        
        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                SaveData();
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                LoadData();
            }
        }

        private void SaveData()
        {
            Debug.Log("Save data");
            var data = _inventoryController.GetInventoryData();
            var gameData = new GameSaveData(data.Length);
            gameData.SaveItemDatas = data;
            _saveSystem.SaveData(gameData);
        }

        private void LoadData()
        {
            Debug.Log("Load data");
            _saveSystem.LoadData();
            _inventoryController.SetInventoryData(_saveSystem.GameData.SaveItemDatas);
        }
    }
}