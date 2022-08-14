using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    public class InGameCanvas : MonoBehaviour
    {
        private DialogueManager _dialogueManager;
        //[SerializeField] private UIEventListener uiEventListener;

        private void Awake()
        {
            _dialogueManager = GetComponentInChildren<DialogueManager>();
            Assert.IsNotNull(_dialogueManager);
        }

        public void UIEventCallback(UIParams callbackContext)
        {
            switch (callbackContext.command)
            {
                case UICommand.DialogueCommand:
                    DialogueManagementEvent(callbackContext);
                    break;
                case UICommand.DeathCommand:

                    break;
            }
        }

        private void DialogueManagementEvent(UIParams p)
        {
            
        }
    }
}