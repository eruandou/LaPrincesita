using ScriptableObjects.Dialogue;
using ScriptableObjects.Events;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public enum UICommand
    {
        DialogueCommand,
        DeathCommand
    }

    [System.Serializable]
    public struct UIParams
    {
        public UICommand command;
        public MultiDialogueObject message;

        public UIParams(UICommand newCommand, MultiDialogueObject dialogueObject = default)
        {
            command = newCommand;
            message = dialogueObject;
        }
        
    }

    [System.Serializable]
    public class UIUnityEvent : UnityEvent<UIParams>
    {
    }

    public class UIEventListener : MonoBehaviour
    {
        [SerializeField] private UIEvent uiEvent;
        [SerializeField] private UIUnityEvent eventResponse;

        private void OnEnable()
        {
            uiEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            uiEvent.UnregisterListener(this);
        }

        public void OnEventRaised(UIParams p)
        {
            eventResponse.Invoke(p);
        }
    }
}