using System;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class DiegeticDialogueManager : GenericDialogueManager
    {
        public static event Action OnDiegeticDialogueFinished;
        private WaitForSeconds _waitForSeconds = new(1.5f);

        protected override void DialogueFinished()
        {
            OnDiegeticDialogueFinished?.Invoke();
        }

        protected override void FinishedTypingTextCallback()
        {
            StartCoroutine(WaitToNextDialogue());
        }

        private IEnumerator WaitToNextDialogue()
        {
            yield return _waitForSeconds;
            NextDialogueTest();
        }
    }
}