using SystemSettings;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ProjectConfigInstaller", menuName = "Installers/ProjectConfigInstaller")]
public class ProjectConfigInstaller : ScriptableObjectInstaller<ProjectConfigInstaller>
{
    [SerializeField] private ProjectSettings projectSettings;
    
    public override void InstallBindings()
    {
        Container.BindInstance(projectSettings);
    }
}