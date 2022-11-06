using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Panel : MonoBehaviour
{
    public bool IsOpen { get; private set; }

    public Action OnOpen = delegate { };
    public Action OnClose = delegate { };

    public void Open()
    {
        IsOpen = true;
        OnOpen.Invoke();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        IsOpen = false;
        OnClose.Invoke();
        gameObject.SetActive(false);
    }
}
