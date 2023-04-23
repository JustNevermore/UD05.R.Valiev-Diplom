using ItemBehaviours;

namespace Signals
{
    public struct OnWeaponBehawiourChange
    {
        public WeaponBehaviour Behaviour;

        public OnWeaponBehawiourChange(WeaponBehaviour behaviour)
        {
            Behaviour = behaviour;
        }
    }
}