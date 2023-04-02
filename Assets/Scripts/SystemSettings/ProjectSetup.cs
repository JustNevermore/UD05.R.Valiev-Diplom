using UnityEngine;
using Zenject;

namespace SystemSettings
{
    public class ProjectSetup : IInitializable
    {
        private readonly ProjectSettings _settings;

        public ProjectSetup(ProjectSettings settings)
        {
            _settings = settings;
        }

        public void Initialize()
        {
            Application.targetFrameRate = _settings.TargetFPS;
            Input.multiTouchEnabled = _settings.IsMultitouch;
        }
    }
}