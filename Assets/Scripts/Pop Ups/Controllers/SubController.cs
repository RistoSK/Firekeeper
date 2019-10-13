using UnityEngine;

public abstract class SubController : MonoBehaviour
{
    [HideInInspector] public RootPopUpController root;

    public virtual void InitiateController()
    {
        gameObject.SetActive(true);
    }

    public virtual void DisableController()
    {
        gameObject.SetActive(false);
    }
}

public abstract class SubController<T> : SubController where T : UIRoot
{
    [SerializeField] protected T ui;

    public T UI => ui;

    public override void InitiateController()
    {
        base.InitiateController();

        ui.ShowRoot();
    }

    public override void DisableController()
    {
        base.DisableController();

        ui.HideRoot();
    }
}