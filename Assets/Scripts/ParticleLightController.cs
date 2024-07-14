using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ParticleLightController : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public GameObject lightPrefab;
    public int maxLights = 10;

    private ParticleSystem.Particle[] particles;
    private GameObject[] lights;

    void Start()
    {
        particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        lights = new GameObject[maxLights];

        for (int i = 0; i < maxLights; i++)
        {
            lights[i] = Instantiate(lightPrefab);
            lights[i].SetActive(false);
        }
    }

    void LateUpdate()
    {
        int particleCount = particleSystem.GetParticles(particles);

        for (int i = 0; i < lights.Length; i++)
        {
            if (i < particleCount)
            {
                if (!lights[i].activeInHierarchy)
                {
                    lights[i].SetActive(true);
                }

                lights[i].transform.position = particles[i].position;
            }
            else
            {
                if (lights[i].activeInHierarchy)
                {
                    lights[i].SetActive(false);
                }
            }
        }
    }
}
