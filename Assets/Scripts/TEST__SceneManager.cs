using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TEST__SceneManager : MonoBehaviour
{
    public void DebugBtn()
    {
        SceneManager.LoadScene(1);
    }

    public void NormalBtn()
    {
        SceneManager.LoadScene(2);
    }

}
