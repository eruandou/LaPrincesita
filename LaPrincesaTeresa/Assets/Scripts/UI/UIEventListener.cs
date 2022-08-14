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
        public string message;
        public Sprite speakerImage;

        public UIParams(UICommand newCommand, string newMessage = default, Sprite speakerImage = default)
        {
            command = newCommand;
            message = newMessage;
            this.speakerImage = speakerImage;
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