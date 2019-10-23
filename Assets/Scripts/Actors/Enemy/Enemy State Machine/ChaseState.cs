using System;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : BaseState
{
    private Enemy _enemy;
    private NavMeshAgent _enemyAgent;
    private Rigidbody _rigidBody;

    private bool _fenceCollision = false;

    public ChaseState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;
        _enemyAgent = _enemy.GetComponent<NavMeshAgent>();
        _rigidBody = _enemy.GetComponent<Rigidbody>();
    }

    public override void OnEnter()
    {
        _enemy.onCollidedWithFence += OnFenceCollision;

        _fenceCollision = false;     
    }

    public override Type Tick()
    {
        if (DayNightCycleController.IsDay)
        {
            return typeof(DeadState);
        }

        Vector3 playerPosition = PlayerController.PlayerPosition;

        transform.position = Vector3.MoveTowards(transform.position, playerPosition, EnemySettings.EnemySpeed * Time.deltaTime);
        transform.LookAt(PlayerController.PlayerPosition);      

        if (_fenceCollision)
        {
            return typeof(AttackState);
        }

        return null;
    }

    private void OnFenceCollision()
    {
        _fenceCollision = true;
    }
}