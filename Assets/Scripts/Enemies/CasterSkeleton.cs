using UnityEngine;

namespace Enemies
{
    public class CasterSkeleton : EnemyBase
    {
        protected override void AdditionalAwake()
        {
            
        }

        protected override void Attack()
        {
            var dir = (Player.transform.position - transform.position).normalized;

            var spell = Pool.GetEnemySpell();
            spell.transform.position = AttackPos.transform.position;
            spell.Init(dir, attackDamage);
            spell.Launch();
        }

        protected override void Special()
        {
            
        }
    }
}