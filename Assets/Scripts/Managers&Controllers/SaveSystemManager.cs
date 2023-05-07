using System;
using Environment;
using InventorySystem;
using Player;
using SaveSystem;
using UnityEngine;
using Zenject;

namespace Managers_Controllers
{
    public class SaveSystemManager: IInitializable, IDisposable, ITickable
    {
        private  InventoryWindow _inventoryWindow;
        private SaveSystemJson _saveSystem;
        private  Stash _stash;
        private PlayerStats _playerStats;

        public SaveSystemManager(InventoryWindow inventoryWindow, SaveSystemJson saveSystem, Stash stash, PlayerStats playerStats)
        {
            _inventoryWindow = inventoryWindow;
            _saveSystem = saveSystem;
            _stash = stash;
            _playerStats = playerStats;
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
            var stashData = _stash.GetStashData();
            var gameData = new GameSaveData(_playerStats.CurrentGold, _playerStats.ReviveShards, inventoryData.Length, stashData.Length);
            gameData.InvItemDatas = inventoryData;
            gameData.StashItemDatas = stashData;
            _saveSystem.SaveData(gameData);
        }

        private void LoadData()
        {
            Debug.Log("Load data");
            var data = _saveSystem.LoadData();
            _playerStats.SetData(data.Gold, data.Shards);
            _inventoryWindow.SetInventoryData(data.InvItemDatas);
            _stash.SetStashData(data.StashItemDatas);
        }
    }
}