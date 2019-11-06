using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _roadBuildingPanel;
    [SerializeField] private BuildingStateMachine _buildingStateMachine;

    public enum GameMode
    {
        MainGame,
        BuildingMode
    }
    
    private static GameManager GameManagerInstance { get; set; }

    public static event Action<GameMode> OnModeChanged;

    public static GameMode CurrentGameMode => GameManagerInstance._currentMode;
    
    private GameMode _currentMode;
    
    public void ChangeGameMode(int i)
    {
        switch (i)
        {
            case 1:
                OnModeChanged?.Invoke(GameMode.MainGame);
                _currentMode = GameMode.MainGame;
                _buildingStateMachine.SetWallBuildingMode(4);
                _roadBuildingPanel.SetActive(false);
                break;
            case 2:
                OnModeChanged?.Invoke(GameMode.BuildingMode);
                _roadBuildingPanel.SetActive(true);
                _currentMode = GameMode.BuildingMode;
                break;
        }
    }
    
    private void Awake()
    {
        if (GameManagerInstance)
        {
            Destroy(gameObject);
        }
        else
        {
            GameManagerInstance = this;
        }

        _currentMode = GameMode.BuildingMode;
    }
}
