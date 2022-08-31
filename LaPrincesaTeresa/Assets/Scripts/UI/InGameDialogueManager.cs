using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class InGameDialogueManager : GenericDialogueManager
    {
        public static event Action OnInGameDialogueFinished;

        protected override void DialogueFinished()
        {
            OnInGameDialogueFinished?.Invoke();
        }

        protected override void Awake()
        {
            base.Awake();
            var playerInput = FindObjectOfType<PlayerInput>();

            if (playerInput != null)
            {
                SubscribeToEvents(playerInput);
            }
        }

        private void SubscribeToEvents(PlayerInput playerInput)
        {
            var moveOption = playerInput.actions["MoveOption"];
            var selectOption = playerInput.actions["Select"];
            var cancelOption = playerInput.actions["Cancel"];

            moveOption.performed += PlayerPressMove;
            selectOption.performed += PlayerPressSubmit;
            cancelOption.performed += PlayerPressCancel;
        }

        private void PlayerPressMove(InputAction.CallbackContext context)
        {
            Debug.Log($"Move option with {context.ReadValue<Vector2>()}");
        }


        private void PlayerPressSubmit(InputAction.CallbackContext context)
        {
            PressContinueCallback();
        }


        private void PlayerPressCancel(InputAction.CallbackContext context)
        {
            Debug.Log("Player canceled selection");
        }
    }
}