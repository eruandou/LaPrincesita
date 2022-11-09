using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    public class InGameCanvas : MonoBehaviour
    {
        private InGameDialogueManager _inGameDialogueManager;
        private DiegeticDialogueManager _diegeticDialogueManager;

        private void Awake()
        {
            _inGameDialogueManager = GetComponentInChildren<InGameDialogueManager>();
            _diegeticDialogueManager = GetComponentInChildren<DiegeticDialogueManager>();
          //  Assert.IsNotNull(_inGameDialogueManager);
            Assert.IsNotNull(_diegeticDialogueManager);
        }

        public void UIEventCallback(UIParams callbackContext)
        {
            switch (callbackContext.command)
            {
                case UICommand.DialogueCommand:
                    HandleReceiveDialogue(callbackContext, _inGameDialogueManager);
                    break;
                case UICommand.DeathCommand:

                    break;
                case UICommand.DiegeticDialogueCommand:
                    HandleReceiveDialogue(callbackContext, _diegeticDialogueManager);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void HandleReceiveDialogue(UIParams p, GenericDialogueManager dialogueManager)
        {
            dialogueManager.ReceiveDialogue(p.message);
        }
    }
}