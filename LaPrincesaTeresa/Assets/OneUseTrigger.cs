using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class OneUseTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask layerToCheckAgainst;
    [SerializeField] private UnityEvent onPickUp;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (GameStaticFunctions.IsGoInLayerMask(col.gameObject, layerToCheckAgainst))
        {
            onPickUp.Invoke();
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
