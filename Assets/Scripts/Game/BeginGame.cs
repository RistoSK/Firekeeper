using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginGame : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Firekeeper");
        Time.timeScale = 1;
    }
}
