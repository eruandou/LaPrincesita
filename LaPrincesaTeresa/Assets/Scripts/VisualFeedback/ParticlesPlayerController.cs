using System;
using UnityEngine;

namespace VisualFeedback
{
    public class ParticlesPlayerController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem jumpParticles;
        [SerializeField] private ParticleSystem startRunParticles;

        private void Awake()
        {
            PlayerView.OnStartJumpFromGround += () => jumpParticles.Play();
        }
    }
}