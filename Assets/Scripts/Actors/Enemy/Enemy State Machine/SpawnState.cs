using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnState : BaseState
{
    private Enemy _enemy;

    public SpawnState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;
    }

    public override void OnEnter()
    {
        _enemy.gameObject.SetActive(true);
    }

    public override Type Tick()
    {
        Vector3 enemySpawnPosition = transform.position;
        enemySpawnPosition.y = 0;
        enemySpawnPosition.x = UnityEngine.Random.Range(-EnemySettings.xPositionRange, EnemySettings.xPositionRange);

        if (enemySpawnPosition.x > 0)
        {
            enemySpawnPosition.x += UnityEngine.Random.Range(EnemySettings.MinRadius, EnemySettings.MinRadius * 2);
        }

        if (enemySpawnPosition.x < 0)
        {
            enemySpawnPosition.x -= UnityEngine.Random.Range(EnemySettings.MinRadius, EnemySettings.MinRadius * 2);
        }

        enemySpawnPosition.z += UnityEngine.Random.Range(-EnemySettings.zPositionRange, EnemySettings.zPositionRange);

        if (enemySpawnPosition.z > 0)
        {
            enemySpawnPosition.z += UnityEngine.Random.Range(EnemySettings.MinRadius, EnemySettings.MinRadius * 2);
        }

        if (enemySpawnPosition.z < 0)
        {
            enemySpawnPosition.z -= UnityEngine.Random.Range(EnemySettings.MinRadius, EnemySettings.MinRadius * 2);
        }

        enemySpawnPosition.y = _enemy.transform.position.y;
        _enemy.transform.position = enemySpawnPosition;

        return typeof(ChaseState);
    }
}
