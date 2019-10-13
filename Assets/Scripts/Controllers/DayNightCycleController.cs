using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCycleController : MonoBehaviour
{
    [SerializeField] private Light _sun;
    [SerializeField] private Light _moon;
    [SerializeField] private float _lightTransitionTime;
    [SerializeField] private Light _campFire;
    [SerializeField] private float _campFireMin;
    [SerializeField] private float _campFireMax;
    [SerializeField] private float _campFireBlendTime;
    [SerializeField] private float _sunriseIntensity = 10;
    [SerializeField] private float _sunriseFlashTime = 0.3f;
    [SerializeField] private RectTransform _sundialHandle;

    [Header("Time in minutes")]
    [SerializeField] private float _dayLength = 5;
    [Header("Time in minutes")]
    [SerializeField] private float _nightLength = 3;

    private Transform _sunTransform;
    private Transform _moonTransform;
    private Quaternion _sunRotationStored;
    private Quaternion _moonRotationStored;

    private float _sunIntensity;
    private float _moonIntensity;

    private float _daySeconds;
    private float _nightSeconds;
    private float _time = 0;
    private float _dayProgress;
    private float _nightProgress;

    private bool _isDay;
    public event Action OnNightTimeStarted;
    public event Action OnDayTimeStarted;

    public static DayNightCycleController DayNightCycleControllerInstance { get; private set; }
    public static bool IsDay => DayNightCycleControllerInstance._isDay;

    private void Awake()
    {
        if (DayNightCycleControllerInstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DayNightCycleControllerInstance = this;
        }
    }

    private void Start()
    {
        _daySeconds = _dayLength * 60;
        _nightSeconds = _nightLength * 60;
        _sunTransform = _sun.transform;
        _moonTransform = _moon.transform;
        _sunRotationStored = _sunTransform.rotation;
        _moonRotationStored = _moonTransform.rotation;
        _sun.enabled = false;
        _moon.enabled = true;
        _sunIntensity = _sun.intensity;
        _moonIntensity = _moon.intensity;
    }

    private void FixedUpdate()
    {
        //tick
        _time += Time.fixedDeltaTime;

        if (_isDay)
        {
            //DAY
            if (_time > _daySeconds)
            {
                //switch to night
                _isDay = false;
                OnNightTimeStarted?.Invoke();
                _time = 0;
                Sunset();
            }
            else
            {
                //get percentage of current day time
                _dayProgress = _time / _daySeconds;
                //reset sun rotation
                _sunTransform.rotation = _sunRotationStored;
                //rotate the sun
                _sunTransform.rotation *= Quaternion.Euler( transform.right * (_dayProgress * 180));
                //set the dayProgress
                if (_dayProgress > 0.8)
                {
                    StartCoroutine(TurnDownSun(_lightTransitionTime));
                } 
            }
        }
        else
        {
            //NIGHT
            if (_time > _nightSeconds)
            {
                //switch to day
                _isDay = true;
                OnDayTimeStarted?.Invoke();

                _time = 0;
                Sunrise();
            }
            else
            {
                //get percentage of current day time
                _nightProgress = _time / _nightSeconds;
                //reset sun rotation
                _moonTransform.rotation = _moonRotationStored;
                //rotate the sun
                _moonTransform.rotation *= Quaternion.Euler(transform.right * (_nightProgress * 180));
            }
        }

        //z = 0 is night start, rotate negatively to go counterclockwise
        float angle;

        if (_isDay)
        {
             angle = (1 - _dayProgress) * 180;
        }
        else
        {
             angle = -_nightProgress * 180;
        }

        Vector3 rot = _sundialHandle.eulerAngles;
        rot.z = angle;
        _sundialHandle.eulerAngles = rot;
    }

    private void Sunset()
    {
        _sun.enabled = false;
        _moon.enabled = true;

        //turn up the campfire
        StartCoroutine(TurnUpCampfire(_campFireBlendTime));

        //_player.canEnterCampAtNight = true;
    }

    private void Sunrise()
    {
        _sun.enabled = true;
        StartCoroutine(TurnUpSun(_lightTransitionTime));
        _moon.enabled = false;
        StartCoroutine(SunFlash(_sunriseFlashTime));
        //turn down the campfire
        StartCoroutine(TurnDownCampfire(_campFireBlendTime));
    }

    private IEnumerator TurnUpCampfire(float time)
    {
        float timePassed = 0;

        while (timePassed < time)
        {
            timePassed += Time.fixedDeltaTime;
            float factor = timePassed / time;
            float luminance = Mathf.Lerp(_campFireMin, _campFireMax, factor);
            _campFire.intensity = luminance;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator TurnDownCampfire(float time)
    {
        float timePassed = 0;

        while (timePassed < time)
        {
            timePassed += Time.fixedDeltaTime;
            float factor = timePassed / time;
            float luminance = Mathf.Lerp(_campFireMax, _campFireMin, factor);
            _campFire.intensity = luminance;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator TurnUpSun(float time)
    {
        float timePassed = 0;

        while (timePassed < time)
        {
            timePassed += Time.fixedDeltaTime;
            float factor = timePassed / time;
            float luminance = Mathf.Lerp(0, _sunIntensity, factor);
            _sun.intensity = luminance;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator SunFlash(float time)
    {
        float timePassed = 0;
        float startIntensity = _sun.intensity;

        while (timePassed < time)
        {
            timePassed += Time.fixedDeltaTime;
            float factor = timePassed / time;
            float luminance = Mathf.Lerp(startIntensity, _sunriseIntensity, factor);
            _sun.intensity = luminance;
            yield return new WaitForFixedUpdate();
        }

        timePassed = 0;
        //_enemyManager.Exterminate();

        while (timePassed < time)
        {
            timePassed += Time.fixedDeltaTime;
            float factor = timePassed / time;
            float luminance = Mathf.Lerp(_sunriseIntensity, startIntensity, factor);
            _sun.intensity = luminance;
            yield return new WaitForFixedUpdate();
        }
      
    }

    private IEnumerator TurnDownSun(float time)
    {
        float timePassed = 0;

        while (timePassed < time)
        {
            timePassed += Time.fixedDeltaTime;
            float factor = timePassed / time;
            float luminance = Mathf.Lerp(_sunIntensity, 0, factor);
            _sun.intensity = luminance;
            yield return new WaitForFixedUpdate();
        }
    }
}
