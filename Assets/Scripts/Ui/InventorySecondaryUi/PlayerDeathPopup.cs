using System;
using Managers_Controllers;
using Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ui.InventorySecondaryUi
{
    public class PlayerDeathPopup : MonoBehaviour
    {
        private GameManager _gameManager;
        private PlayerStats _playerStats;

        [SerializeField] private Button surrendButton;
        [SerializeField] private Button adReviveButton;
        [SerializeField] private Button shardReviveButton;

        
        [Inject]
        private void Construct(GameManager gameManager, PlayerStats playerStats)
        {
            _gameManager = gameManager;
            _playerStats = playerStats;
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            adReviveButton.gameObject.SetActive(true);
            shardReviveButton.gameObject.SetActive(true);
            
            if (_playerStats.ReviveShards == 0)
            {
                shardReviveButton.gameObject.SetActive(false);
            }

            if (_gameManager.UseAdRevive)
            {
                adReviveButton.gameObject.SetActive(false);
            }
        }
    }
}