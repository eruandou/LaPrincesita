using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevate : MonoBehaviour
{
    [SerializeField] private float timeToElevate = 1f;
    public void StartElevation(float elevationAmount)
    {
        transform.position += Vector3.up * elevationAmount;
    }
}
