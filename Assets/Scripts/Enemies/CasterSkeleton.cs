﻿using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Enemies
{
    public class CasterSkeleton : EnemyBase
    {
        protected override void AdditionalAwake()
        {
            
        }

        protected override async void Attack()
        {
            var dir = (Player.transform.position - transform.position).normalized;

            var rotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = rotation;

            await UniTask.Delay(TimeSpan.FromSeconds(attackDelay));
            
            dir = (Player.transform.position - transform.position).normalized;
            
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