using System;
using Environment;
using InventorySystem;
using Player;
using SaveSystem;
using UnityEngine;
using Zenject;

namespace Managers_Controllers
{
    public class SaveSystemManager: IInitializable, IDisposable
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

        public void SaveData()
        {
            var inventoryData = _inventoryWindow.GetInventoryData();
            var stashData = _stash.GetStashData();
            var gameData = new GameSaveData(_playerStats.CurrentGold, _playerStats.ReviveShards, inventoryData.Length, stashData.Length);
            gameData.InvItemDatas = inventoryData;
            gameData.StashItemDatas = stashData;
            _saveSystem.SaveData(gameData);
        }

        public void LoadData()
        {
            var data = _saveSystem.LoadData();
            if (data == null)
                return;
            
            _playerStats.SetData(data.Gold, data.Shards);
            _inventoryWindow.SetInventoryData(data.InvItemDatas);
            _stash.SetStashData(data.StashItemDatas);
        }
    }
}