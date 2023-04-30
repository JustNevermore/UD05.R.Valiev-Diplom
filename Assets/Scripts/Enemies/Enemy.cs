using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        protected readonly float SlowDuration = 1f;
        protected readonly float SlowValue = 0.5f;
    }
}