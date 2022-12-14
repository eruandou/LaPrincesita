using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class BGMSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioMixerSnapshot useSnapshot;
        private void Start()
        {
            useSnapshot.TransitionTo(1f);
        }
    }
}
