using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FallingPlatform : MonoBehaviour
{
    public float timer;
    private Vector3 _origin;
    private float currCD;

    private float destroyTime;
    public float countDown;
    
    private Rigidbody2D _rb;
    private Collider2D[] _collider2Ds;
    public GameObject visual;
    private bool hasToFall;
    private bool restore; // tendria que hacer una coroutine jeje
    private bool activeTrap;
    private void Awake()
    {
        hasToFall = true;
        restore = false;
        activeTrap = false;

        _origin = transform.position;
        _collider2Ds = GetComponents<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true;
     
    }


    // Update is called once per frame
    void Update()
    {
        if(!activeTrap) return;

        if (currCD<Time.time && hasToFall)
        {
            _rb.isKinematic = false;
            hasToFall = false;
            destroyTime = Time.time + countDown;
        }

        if (!hasToFall && destroyTime<Time.time)
        {

            EnableDisableThingys(false);
            
        }

        if (restore)
        {
            EnableDisableThingys(true);
            hasToFall = true;
            activeTrap = false;
            _rb.isKinematic = true;
            _rb.velocity = Vector2.zero;
        }
        
    }

    void EnableDisableThingys(bool enaDis)
    {
        for (int i = 0; i < _collider2Ds.Length; i++)
        {
            _collider2Ds[i].enabled = enaDis;
        }

        visual.SetActive(enaDis);
       // _rb.collisionDetectionMode. = enaDis ? true : false;
        if (enaDis)
        {
            transform.position = _origin;
            restore = false;
        }
        else
        {
            restore = true;
        }
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        currCD = Time.time + timer;
        activeTrap = true;
    }
    
    
    
}
