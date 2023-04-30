using UnityEngine;

namespace Enemies
{
    public class MarksmanSkeleton : EnemyBase
    {
        protected override void AdditionalAwake()
        {
            
        }

        protected override void Attack()
        {
            var dir = (Player.transform.position - transform.position).normalized;
            var arrow = Pool.GetEnemyArrow();
            arrow.transform.position = AttackPos.transform.position;
            arrow.Init(dir, attackDamage);
            arrow.Launch();
        }

        protected override void Special()
        {
            
        }
    }
}