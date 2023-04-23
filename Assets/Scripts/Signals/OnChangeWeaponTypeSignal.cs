using ItemBehaviours;

namespace Signals
{
    public struct OnChangeWeaponTypeSignal
    {
        public readonly WeaponType Type;

        public OnChangeWeaponTypeSignal(WeaponType type)
        {
            Type = type;
        }
    }
}