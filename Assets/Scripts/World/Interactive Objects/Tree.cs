using System.Collections;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private void OnEnable()
    {
        transform.localScale = new Vector3(UnityEngine.Random.Range(80f, 120f), UnityEngine.Random.Range(80f, 120f), UnityEngine.Random.Range(80f, 120f));
    }    

    public IEnumerator Harvest()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
