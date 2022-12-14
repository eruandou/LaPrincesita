using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "LaPrincesa/Audio/AudioPool")]
    public sealed class AudioPool : ScriptableObject
    {
        [SerializeField] private List<AudioClipData> clipData;
        private Dictionary<string, AudioClip> _idToAudio;

        public AudioClip RequestAudioClip(string audioID)
        {
            if (_idToAudio == default)
                CreateDictionary();

            return _idToAudio.TryGetValue(audioID, out AudioClip clip) ? clip : default;
        }

        private void CreateDictionary()
        {
            _idToAudio = new Dictionary<string, AudioClip>();
            foreach (var clipInfo in clipData)
            {
                _idToAudio.Add(clipInfo.AudioID, clipInfo.clip);
            }
        }
    }
}