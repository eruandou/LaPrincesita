using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Panel : MonoBehaviour
{
    private bool isOpen;

    private Action OnOpen = delegate { };
    private Action OnClose = delegate { };

    public void SetOpenCallback(Action onOpenAction) => OnOpen += onOpenAction;
    public void ClearOpenCallbacks() => OnOpen = delegate { };

    public void SetCloseCallback(Action onCloseAction) => OnClose += onCloseAction;
    public void ClearCloseCallbacks() => OnClose = delegate { };

    public bool GetIsOpen()
    {
        return isOpen;
    }

    public void Open()
    {
        isOpen = true;
        OnOpen.Invoke();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        isOpen = false;
        OnClose.Invoke();
        gameObject.SetActive(false);
    }
}