using System.Collections;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private GameObject _fencePrefab;
    [SerializeField] private float _particleDuration = 1f;

    public IEnumerator Build()
    {
        yield return new WaitForSeconds(_particleDuration);
        _fencePrefab.SetActive(true);
        _fencePrefab.GetComponent<BoxCollider>().enabled = true;
        gameObject.SetActive(false);
    }
}
