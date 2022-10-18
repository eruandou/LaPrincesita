using System;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class DiegeticDialogueManager : GenericDialogueManager
    {
        public static event Action OnDiegeticDialogueFinished;
        [SerializeField] private float waitBetweenDialogues = 1.5f;
        private WaitForSeconds _waitForSeconds;

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
            _waitForSeconds ??= new WaitForSeconds(waitBetweenDialogues);
            yield return _waitForSeconds;
            NextDialogue();
        }
    }
}