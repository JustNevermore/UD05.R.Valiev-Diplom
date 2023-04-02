using SystemSettings;
using UnityEngine;
using Zenject;

public class ProjectMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ProjectSetup>().AsSingle().NonLazy();
    }
}