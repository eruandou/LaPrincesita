﻿using Interface;
using ScriptableObjects.Dialogue;
using ScriptableObjects.Events;
using UI;
using UnityEngine;

namespace NPC
{
    [SelectionBase]
    public class NPCController : MonoBehaviour, IInteractable
    {
        [SerializeField] private MultiDialogueObject npcDialogue;
        [SerializeField] private UIEvent uiEvent;
        private PlayerModel _model;
        private bool _isInteractable;

        private void Awake()
        {
            _isInteractable = true;

#if UNITY_EDITOR
            CheckUniqueNPCController();
#endif
        }

#if UNITY_EDITOR
        private void CheckUniqueNPCController()
        {
            var otherNPCController = GetComponentsInChildren<NPCController>();

            if (otherNPCController != null)
            {
                Debug.LogError($"Character {gameObject.name} has more than one NPC Controller assigned");
                Debug.Break();
            }
        }
#endif

        public void OnInteract(PlayerModel model)
        {
            if (!_isInteractable)
                return;
            _isInteractable = false;
            _model = model;

            OnInteractionChange(ControllerTypes.Dialogue);

            uiEvent.Raise(new UIParams(UICommand.DialogueCommand, npcDialogue));

            InGameDialogueManager.OnInGameDialogueFinished += FinishedInteractionCallback;
        }


        private void OnInteractionChange(ControllerTypes newControllerType)
        {
            if (_model == null || _model.Controller == null)
                return;
            _model.Controller.RequestChangeMap(newControllerType);
        }

        public void FinishedInteractionCallback()
        {
            _isInteractable = true;
            OnInteractionChange(ControllerTypes.Regular);
            InGameDialogueManager.OnInGameDialogueFinished -= FinishedInteractionCallback;
        }

        public bool IsInteractable()
        {
            return _isInteractable;
        }
    }
}