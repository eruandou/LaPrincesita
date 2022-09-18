using System;
using UnityEngine;
using UnityEngine.Events;

namespace InteractableObjects
{
    public class ProximityByCollider : MonoBehaviour
    {
        [SerializeField] private LayerMask layerToCheckAgainst;
        [SerializeField] private UnityEvent<bool> objectIsInProximity;


        private void Awake()
        {
            objectIsInProximity.Invoke(false);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (GameStaticFunctions.IsGoInLayerMask(col.gameObject, layerToCheckAgainst))
            {
                objectIsInProximity.Invoke(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            objectIsInProximity.Invoke(false);
        }
    }
}