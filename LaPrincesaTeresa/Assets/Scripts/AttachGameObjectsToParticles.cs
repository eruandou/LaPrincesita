﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(ParticleSystem))]
public class AttachGameObjectsToParticles : MonoBehaviour
{
    public GameObject m_Prefab;

    private ParticleSystem m_ParticleSystem;
    private List<GameObject> m_Instances = new List<GameObject>();
    private ParticleSystem.Particle[] m_Particles;
    [SerializeField] private float lightMaxIntensity = 1;

    // Start is called before the first frame update
    void Start()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_ParticleSystem.main.maxParticles];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int count = m_ParticleSystem.GetParticles(m_Particles);

        while (m_Instances.Count < count)
            m_Instances.Add(Instantiate(m_Prefab, m_ParticleSystem.transform));

        bool worldSpace = (m_ParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
        for (int i = 0; i < m_Instances.Count; i++)
        {
            if (i < count)
            {
                if (worldSpace)
                    m_Instances[i].transform.position = m_Particles[i].position;
                else
                    m_Instances[i].transform.localPosition = m_Particles[i].position;
                m_Instances[i].SetActive(true);
                //Cambia la intensidad de la luz en base al tamaño de la particula :D
                var currLight = m_Instances[i].GetComponent<Light2D>();
               // currLight.intensity = (m_Particles[i].GetCurrentSize(m_ParticleSystem) / m_Particles[i].startSize) * lightMaxIntensity;
               
               //le cambio la intensidad a la particula en base a su lifetime
                currLight.intensity = (m_Particles[i].remainingLifetime / m_Particles[i].startLifetime) * lightMaxIntensity;
                //cambio el radio de la luz en base a su tamaño.
                currLight.pointLightInnerRadius = m_Particles[i].GetCurrentSize(m_ParticleSystem) / m_Particles[i].startSize;
            }
            else
            {
                m_Instances[i].SetActive(false);
            }
        }
    }
}
