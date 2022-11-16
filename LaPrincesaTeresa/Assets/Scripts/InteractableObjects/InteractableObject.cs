using Interface;
using UnityEngine;
using UnityEngine.Events;

namespace InteractableObjects
{
    public abstract class InteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private UnityEvent onTriggered, onFinishedInteraction;
        private bool _isInteractable;

        protected virtual void Awake()
        {
            SetInteractable(true);
        }

        public virtual void OnInteract(PlayerModel model)
        {
            if (!_isInteractable)
                return;

            onTriggered?.Invoke();
        }

        public bool IsInteractable()
        {
            return _isInteractable;
        }

        protected void SetInteractable(bool isInteractable)
        {
            _isInteractable = isInteractable;
        }

        public void FinishedInteractionCallback()
        {
            onFinishedInteraction?.Invoke();
        }

        public void OnRangeChanged(bool isInRange)
        {
            
        }
    }
}