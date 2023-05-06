using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        protected readonly float SlowDuration = 2f;
        protected readonly float SlowValue = 2f;
    }
}