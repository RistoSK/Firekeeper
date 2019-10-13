using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipPart : MonoBehaviour
{
    [SerializeField] private float _particleDuration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {

        }
    }

    public IEnumerator PickUpPart()
    {
        yield return new WaitForSeconds(_particleDuration);
        gameObject.SetActive(false);
    }
}
