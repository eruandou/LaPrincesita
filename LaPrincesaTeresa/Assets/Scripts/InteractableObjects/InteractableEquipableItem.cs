using System;
using ScriptableObjects.Extras;
using UnityEngine;

namespace InteractableObjects
{
    public class InteractableEquipableItem : InteractableObject
    {
        [SerializeField] private string socketName;
        [SerializeField] private ItemData data;
        private UpAndDownConstantMovement _movement;

        public static event Action<string, ItemData> OnItemPickedUp = delegate(string s, ItemData data)
        {
        };

        protected override void Awake()
        {
            base.Awake();
            _movement = new UpAndDownConstantMovement(transform, data);
        }

        private void Start()
        {
            StartCoroutine(_movement.UpAndDownMovement());
        }

        public override void OnInteract(PlayerModel model)
        {
            base.OnInteract(model);

            var socket = model.GetSocket(socketName);

#if UNITY_EDITOR
            if (socket == default)
            {
                Debug.LogError($"No socket named {socketName} found in player");
                return;
            }
#endif

            _movement.SpeedChange();
            SetInteractable(false);
            var transform1 = transform;
            transform1.parent = socket;
            transform1.localPosition = Vector3.zero;
            transform1.localRotation = Quaternion.identity;
            FinishedInteractionCallback();
            OnItemPickedUp.Invoke(socketName, data);
        }

        public void UseItem()
        {
        }

#if UNITY_EDITOR
        public void SetDefaults()
        {
            socketName = "BackSocket";
        }

        public void SetSocketByEditor(string socket)
        {
            socketName = socket;
        }
#endif
    }
}