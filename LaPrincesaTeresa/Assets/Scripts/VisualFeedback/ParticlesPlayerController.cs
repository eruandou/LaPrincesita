using System;
using UnityEngine;

namespace VisualFeedback
{
    public class ParticlesPlayerController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem jumpParticles;

        private void Awake()
        {
            PlayerView.OnStartJumpFromGround += OnStarJumpHandler;
        }

        private void OnStarJumpHandler()
        {
            jumpParticles.Play();
        }


        private void OnDisable()
        {
            PlayerView.OnStartJumpFromGround -= OnStarJumpHandler;
        }
    }
}