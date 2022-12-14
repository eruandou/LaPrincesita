using UnityEngine;

namespace Player
{
    public class PlayerSfxManager : MonoBehaviour
    {
        [SerializeField] private AudioPool pool;

        public AudioClip GetAudioClip(string audioClipID)
        {
            return pool.RequestAudioClip(audioClipID);
        }
    }
}