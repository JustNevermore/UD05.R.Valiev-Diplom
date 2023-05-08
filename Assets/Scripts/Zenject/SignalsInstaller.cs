using Signals;

namespace Zenject
{
    public class SignalsInstaller : Installer<SignalsInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<OnChangeWeaponTypeSignal>();
            Container.DeclareSignal<OnWeaponBehaviourChangeSignal>();
            Container.DeclareSignal<OnDefenceSkillChangeSignal>();
            Container.DeclareSignal<OnEnemyDeathSignal>();
            Container.DeclareSignal<OnBossDeathSignal>();
            Container.DeclareSignal<OpenMerchantSignal>();
            Container.DeclareSignal<CloseMerchantSignal>();
            Container.DeclareSignal<OpenChestSignal>();
            Container.DeclareSignal<CloseChestSignal>();
            Container.DeclareSignal<OnItemClickForStatsSignal>();
            Container.DeclareSignal<CloseStashSignal>();
            Container.DeclareSignal<OnPlayerDeathSignal>();
            Container.DeclareSignal<ReturnToSpawnSignal>();
            Container.DeclareSignal<GoToDungeonSignal>();
            Container.DeclareSignal<SurrendButtonSignal>();
            Container.DeclareSignal<AdReviveButtonSignal>();
            Container.DeclareSignal<ShardReviveButtonSignal>();
        }
    }
}