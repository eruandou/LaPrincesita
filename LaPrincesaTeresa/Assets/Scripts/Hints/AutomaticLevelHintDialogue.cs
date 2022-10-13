using System;
using System.Collections.Generic;
using ScriptableObjects.Dialogue;
using UnityEngine;

namespace Hints
{
    [CreateAssetMenu(menuName = "ScriptableObjects/LevelHints/HintDialogues")]
    public class AutomaticLevelHintDialogue : ScriptableObject
    {
        [SerializeField] private List<DialogueInterval> hints;
        [SerializeField] private bool repeatLast;
        private int _pointer;

        public void InitializeDialogueHint()
        {
            _pointer = 0;
        }

        public DialogueInterval GetNextIntervalDialogue()
        {
            if (_pointer >= hints.Count)
            {
                return repeatLast ? hints[^1] : default;
            }

            var dialogueToReturn = hints[_pointer];
            _pointer++;
            if (_pointer > hints.Count)
            {
                _pointer = hints.Count;
            }

            return dialogueToReturn;
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = 0; i < hints.Count; i++)
            {
                var currHintObject = hints[i];
                var thisObjectTime = currHintObject.dialogueObject.GetAllDialogueDelay();

                if (i + 1 >= hints.Count) continue;

                var nextDialogue = hints[i + 1];
                if (nextDialogue.timeToTrigger < thisObjectTime)
                {
                    nextDialogue.timeToTrigger = thisObjectTime;
                }

                if (i == hints.Count - 1)
                {
                    currHintObject.timeToTrigger = thisObjectTime;
                }
            }
        }
#endif
    }
}

[System.Serializable]
public sealed class DialogueInterval
{
    public MultiDialogueObject dialogueObject;
    public float timeToTrigger;
}