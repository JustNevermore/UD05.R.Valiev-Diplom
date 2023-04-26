using ItemBehaviours;
using ItemBehaviours.NecklaceBehaviour;

namespace Signals
{
    public struct OnDefenceSkillChangeSignal
    {
        public NecklaceBehaviour DefenceSkill;

        public OnDefenceSkillChangeSignal(NecklaceBehaviour defenceSkill)
        {
            DefenceSkill = defenceSkill;
        }
    }
}