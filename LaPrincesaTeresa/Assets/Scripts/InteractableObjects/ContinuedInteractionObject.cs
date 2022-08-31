using UnityEngine;
using UnityEngine.Events;

namespace InteractableObjects
{
    public class ContinuedInteractionObject : InteractableObject
    {
        [SerializeField,Min(0.1f)] private float interactionCooldown;
        private float _currentInteractionTime;
        [Range(0, 1)] [SerializeField] private float progressPerInteraction;
        private float _currentProgress;
        [SerializeField] private UnityEvent<float> OnInteractionProgressUpdate;
#if UNITY_EDITOR
        [Header("For editor only")]
        [SerializeField, Min(1)] private int numberOfInteractionsNeeded;
#endif
        public override void OnInteract(PlayerModel model)
        {
            if (_currentInteractionTime > Time.time)
                return;

            base.OnInteract(model);

            _currentInteractionTime = Time.time + interactionCooldown;

            _currentProgress += progressPerInteraction;
            OnInteractionProgressUpdate?.Invoke(_currentProgress);
            if (!(_currentProgress >= 1)) return;

            FinishedInteractionCallback();
            SetInteractable(false);
        }

#if UNITY_EDITOR
        [ContextMenu("Calculate progress per interaction")]
        public void CalculateProgressPerInteractionNeeded()
        {
            var calculatedProgressPerInteract = 1f / numberOfInteractionsNeeded;
            progressPerInteraction = calculatedProgressPerInteract;
        }

        public void SetDefaults()
        {
            interactionCooldown = 0.1f;
            progressPerInteraction = 0.2f;
            numberOfInteractionsNeeded = 5;
        }
#endif
    }
}