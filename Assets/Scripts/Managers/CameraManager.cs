using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _buildingModeCamera;
    [SerializeField] private Transform _hexMapCameraTransform;

    private Camera _currentCamera;
    private static CameraManager CameraManagerInstance { get; set; }

    public static Camera CurrentCamera => CameraManagerInstance._currentCamera;
    
    public static Camera MainCamera => CameraManagerInstance._mainCamera;
    
    public static Camera BuildModeCamera => CameraManagerInstance._buildingModeCamera;
    
    public void ChangeToMainCamera()
    {
            _buildingModeCamera.gameObject.SetActive(false);
            _mainCamera.gameObject.SetActive(true);
            
            _currentCamera = _mainCamera;
    }
    
    public void ChangeToBuildingModeCamera()
    {
        _mainCamera.gameObject.SetActive(false);
        _buildingModeCamera.gameObject.SetActive(true);
        
        _currentCamera = _buildingModeCamera;
    }
    
    private void Awake()
    {
        if (CameraManagerInstance)
        {
            Destroy(gameObject);
        }
        else
        {
            CameraManagerInstance = this;
        }

        _currentCamera = _mainCamera;

        _hexMapCameraTransform.position = new Vector3(420, 0, 480);
        GameManager.OnModeChanged += GameModeChanged;
    }

    private void GameModeChanged(GameManager.GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameManager.GameMode.MainGame:
                ChangeToMainCamera();
                break;
            case GameManager.GameMode.BuildingMode:
                ChangeToBuildingModeCamera();
                break;
        }
    }
}
