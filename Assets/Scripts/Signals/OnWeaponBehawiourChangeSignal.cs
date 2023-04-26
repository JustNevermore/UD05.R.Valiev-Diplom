using ItemBehaviours;
using ItemBehaviours.WeaponBehaviour;

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