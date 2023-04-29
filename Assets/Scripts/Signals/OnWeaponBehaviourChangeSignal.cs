using ItemBehaviours;
using ItemBehaviours.WeaponBehaviour;

namespace Signals
{
    public struct OnWeaponBehaviourChangeSignal
    {
        public WeaponBehaviour Behaviour;

        public OnWeaponBehaviourChangeSignal(WeaponBehaviour behaviour)
        {
            Behaviour = behaviour;
        }
    }
}