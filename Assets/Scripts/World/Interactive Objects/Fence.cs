using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fence : MonoBehaviour
{
    [SerializeField] private GameObject _base;
    [SerializeField] private GameObject _uiCanvas;
    [SerializeField] private float _particleDuration = 1f;
    [SerializeField] private GameObject _repairPopUpGameObject;
    [SerializeField] private GameObject _healthBarGameObject;
    [SerializeField] private Transform _hpTrans;

    public FenceStatus status;

    private Vector3 _healthBarPosition;
    private GameObject _currentlyPlayingPFX;
    private FenceHealth _health;
    private Slider _healthBarSlider;

    public enum FenceStatus
    {
        Damaged,
        Fixed
    }

    private void Start()
    {
        _healthBarSlider = _healthBarGameObject.GetComponent<Slider>();
        _health = gameObject.GetComponent<FenceHealth>();

        if (_health == null)
        {
            Debug.LogError("Attach Health script");
        }

        _repairPopUpGameObject.SetActive(false);
        _healthBarGameObject.SetActive(false);

        _health.SetHealthSlider(_healthBarSlider);
        _healthBarPosition = _healthBarGameObject.transform.position;

        status = FenceStatus.Fixed;

        _health.OnFenceDestroyed += FenceIsDestroyed;
        _health.OnFenceDamaged += FenceDamaged;

        CameraFollow.CameraFollowInstance.OnCameraMoved += DisableFenceUI;
        CameraFollow.CameraFollowInstance.OnCameraOnCamp += EnableFenceUI;
    }

    public bool IsFenceFixable()
    {
        return _health.IsFenceDamaged();
    }

    private void FenceIsDestroyed()
    {
        _base.SetActive(true);
        _repairPopUpGameObject.SetActive(false);
        _base.GetComponent<BoxCollider>().enabled = true;
        gameObject.SetActive(false);
    }

    private void FenceDamaged()
    {
        status = FenceStatus.Damaged;
    }

    private void EnableFenceUI()
    {
        if (status == FenceStatus.Damaged)
        {
            _repairPopUpGameObject.SetActive(true);
            _healthBarGameObject.SetActive(true);
        }
    }

    private void DisableFenceUI()
    {
        _repairPopUpGameObject.SetActive(false);
        _healthBarGameObject.SetActive(false);
    }

    public IEnumerator Fix()
    {
        status = FenceStatus.Fixed;
        _repairPopUpGameObject.SetActive(false);
        yield return new WaitForSeconds(_particleDuration);
        _health.RestoreHealth();
        _health.UpdateHealthBar();       
        _healthBarGameObject.SetActive(false);
        Destroy(_currentlyPlayingPFX);
        //TODO: this is here because I am disabling the all box colliders in InteractiveObjectController
        GetComponent<BoxCollider>().enabled = true;
    }
}
