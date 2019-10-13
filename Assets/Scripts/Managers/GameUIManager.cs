using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _nightsRemainingUIGameobject;
    [SerializeField] private int _nightsRemaining;

    [SerializeField] private Text _nightRemainingAmountText;
    [SerializeField] private Text _nightRemainingDescriptionText;

    private void Start()
    {
        DayNightCycleController.DayNightCycleControllerInstance.OnDayTimeStarted += DayStarted;
        DayNightCycleController.DayNightCycleControllerInstance.OnNightTimeStarted += NightStarted;

        _nightRemainingAmountText.text = _nightsRemaining.ToString();
    }

    private void DayStarted()
    {
        NightPassed();

        _nightsRemainingUIGameobject.SetActive(true);
    }

    private void NightStarted()
    {
        _nightsRemainingUIGameobject.SetActive(false);
    }

    private void NightPassed()
    {
        _nightsRemaining--;

        if (_nightsRemaining > 0)
        {
            _nightRemainingAmountText.text = _nightsRemaining.ToString();
        }
        else if (_nightsRemaining == 0)
        {
            _nightRemainingAmountText.text = GameText.Empty;
            _nightRemainingDescriptionText.text = GameText.NightsRemainingLastDay;
        }
        else
        {
            GameSettings.GameOver();
        }
    }
}
