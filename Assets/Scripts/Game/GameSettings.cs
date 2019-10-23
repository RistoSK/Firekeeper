using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverCanvas;

    private static GameSettings GameSettingsInstance { get; set; }

    private static GameObject GameOverCanvas => GameSettingsInstance._gameOverCanvas;

    public static void GameOver()
    {
        GameOverCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Awake()
    {
        if (GameSettingsInstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            GameSettingsInstance = this;
        }
    }
}
