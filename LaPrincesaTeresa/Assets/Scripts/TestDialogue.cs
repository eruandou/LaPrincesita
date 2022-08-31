using ScriptableObjects.Dialogue;
using ScriptableObjects.Events;
using UI;
using UnityEngine;

public class TestDialogue : MonoBehaviour
{
    [SerializeField] private MultiDialogueObject dialogueToDisplay;
    [SerializeField] private UIEvent eventManager;

    private void Start()
    {
        if (eventManager.CheckActiveListeners())
        {
            eventManager.Raise(new UIParams(UICommand.DialogueCommand, dialogueToDisplay));
        }
    }
}