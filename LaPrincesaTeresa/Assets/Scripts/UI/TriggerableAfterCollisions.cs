using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerableAfterCollisions : MonoBehaviour
{
    [SerializeField] private int neededInteractionTimes;
    [SerializeField] private UnityEvent interacted;
    [SerializeField] private LayerMask layerToCollideWith;
    [SerializeField] private bool isRepeatable;
    private Coroutine _eventCoroutine;
    private int _currentlyInteractedTimes;
    private bool _triggered;
    private WaitForSeconds _waitTime;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_triggered || !GameStaticFunctions.IsGoInLayerMask(col.gameObject, layerToCollideWith) ||
            _eventCoroutine != default || _currentlyInteractedTimes++ < neededInteractionTimes) return;
        TryInteract();
    }

    protected void TryInteract()
    {
        _triggered = !isRepeatable;

        if (isRepeatable)
        {
            _currentlyInteractedTimes = 0;
        }

        interacted?.Invoke();
    }
}