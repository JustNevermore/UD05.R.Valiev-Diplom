using SystemSettings;

namespace Zenject
{
    public class ProjectMonoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.BindInterfacesAndSelfTo<ProjectSetup>().AsSingle().NonLazy();
        }
    }
}