using FSG.MeshAnimator;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.Events;

public class Enemy : EnemyManager
{   
    private NavMeshAgent _agent;

    private FSG.MeshAnimator.MeshAnimator _animator;
    public EnemyManager _enemyManager;
    public FenceHealth _targetsHealth;

    public UnityAction OnCollidedWithFence;

    private void Start()
    {
        _animator = GetComponentInChildren<MeshAnimator>();
        _enemyManager = GetComponentInParent<EnemyManager>();
        
        InitialiseStateMachine();
    }

    public void InitialiseStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {
            { typeof(SpawnState), new SpawnState(this)},
            { typeof(ChaseState), new ChaseState(this)},
            { typeof(AttackState), new AttackState(this)},
            { typeof(DeadState), new DeadState(this)}
        };
        GetComponentInParent<EnemyStateMachine>().SetState(states);
    }

    private void OnTriggerEnter(Collider other)
    {
        FenceHealth health = other.gameObject.GetComponent<FenceHealth>();

        if (health != null)
        {
            _targetsHealth = health;
            OnCollidedWithFence?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_targetsHealth != null)
        {
            _targetsHealth = null;
        }
    }

    public void Attack()
    {
        _animator.Play(0);
        _targetsHealth.DealDamage(EnemySettings.AttackDamage);
    }

    public void PrepareToAttack()
    {
        _animator.Play(2);
    }
}
