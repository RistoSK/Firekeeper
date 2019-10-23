using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private readonly Enemy _enemy;
    private float _attackTimer = 0f;

    public AttackState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;
    }

    public override void OnEnter()
    {
    }

    public override Type Tick()
    {
        if (DayNightCycleController.IsDay)
        {
            return typeof(DeadState);
        }

        //TODO consider removing this hack
        if (_enemy.targetsHealth == null)
        {
            return null;
        }

        if (_enemy.targetsHealth.GetCurrentHealthPoints() <= 0f)
        {
            return typeof(ChaseState);
        }

        _attackTimer += Time.deltaTime;

        if (_attackTimer >= EnemySettings.AttackCooldown)
        {
            _enemy.Attack();
            _attackTimer = 0f;
        }
        else
        {
            _enemy.PrepareToAttack();
        }

        return null;
    }
}
