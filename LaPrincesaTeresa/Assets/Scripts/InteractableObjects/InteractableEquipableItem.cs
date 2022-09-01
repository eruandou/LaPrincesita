using ScriptableObjects.Extras;
using UnityEngine;
using UnityEngine.Assertions;

namespace InteractableObjects
{
    public class InteractableEquipableItem : InteractableObject
    {
        [SerializeField] private string socketName;
        [SerializeField] private ItemGenericData genericData;
        private UpAndDownConstantMovement _movement;

        protected override void Awake()
        {
            base.Awake();
            _movement = new UpAndDownConstantMovement(transform, genericData);
            Assert.IsNotNull(_movement);
        }

        private void Start()
        {
            StartCoroutine(_movement.UpAndDownMovement());
        }

        public override void OnInteract(PlayerModel model)
        {
            base.OnInteract(model);

            var socket = model.GetSocket(socketName);

            if (socket == default)
            {
                Debug.LogError($"No socket named {socketName} found in player");
                return;
            }
            _movement.SpeedChange();
            SetInteractable(false);
            var transform1 = transform;
            transform1.parent = socket;
            transform1.localPosition = Vector3.zero;
            transform1.localRotation = Quaternion.identity;
            FinishedInteractionCallback();
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