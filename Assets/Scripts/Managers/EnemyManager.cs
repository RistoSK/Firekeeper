using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;

    private List<NavMeshAgent> _enemiesNavAgentsList;

    private void Start()
    {
        _enemiesNavAgentsList = new List<NavMeshAgent>();

        for (int i = 0; i < EnemySettings.SpawnAmount; i++)
        {
            GameObject spawnedZombie = Instantiate(_enemy, transform); 
            _enemiesNavAgentsList.Add(spawnedZombie.GetComponent<NavMeshAgent>());      
        }
    } 
}
