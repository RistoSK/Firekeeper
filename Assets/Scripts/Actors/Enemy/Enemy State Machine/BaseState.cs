using UnityEngine;
using System;

public abstract class BaseState
{
    protected BaseState(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
    }

    protected GameObject gameObject;
    protected Transform transform;

    public abstract void OnEnter();
    public abstract Type Tick();
}
