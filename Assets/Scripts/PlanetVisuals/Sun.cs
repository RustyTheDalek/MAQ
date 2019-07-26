using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour {

    const float MINSIZE = 15, MAXSIZE = 250;

    public Light halo;
    public Light worldLight;
    public ParticleSystem Stars;
    public GameObject Binary;
    public ParticleSystem sunParts, binarySunParts;
    public bool display;

    public GameObject posRef;

    [Range(0.01f, 1.5f) ]
    public float speed = 1.0f;

    public float offset;

    //Brightness at Noon
    float noonLight = 0.5f;

    ParticleSystem.Particle[] particles;

    float a = 0;

    //TODO:change max min brightness - 2.35, 0.27
    int rand;

    float trans = Tools.handleExpData(0.0145f, 0.0145f,
                                      0.7472f, 
                                      0.163283f, 0.01f, 1.5f, 0);

    float midR = Tools.handleExpData(0.3857021f, -0.237f, 
                                     1.357f,
                                     0.3857021f,
                                     TwoGradients.firstRMin,
                                     TwoGradients.firstRMax, 0)/100;

    float midG = Tools.handleExpData(0.09364443f, -0.906f, 
                                     0.773f,
                                     0.09364443f,
                                    TwoGradients.firstGMin,
                                    TwoGradients.firstGMax, 0)/100;

    float midB = Tools.handleExpData(0.4791879f, -0.249f, 
                                     1.971f,
                                     0.4791879f,
                                     TwoGradients.firstBMin,
                                     TwoGradients.firstBMax, 0)/100;

	// Use this for initialization
	void Start () {

        if(!display)
        {
            Stars = GetComponentInChildren<ParticleSystem>();
        }

        if(Binary.activeSelf && Binary.light)
        {
            Binary.renderer.material.color = this.renderer.material.color;
            Binary.light.color = this.renderer.material.color;
        }
	}

    //Value set if loaded from another level
    void OnLevelWasLoaded(int level)
    {
//        setSuns();
    }
	
	// Update is called once per frame
	void Update () 
    {
	if(display)
	{
        	worldLight.transform.LookAt(posRef.transform.position);
	}

//        if(!display)
//        {
//            float time = (Time.timeSinceLevelLoad + offset)*trans;
//
//            transform.position = new Vector3((Mathf.Cos(time)) * -400, Mathf.Sin(time) * 400) + 
//                new Vector3(posRef.transform.position.x, 0,posRef.transform.position.z);
//
//            GetComponent<Light>().intensity = Mathf.Lerp(0, noonLight, Mathf.Max(0.0f, Mathf.Sin(time))) * 2;
//
//            worldLight.intensity = Mathf.Lerp(0, noonLight, Mathf.Max(0.0f, Mathf.Sin(time))); 
//
//            transform.position = new Vector3(transform.position.x, transform.position.y, posRef.transform.position.z);
//
//            Stars.transform.position = new Vector3(posRef.transform.position.x, 0, posRef.transform.position.z);
//            Stars.transform.eulerAngles = new Vector3(0,180,Time.timeSinceLevelLoad * trans*90);
//
//            if(transform.position.y >= 0)
//            {
//                particles = new ParticleSystem.Particle[Stars.particleCount];
//                
//                Stars.GetParticles(particles);
//                
//                a = Mathf.Lerp(1, 0, transform.position.y/50);
//                
//                for(int p = 0; p < particles.Length; p++)
//                {
//
//                    Color col = particles[p].color;
//                    
//                    particles[p].color = new Color(col.r, col.g, col.b, a);
//                }
//
//                Stars.SetParticles(particles, Stars.particleCount);
//            }
//            else
//            {
//
//                particles = new ParticleSystem.Particle[Stars.particleCount];
//
//                Stars.GetParticles(particles);
//
//                a = Mathf.Lerp(0, 1, Mathf.Abs(transform.position.y)/50);
//                    
//                for(int p = 0; p < particles.Length; p++)
//                {
//                    Color col = particles[p].color;
//
//                    particles[p].color = new Color(col.r, col.g, col.b, a);
//                }
//
//                Stars.SetParticles(particles, Stars.particleCount);
//            }
//
//
//        }
	}

    public void setSuns()
    {
        Color col = Game.Current.planet.sunColour;
        renderer.material.color = col;

        if(!display)
        {
            halo.color = col;
            halo.range = Game.Current.planet.sunBrightness*20;
        }
        else
        {
            
            sunParts.startColor = col;
            sunParts.renderer.material.SetColor("_TintColor", new Color(col.r, col.g, col.b, .03f));
            sunParts.startSize = Mathf.Lerp(MINSIZE, MAXSIZE, Tools.RangeConvert(Game.Current.planet.steallarMass, 50, 800, 0, 1));
            sunParts.Play();
        }

        worldLight.color = col;
        
//        if(Game.Current.planet.orbitsBinaryStars)
//        {
//            Binary.SetActive(true);
//
//            Binary.renderer.material.color = col;
//            Binary.GetComponent<Light>().color = col;
//            Binary.GetComponent<Light>().range = Game.Current.planet.sunBrightness*10;
//
//            sunParts.startColor = col;
//            Debug.Break();
//        }
//        else
//        {
////            Debug.Log("No Binary :(");
//            Binary.SetActive(false);
//        }
    }

}
