using UnityEngine;

public class UIRoot : MonoBehaviour
{
    public virtual void ShowRoot()
    {
        gameObject.SetActive(true);
    }

    public virtual void HideRoot()
    {
        gameObject.SetActive(false);
    }
}
