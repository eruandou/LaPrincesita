using ScriptableObjects.Dialogue;
using ScriptableObjects.Events;
using UI;
using UnityEngine;

public class DiegeticDialogueTrigger : MonoBehaviour
{
    [SerializeField] private MultiDialogueObject dialogue;
    [SerializeField] private UIEvent uiEvent;
    private bool _triggered;
    [SerializeField] private LayerMask playerCollision;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_triggered || !GameStaticFunctions.IsGoInLayerMask(col.gameObject, playerCollision)) return;

        _triggered = true;
        uiEvent.Raise(new UIParams(UICommand.DiegeticDialogueCommand, dialogue));
    }
}