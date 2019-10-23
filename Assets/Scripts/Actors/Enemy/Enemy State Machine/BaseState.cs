using UnityEngine;
using System;

public abstract class BaseState
{
    protected BaseState(GameObject gameObject)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
    }

    protected GameObject gameObject;
    protected readonly Transform transform;

    public abstract void OnEnter();
    public abstract Type Tick();
}
