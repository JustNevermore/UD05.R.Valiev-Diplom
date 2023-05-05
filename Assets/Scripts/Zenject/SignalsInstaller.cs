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
            Container.DeclareSignal<OpenMerchantSignal>();
            Container.DeclareSignal<CloseMerchantSignal>();
            Container.DeclareSignal<OpenChestSignal>();
            Container.DeclareSignal<CloseChestSignal>();
            Container.DeclareSignal<OnItemClickForStatsSignal>();
        }
    }
}