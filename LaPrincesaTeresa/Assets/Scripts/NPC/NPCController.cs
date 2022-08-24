using System;
using Interface;
using ScriptableObjects.Dialogue;
using ScriptableObjects.Events;
using UI;
using UnityEngine;

namespace NPC
{
    public class NPCController : MonoBehaviour, IInteractable
    {
        [SerializeField] private MultiDialogueObject npcDialogue;
        [SerializeField] private UIEvent uiEvent;
        private PlayerModel _model;
        private bool _isInteractable;

        private void Awake()
        {
            _isInteractable = true;
        }

        public void OnInteract(PlayerModel model)
        {
            if (!_isInteractable)
                return;
            _isInteractable = false;
            _model = model;

            OnInteractionChange(ControllerTypes.Dialogue);

            uiEvent.Raise(new UIParams()
            {
                command = UICommand.DialogueCommand,
                message = npcDialogue
            });

            DialogueManager.OnDialogueFinished += FinishedInteractionCallback;
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
            DialogueManager.OnDialogueFinished -= FinishedInteractionCallback;
        }

        public bool IsInteractable()
        {
            return _isInteractable;
        }
    }
}