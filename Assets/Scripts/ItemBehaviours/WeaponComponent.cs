using Player;
using UnityEngine;

namespace ItemBehaviours
{
    public abstract class WeaponComponent : MonoBehaviour
    {
        public abstract void Init(PlayerStats stats);
    }
}