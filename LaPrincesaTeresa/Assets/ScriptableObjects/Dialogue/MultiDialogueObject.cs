using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableObjects.Dialogue
{
    [CreateAssetMenu(fileName = "MultiDialogueObject", menuName = "ScriptableObjects/UI/MultiDialogue", order = 0)]
    public class MultiDialogueObject : ScriptableObject
    {
        public DialogueObject[] dialogueObjects;

        private int _currentIndex;
        public int PositionedIndex { get; private set; }

        public void ResetDialogue()
        {
            _currentIndex = 0;
            PositionedIndex = 0;
        }

        /// <summary>
        /// Call to get next dialogue in line
        /// </summary>
        /// <returns>states whether there's more dialogue or not</returns>
        public bool CheckNextDialogueAvailable()
        {
            PositionedIndex = _currentIndex++;
            return PositionedIndex < dialogueObjects.Length;
        }

        public DialogueObject GetNextDialogue()
        {
            return dialogueObjects[PositionedIndex];
        }


#if UNITY_EDITOR
        [ContextMenu("Purge dialogue")]
        public void PurgeDialogue()
        {
            dialogueObjects = dialogueObjects.Distinct().ToArray();
        }

        public float GetAllDialogueDelay()
        {
            //Time to fade in and out
            var time = 1.6f;
            const float timeBetweenDialogues = 1.5f;
            for (int i = 0; i < dialogueObjects.Length; i++)
            {
                var currDialogueObject = dialogueObjects[i];
                var thisTime = currDialogueObject.timeBetweenCharacters * currDialogueObject.dialogue.Length;
                time += thisTime + timeBetweenDialogues;
            }

            return time;
        }
#endif
    }
}