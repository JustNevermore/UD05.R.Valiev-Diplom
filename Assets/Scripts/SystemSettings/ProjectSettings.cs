using UnityEngine;

namespace SystemSettings
{
    [CreateAssetMenu(fileName = "ProjectSettings", menuName = "Configs/ProjectSettings", order = 0)]
    public class ProjectSettings : ScriptableObject
    {
        [SerializeField] private int targetFPS;
        [SerializeField] private bool isMultitouch;

        public int TargetFPS => targetFPS;

        public bool IsMultitouch => isMultitouch;
    }
}