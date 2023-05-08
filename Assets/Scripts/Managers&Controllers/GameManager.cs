﻿using System;
using Ads;
using Cysharp.Threading.Tasks;
using Environment;
using InventorySystem;
using Markers;
using Player;
using Signals;
using Ui.InventorySecondaryUi;
using UnityEngine;
using Zenject;

namespace Managers_Controllers
{
    public class GameManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        private DungeonGenerator _dungeonGenerator;
        private PlayerController _player;
        private PlayerStats _stats;
        private InventoryWindow _inventory;
        private SaveSystemManager _saveSystem;
        private PlayerDeathPopup _deathPopup;
        private RewardedAdsButton _rewardedButton;

        private SpawnPointMarker _spawnPoint;

        private bool _inDungeon;
        private bool _useAdRevive;
        private readonly float _reviveDamageBlockTime = 5f;

        public bool UseAdRevive => _useAdRevive;


        [Inject]
        private void Construct(SignalBus signalBus, DungeonGenerator dungeonGenerator, PlayerController playerController, PlayerStats playerStats, InventoryWindow inventoryWindow, SaveSystemManager saveSystemManager, PlayerDeathPopup deathPopup)
        {
            _signalBus = signalBus;
            _dungeonGenerator = dungeonGenerator;
            _player = playerController;
            _stats = playerStats;
            _inventory = inventoryWindow;
            _saveSystem = saveSystemManager;
            _deathPopup = deathPopup;
        }

        private void Awake()
        {
            _spawnPoint = FindObjectOfType<SpawnPointMarker>();
            _rewardedButton = GetComponent<RewardedAdsButton>();
        }

        private void Start()
        {
            _signalBus.Subscribe<OnPlayerDeathSignal>(PlayerDeath);
            _signalBus.Subscribe<CloseStashSignal>(CloseStash);
            _signalBus.Subscribe<GoToDungeonSignal>(GoToDungeon);
            _signalBus.Subscribe<ReturnToSpawnSignal>(ReturnToSpawn);
            _signalBus.Subscribe<SurrendButtonSignal>(PlayerSurrend);
            _signalBus.Subscribe<ShardReviveButtonSignal>(PlayerRevive);
            _rewardedButton.OnShowAdComplete += PlayerAdRevive;

            _player.transform.position = _spawnPoint.transform.position;
            _saveSystem.LoadData();
            
            //todo не забыть убрать
            _stats.SetData(50000, 5);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnPlayerDeathSignal>(PlayerDeath);
            _signalBus.Unsubscribe<CloseStashSignal>(CloseStash);
            _signalBus.Unsubscribe<GoToDungeonSignal>(GoToDungeon);
            _signalBus.Unsubscribe<ReturnToSpawnSignal>(ReturnToSpawn);
            _signalBus.Unsubscribe<SurrendButtonSignal>(PlayerSurrend);
            _signalBus.Unsubscribe<ShardReviveButtonSignal>(PlayerRevive);
            _rewardedButton.OnShowAdComplete -= PlayerAdRevive;
        }

        private void PlayerDeath()
        {
            Time.timeScale = 0;
            _deathPopup.gameObject.SetActive(true);
        }

        private void PlayerSurrend()
        {
            Time.timeScale = 1;
            _signalBus.Fire<DisableAllPoolObjectsSignal>();
            _inDungeon = false;
            _useAdRevive = false;
            _deathPopup.gameObject.SetActive(false);
            _player.transform.position = _spawnPoint.transform.position;
            _inventory.ResetInventory();
            _stats.ResetPlayer();
        }
        
        private async void PlayerAdRevive()
        {
            Time.timeScale = 1;
            _deathPopup.gameObject.SetActive(false);
            _useAdRevive = true;
            _stats.ResetPlayer();
            _player.PushEnemies();
            _player.DamageBox.EnableBlock();
            _player.BarrierView.SetActive(true);

            await UniTask.Delay(TimeSpan.FromSeconds(_reviveDamageBlockTime));
            
            _player.DamageBox.DisableBlock();
            _player.BarrierView.SetActive(false);
        }

        private async void PlayerRevive()
        {
            Time.timeScale = 1;
            _deathPopup.gameObject.SetActive(false);
            _stats.RemoveReviveShards(1);
            _stats.ResetPlayer();
            _player.PushEnemies();
            _player.DamageBox.EnableBlock();
            _player.BarrierView.SetActive(true);

            await UniTask.Delay(TimeSpan.FromSeconds(_reviveDamageBlockTime));
            
            _player.DamageBox.DisableBlock();
            _player.BarrierView.SetActive(false);
        }

        private void CloseStash()
        {
            _saveSystem.SaveData();
        }

        private void GoToDungeon()
        {
            _dungeonGenerator.GenerateDungeon();
            _inDungeon = true;
        }

        private void ReturnToSpawn()
        {
            _saveSystem.SaveData();
            _dungeonGenerator.DestroyDungeon();
            _inDungeon = false;
            _useAdRevive = false;
        }

        private void OnApplicationQuit()
        {
            if (_inDungeon) _inventory.ResetInventory();
            
            _saveSystem.SaveData();
            
        }
    }
}