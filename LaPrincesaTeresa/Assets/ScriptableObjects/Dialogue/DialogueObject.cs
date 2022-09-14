using UnityEngine;

namespace ScriptableObjects.Dialogue
{
    [CreateAssetMenu(fileName = "NewDialogueObject", menuName = "ScriptableObjects/UI/Dialogue", order = 0)]
    public class DialogueObject : ScriptableObject
    {
        public Sprite speakerImage;
        public string speakerName;
        public float timeBetweenCharacters;
        [TextArea(1, 3)] public string dialogue;

#if UNITY_EDITOR
        [Header("EDITOR ONLY")]
        [SerializeField] private int characterPerSecond;

        [ContextMenu("Set characters per second")]
        public void SetCharactersSpeed()
        {
            timeBetweenCharacters = 1f / characterPerSecond;
        }

#endif
    }
}