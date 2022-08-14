using UnityEngine;

namespace ScriptableObjects.Dialogue
{
    [CreateAssetMenu(fileName = "NewDialogueObject", menuName = "ScriptableObjects/UI/Dialogue", order = 0)]
    public class DialogueObject : ScriptableObject
    {
        public Sprite speakerImage;
        public float timeBetweenCharacters;
        [TextArea(1, 3)] public string dialogue;
    }
}