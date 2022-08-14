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
#endif
    }
}