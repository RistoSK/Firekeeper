using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugMode : MonoBehaviour
{
    [SerializeField] private GameObject _spawner;
    [SerializeField] private FenceHealth[] _FenceHealthList;

    private void Start()
    {
        NoEnemies();
    }

    public void NoEnemies()
    {
        _spawner.SetActive(false); 
    }

    public void YesEnemies()
    {
        _spawner.SetActive(true);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DealDamageToAllFences()
    {
        for (int i = 0; i < _FenceHealthList.Length; i++)
        {
            _FenceHealthList[i].DealDamage(1);
        }
    }
}
