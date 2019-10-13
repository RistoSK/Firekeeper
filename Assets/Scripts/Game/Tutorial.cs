using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    public GameObject buildMessage;
    public float buildMessageLength;
    public GameObject surviveMessage;
    public float surviveMessageDelay;
    public float surviveMessageLength;

    void Start()
    {
        StartCoroutine(BuildMessage());
    }

    IEnumerator BuildMessage()
    {
        buildMessage.SetActive(true);
        yield return new WaitForSeconds(buildMessageLength);
        buildMessage.SetActive(false);
        StartCoroutine(SurviveMessage());
    }

    IEnumerator SurviveMessage()
    {
        yield return new WaitForSeconds(surviveMessageDelay);
        surviveMessage.SetActive(true);
        yield return new WaitForSeconds(surviveMessageLength);
        surviveMessage.SetActive(false);
        Destroy(this.gameObject);
    }

}
