using ItemBehaviours;

namespace Signals
{
    public struct OnWeaponBehawiourChangeSignal
    {
        public WeaponBehaviour Behaviour;

        public OnWeaponBehawiourChangeSignal(WeaponBehaviour behaviour)
        {
            Behaviour = behaviour;
        }
    }
}