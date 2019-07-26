using UnityEngine;
using System.Collections;

public class ColourStars : MonoBehaviour 
{

    public ParticleSystem pSystem;

    ParticleSystem.Particle[] particles;

	// Use this for initialization
	void Start () 
    {   
        StartCoroutine(first());
	}

    void OnLevelLoaded(int level)
    {
        StartCoroutine(first());
    }
	
	// Update is called once per frame
	void Update () 
    {

	}

    public IEnumerator NewStars()
    {
        pSystem.Simulate(0.5f);

        particles = new ParticleSystem.Particle[GetComponent<ParticleSystem>().particleCount];
        
        GetComponent<ParticleSystem>().GetParticles(particles);
        
        for(int p = 0; p < particles.Length; p++)
        {
            Color col;
            
            col.r = Tools.RangeConvert(Random.value, 0, 1, TwoGradients.firstRMin, TwoGradients.firstRMax);
            col.g = Tools.RangeConvert(Random.value, 0, 1, TwoGradients.firstGMin, TwoGradients.firstGMax);

            particles[p].size = Random.Range(10, 30);
            
            particles[p].color = new Color(col.r, col.g, 0, 1);

            if(p % 100 == 0)
            {
                yield return null;
            }
        }
        
        GetComponent<ParticleSystem>().SetParticles(particles, GetComponent<ParticleSystem>().particleCount);
    }

    IEnumerator first()
    {
        while(GetComponent<ParticleSystem>().time < 0.1f)
        {
            yield return null;
        }

        NewStars();
    }
}
