using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private GameObject switchMov;
    [SerializeField] private Transform activePOs;
    [SerializeField] private float timeAnim;
    [SerializeField] private GameObject crossRoad;


    private bool _isActive;
    private Vector3 _originalPos;

    private Collider2D _trigger;

    // Script Temporal cambiar por Interactable 
    void Start()
    {
        _trigger = GetComponent<Collider2D>();
        _originalPos = switchMov.transform.position;
        crossRoad.SetActive(false);
    }

    private void Update()
    {
        if (!_isActive && _originalPos == switchMov.transform.position) return;

        var actVect = _isActive ? activePOs.position : _originalPos;
        Lerping(switchMov.transform.position, actVect);
        if (switchMov.transform.position == activePOs.position)
        {
            crossRoad.SetActive(true);
        }
        else
        {
            crossRoad.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        _isActive = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _isActive = false;
    }

    private void Lerping(Vector3 pointA, Vector3 pointB)
    {
        switchMov.transform.position = Vector3.Lerp(pointA, pointB, 1);
    }
}