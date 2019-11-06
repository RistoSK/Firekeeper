using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    [SerializeField] private ResourceManager _resourceHud;
    [SerializeField] private Transform _hudSpawnTransform;
    [SerializeField] private Canvas _uiCanvas;
    [SerializeField] private int _maxPartsAmount;

    [SerializeField] private GameObject _spaceshipPartsHud;

    private int currentPartsAmount;

    private void Update()
    {
        //SetHudPosition(); 
    }

    private void SetHudPosition()
    {
        Vector2 screenPosition = CameraManager.CurrentCamera.WorldToScreenPoint(_hudSpawnTransform.position);
        RectTransform rect = _spaceshipPartsHud.GetComponent<RectTransform>();
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_uiCanvas.GetComponent<RectTransform>(), screenPosition, CameraManager.CurrentCamera, out localPosition);
        _spaceshipPartsHud.transform.localPosition = localPosition;
    }

    public IEnumerator FixSpaceship()
    {
        yield return new WaitForSeconds(1f);
        //do stuff
    }
}
