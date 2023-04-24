using ItemBehaviours;

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