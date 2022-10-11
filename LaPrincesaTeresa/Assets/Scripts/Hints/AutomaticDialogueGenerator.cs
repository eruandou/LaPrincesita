using System.Collections;
using Hints;
using Level;
using ScriptableObjects.Events;
using UI;
using UnityEngine;

[RequireComponent(typeof(HintEventListener))]
public class AutomaticDialogueGenerator : MonoBehaviour, IHintReceiver
{
    [SerializeField] private AutomaticLevelHintDialogue hints;
    [SerializeField] private UIEvent uiEvent;


    private Coroutine _countdownCoroutine;

    private void Awake()
    {
        hints.InitializeDialogueHint();
    }

    private IEnumerator CountdownCoroutine()
    {
        while (true)
        {
            var nextIntervalDialogue = hints.GetNextIntervalDialogue();
            if (nextIntervalDialogue == default)
            {
                break;
            }

            var timer = nextIntervalDialogue.timeToTrigger + Time.time;

            while (timer > Time.time)
            {
                yield return null;
            }

            uiEvent.Raise(new UIParams(UICommand.DiegeticDialogueCommand, nextIntervalDialogue.dialogueObject));
        }

        _countdownCoroutine = null;
    }


    private void TryStartCounter()
    {
        if (_countdownCoroutine != null)
            return;
        _countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }

    private void TryStopCounter()
    {
        if (_countdownCoroutine == null)
            return;
    }

    public void ReceiveHintEvent(HintEventParam hintEventParam)
    {
        switch (hintEventParam.hintEventCommand)
        {
            case HintEventCommands.StartHint:
                TryStartCounter();
                break;
            case HintEventCommands.CompletedHint:
                TryStopCounter();
                break;
        }
    }
}