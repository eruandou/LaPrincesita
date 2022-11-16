using Interface;
using UnityEngine;
using UnityEngine.Events;

namespace InteractableObjects
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ThrowableInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private float throwForce = 20f;
        [SerializeField] private UnityEvent onPickUpEvent;
        private bool _isInteractable;
        private Rigidbody2D _rb;
        private Collider2D _collider;

        private void Awake()
        {
            _isInteractable = true;
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        public void OnInteract(PlayerModel model)
        {
            if (!_isInteractable)
                return;
            model.PickupObject(this);
            FinishedInteractionCallback();
        }


        public void ThrowObject(float xDir)
        {
            transform.SetParent(null);
            var throwVector = new Vector2(1, 1).normalized;
            SetInteraction(true);
            throwVector.x *= xDir;
            _rb.AddForce(throwForce * throwVector, ForceMode2D.Impulse);
        }

        public void FinishedInteractionCallback()
        {
            SetInteraction(false);
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0;
            onPickUpEvent?.Invoke();
        }

        public void OnRangeChanged(bool isInRange)
        {
            
        }

        private void SetInteraction(bool isInteractable)
        {
            _isInteractable = isInteractable;
            _rb.isKinematic = !isInteractable;
            _collider.enabled = isInteractable;
        }

#if UNITY_EDITOR
        public void SetBasics()
        {
            var rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 6f;
            }
        }
#endif
    }
}