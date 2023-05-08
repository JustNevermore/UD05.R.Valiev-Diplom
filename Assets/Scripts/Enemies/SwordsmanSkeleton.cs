using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Markers;
using UnityEngine;

namespace Enemies
{
    public class SwordsmanSkeleton : EnemyBase
    {
        [SerializeField] private float attackRadius;

        private int _attackHit;
        private Collider[] _colliders = new Collider[1];

        protected override void AdditionalAwake()
        {
            
        }

        protected override void Attack()
        {
            StartCoroutine(AttackCor());
        }

        private IEnumerator AttackCor()
        {
            yield return new WaitForSeconds(attackDelay);
            
            var attackPoint = AttackPos.transform.position;
            
            _attackHit = Physics.OverlapSphereNonAlloc(attackPoint, attackRadius, _colliders, playerLayer);
            
            if (_attackHit > 0)
            {
                for (int i = 0; i < _attackHit; i++)
                {
                    _colliders[i].GetComponent<HurtBox>().GetDamage(Stats.AttackDamage);
                }
            }
        }

        protected override void Special()
        {
            
        }
    }
}