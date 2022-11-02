using System;
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
    private Coroutine _eventCoroutine;
    private WaitForSeconds _waitTime;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_triggered || !GameStaticFunctions.IsGoInLayerMask(col.gameObject, playerCollision) ||
            _eventCoroutine != default) return;

        _triggered = true;

        if (isRepeatable)
        {
            _eventCoroutine = StartCoroutine(RepeatCooldown());
        }

        uiEvent.Raise(new UIParams(UICommand.DiegeticDialogueCommand, dialogue));
    }

    private IEnumerator RepeatCooldown()
    {
        _waitTime ??= new WaitForSeconds(repeatCooldown);
        yield return _waitTime;
        _triggered = false;
        _eventCoroutine = default;
    }

#if UNITY_EDITOR
    [ContextMenu("Get minimum wait time")]
    private void GetWaitTime()
    {
        var proposedWaitTime = dialogue.GetAllDialogueDelay();
        if (proposedWaitTime > repeatCooldown)
        {
            repeatCooldown = proposedWaitTime;
        }
    }

    private void OnValidate()
    {
        GetWaitTime();
    }
#endif
}