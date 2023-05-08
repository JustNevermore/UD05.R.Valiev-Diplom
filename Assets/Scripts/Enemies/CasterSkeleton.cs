using System;
using System.Collections;
using Cysharp.Threading.Tasks;
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
            StartCoroutine(AttackCor());
        }

        private IEnumerator AttackCor()
        {
            var dir = (Player.transform.position - transform.position).normalized;
            
            var rotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = rotation;

            yield return new WaitForSeconds(attackDelay);
            
            dir = (Player.transform.position - transform.position).normalized;
            
            var spell = Pool.GetEnemySpell();
            spell.transform.position = AttackPos.transform.position;
            spell.Init(dir, Stats.AttackDamage);
            spell.Launch();
        }

        protected override void Special()
        {
            
        }
    }
}