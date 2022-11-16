using System;
using UnityEngine;
using UnityEngine.Events;

namespace InteractableObjects
{
    public class ProximityByCollider : MonoBehaviour
    {
        [SerializeField] private LayerMask layerToCheckAgainst;
        [SerializeField] private UnityEvent<bool> objectIsInProximity;
        [SerializeField] private UnityEvent enterRange, exitRange;

        private void Awake()
        {
            objectIsInProximity.Invoke(false);
        }

        public void ForceOff()
        {
            if (TryGetComponent(out Collider2D coll))
                coll.enabled = false;
            objectIsInProximity.Invoke(false);
            exitRange.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (GameStaticFunctions.IsGoInLayerMask(col.gameObject, layerToCheckAgainst))
            {
                objectIsInProximity.Invoke(true);
                enterRange.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            objectIsInProximity.Invoke(false);
            exitRange.Invoke();
        }
    }
}