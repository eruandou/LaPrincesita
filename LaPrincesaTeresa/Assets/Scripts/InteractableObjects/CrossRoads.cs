using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Interface;
using UnityEditor.Build.Content;
using UnityEngine;

public class CrossRoads : MonoBehaviour, IInteractable
{
    private bool _isInteractable;

    private void Awake()
    {
        _isInteractable = true;
    }

    public void OnInteract(PlayerModel model)
    {
        if (!_isInteractable)
            return;
        _isInteractable = true;
        GameManager.Instance.LoadMenu();
    }

    public void FinishedInteractionCallback()
    {
    }
}