using System;
using System.Collections;
using System.Collections.Generic;
using Interface;
using UnityEngine;

public class ResetableVisualElement : MonoBehaviour, ILevelResetable
{
    private Vector3 _startingPos;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _startingPos = transform.position;
        TryGetComponent(out _rb);
    }

    public void OnResetLevel()
    {
        transform.position = _startingPos;
        if (_rb != null)
        {
            _rb.velocity = Vector3.zero;
        }
    }
}