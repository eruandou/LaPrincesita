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
        if (IsOpen) return;
        IsOpen = true;
        OnOpen.Invoke();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        if (!IsOpen) return;
        IsOpen = false;
        OnClose.Invoke();
        gameObject.SetActive(false);
    }
}
