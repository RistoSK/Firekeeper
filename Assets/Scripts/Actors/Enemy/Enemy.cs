using FSG.MeshAnimator;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.Events;

public class Enemy : EnemyManager
{   
    private NavMeshAgent _agent;

    private MeshAnimator _animator;
    public EnemyManager enemyManager;
    public FenceHealth targetsHealth;

    public UnityAction onCollidedWithFence;

    private void Start()
    {
        _animator = GetComponentInChildren<MeshAnimator>();
        enemyManager = GetComponentInParent<EnemyManager>();
        
        InitialiseStateMachine();
    }

    private void InitialiseStateMachine()
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
            targetsHealth = health;
            onCollidedWithFence?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        targetsHealth = null;
    }

    public void Attack()
    {
        _animator.Play(0);
        targetsHealth.DealDamage(EnemySettings.AttackDamage);
    }

    public void PrepareToAttack()
    {
        _animator.Play(2);
    }
}
