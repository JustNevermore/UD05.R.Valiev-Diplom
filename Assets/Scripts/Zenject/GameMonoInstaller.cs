using InventorySystem;
using Managers_Controllers;
using Player;
using SaveSystem;
using Ui;
using Ui.InventorySecondaryUi;
using UnityEngine;

namespace Zenject
{
    public class GameMonoInstaller : MonoInstaller
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private AllItemsContainer allItemsContainer;
        [SerializeField] private LootDropManager lootDropManager;
        [SerializeField] private ConsumableManager consumableManager;
        [SerializeField] private EnemySpawnManager enemySpawnManager;
        [SerializeField] private PoolManager poolManager;
        [SerializeField] private FxManager fxManager;
        [SerializeField] private InventoryWindow inventoryWindow;
        [SerializeField] private MerchantWindow merchantWindow;
        [SerializeField] private ChestWindow chestWindow;
        [SerializeField] private ItemStatsWindow itemStatsWindow;
        [SerializeField] private UiController uiController;

        public override void InstallBindings()
        {
            SignalsInstaller.Install(Container);
            Container.BindInterfacesAndSelfTo<SaveSystemManager>().AsSingle().NonLazy();
            Container.Bind<SaveSystemJson>().AsSingle().NonLazy();

        
            Container.BindInstance(playerController).AsSingle().NonLazy();
            Container.BindInstance(playerStats).AsSingle().NonLazy();
            Container.BindInstance(allItemsContainer).AsSingle().NonLazy();
            Container.BindInstance(lootDropManager).AsSingle().NonLazy();
            Container.BindInstance(consumableManager).AsSingle().NonLazy();
            Container.BindInstance(enemySpawnManager).AsSingle().NonLazy();
            Container.BindInstance(poolManager).AsSingle().NonLazy();
            Container.BindInstance(fxManager).AsSingle().NonLazy();
            Container.BindInstance(inventoryWindow).AsSingle().NonLazy();
            Container.BindInstance(merchantWindow).AsSingle().NonLazy();
            Container.BindInstance(chestWindow).AsSingle().NonLazy();
            Container.BindInstance(itemStatsWindow).AsSingle().NonLazy();
            Container.BindInstance(uiController).AsSingle().NonLazy();
        }
    }
}