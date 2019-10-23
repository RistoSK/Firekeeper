using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thermometer : MonoBehaviour
{
    [SerializeField] private DayNightCycleController _dayNightCycle;
    [SerializeField] private GameObject _thermoUi;
    
    private Slider _thermoSlider;
    private bool _isDay;
    private Vector3 _campFirePosition;
    private bool _bIsOnMaxTempeture;

    private void Start()
    {
        _thermoSlider = _thermoUi.GetComponent<Slider>();
        _campFirePosition = new Vector3(0, 0, 0);

        _dayNightCycle.OnNightTimeStarted += NightTimeStarted;
        _dayNightCycle.OnDayTimeStarted += DayTimeStarted;
    }

    private void NightTimeStarted()
    {
        StopAllCoroutines();
        StartCoroutine(NightTimeTemperature());
    }

    private void DayTimeStarted()
    {
        StopAllCoroutines();
        StartCoroutine(DayTimeTemperature());
    }

    private IEnumerator NightTimeTemperature()
    {
        while (true)
        {
            if (Vector3.Distance(_campFirePosition, PlayerController.PlayerPosition) > 7f)
            {
                _thermoUi.SetActive(true);
                _thermoSlider.value -= 0.1f;
            }
            else
            {
                StartGettingWarm();
            }
            
            if (_thermoSlider.value <= 0.0f)
            {
                //GameSettings.GameOver();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator DayTimeTemperature()
    {
        _bIsOnMaxTempeture = false;
        while (!_bIsOnMaxTempeture)
        {
            StartGettingWarm();

            yield return new WaitForSeconds(1f);
        }
    }

    private void StartGettingWarm()
    {
        _thermoSlider.value += 0.25f;

        if (_thermoSlider.value != 1)
        {
            return;
        }
        _bIsOnMaxTempeture = true;
        _thermoUi.SetActive(false);
    }
}
