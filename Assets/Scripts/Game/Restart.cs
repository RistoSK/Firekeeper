     using UnityEngine;
     using UnityEngine.SceneManagement;
     using System.Collections;
     
     public class Restart : MonoBehaviour {
     
         public void RestartGame() {
             SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
            Time.timeScale = 1;
         }
     
     }