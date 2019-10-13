using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeadState : BaseState
{
    private Enemy _enemy;

    public DeadState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;
    }

    public override void OnEnter()
    {
        _enemy.gameObject.SetActive(false);
    }

    public override Type Tick()
    {
        if (!DayNightCycleController.IsDay)
        {
            return typeof(SpawnState);
        }

        return null;
    }
}
