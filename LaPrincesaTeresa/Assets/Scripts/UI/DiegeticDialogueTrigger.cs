using System.Collections;
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
    [SerializeField] private bool isRepeatable;
    [SerializeField] private float repeatCooldown = 3;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_triggered || !GameStaticFunctions.IsGoInLayerMask(col.gameObject, playerCollision)) return;

        if (isRepeatable)
        {
            StartCoroutine(RepeatCooldown());
        }
        else
        {
            _triggered = true;
        }
        uiEvent.Raise(new UIParams(UICommand.DiegeticDialogueCommand, dialogue));
    }

    IEnumerator RepeatCooldown()
    {
        _triggered = true;
        yield return new WaitForSeconds(repeatCooldown);
        _triggered = false;
    }
}