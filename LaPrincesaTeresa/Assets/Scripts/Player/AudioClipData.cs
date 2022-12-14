using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "LaPrincesa/Audio/AudioData")]
    public sealed class AudioClipData : ScriptableObject
    {
        [field: SerializeField] public string AudioID { get; private set; }
        public AudioClip clip;
    }
}