using System;
using InventorySystem;
using Managers_Controllers;
using Player;
using SaveSystem;
using Ui;
using Ui.InventorySecondaryUi;
using Ui.MoveHandlers;
using UnityEngine;
using Zenject;

public class GameMonoInstaller : MonoInstaller
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private AllItemsContainer allItemsContainer;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private InventoryWindow inventoryWindow;
    [SerializeField] private ItemStatsWindow itemStatsWindow;
    [SerializeField] private UiAnimationManager uiAnimationManager;
    [SerializeField] private InventoryMoveHandler inventoryMoveHandler;
    [SerializeField] private StorageMoveHandler storageMoveHandler;

    public override void InstallBindings()
    {
        SignalsInstaller.Install(Container);
        Container.BindInterfacesAndSelfTo<SaveSystemManager>().AsSingle().NonLazy();
        Container.Bind<SaveSystemJson>().AsSingle().NonLazy();
        
        Container.BindInstance(playerController).AsSingle().NonLazy();
        Container.BindInstance(playerStats).AsSingle().NonLazy();
        Container.BindInstance(allItemsContainer).AsSingle().NonLazy();
        Container.BindInstance(inventoryController).AsSingle().NonLazy();
        Container.BindInstance(inventoryWindow).AsSingle().NonLazy();
        Container.BindInstance(itemStatsWindow).AsSingle().NonLazy();
        Container.BindInstance(uiAnimationManager).AsSingle().NonLazy();
        Container.BindInstance(inventoryMoveHandler).AsSingle().NonLazy();
        Container.BindInstance(storageMoveHandler).AsSingle().NonLazy();
    }
}