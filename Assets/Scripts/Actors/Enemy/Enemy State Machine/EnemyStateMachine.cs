using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EnemyStateMachine : MonoBehaviour
{
    private Dictionary<Type, BaseState> _enemyStates;

    private BaseState CurrentState { get; set; }
    public event Action<BaseState> OnStateChanged;

    public void SetState(Dictionary<Type, BaseState> states)
    {
        _enemyStates = states;
    }

    private void Update()
    {
        if (CurrentState == null)
        {
            SwitchToNewState(typeof(SpawnState));
        }

        var nextState = CurrentState?.Tick();

        if (nextState != null && nextState != CurrentState?.GetType())
        {
            SwitchToNewState(nextState);
        }
    }

    private void SwitchToNewState(Type nextState)
    {
        CurrentState = _enemyStates[nextState];
        CurrentState.OnEnter();

        //TODO CHECK IF NEEDED
        OnStateChanged?.Invoke(CurrentState);
    }
}
