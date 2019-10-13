using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIView : MonoBehaviour
{
    public virtual void ShowView()
    {
        gameObject.SetActive(true);
    }

    public virtual void HideView()
    {
        gameObject.SetActive(false);
    }
}
