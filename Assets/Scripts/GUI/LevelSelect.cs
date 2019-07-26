using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelect : MonoBehaviour {
   
    public GameObject sun;
    public ColourStars stars;
    public GameObject planet;
    public ExoDataManager EDManager;

    public ParticleSystem WarpSpeed;
    public AudioSource wSpeedSound;

    // set GUI items
    public Button confirmTestBtn, findPlanetbtn;

    public SimpleTerrain previewTerrain;
    public PlanetStatus pStatus;

    public int menuA = 0;

    protected int toggleMenu = 0;

    public bool tutStep4;

    public Text  planetName, yearOfDisc, GalaLong, GalaLat, PlanetMass, PlanetRadius, TransitDur, StarMass, StarBrightness, errorMsg;
    //Whether a new level can be selected
    bool canSelect = true, planetFound = false;

    //Starting postions of sun and Planet
    Vector3 sunPos, planetPos, starsPos;

    int totalMoving;

	// Use this for initialization
	void Start () {

        errorMsg.gameObject.SetActive(false);

        sunPos = sun.transform.position;
        planetPos = planet.transform.position;
        starsPos = stars.transform.position;

//        lvlSelectMenu.alpha = (0f);
//        lvlSelectMenu.interactable = false;
//
//        lvlSelectInfo.alpha = (0f);
//        lvlSelectInfo.interactable = false;
	}

    void OnLevelWasLoaded(int level)
    {
        if (Game.Current.planet != null)
        {
            planet.SetActive(true);
            sun.SetActive(true);

            StartCoroutine(previewTerrain.GenerateHeightsCo(true));
            planet.GetComponent<ProcTerrain>().genereateTerrain();

            //find radius of planet resize planet and clamp size within 75 units
            float r = Game.Current.planet.radius;
            planet.transform.localScale = Vector3.ClampMagnitude(new Vector3(r, r, r), 75);

            sun.GetComponent<Sun>().setSuns();

            planet.GetComponent<RotateAnimal>().direction = new Vector3(0, Game.Current.planet.transitDuration, 0);

            //Resize sun and keep it clamped within 200 units
            sun.transform.localScale = Vector3.ClampMagnitude(Vector3.one * Game.Current.planet.steallarMass, 800);

            if (!stars.gameObject.activeSelf)
            {
                stars.gameObject.SetActive(true);
            }

            pStatus.updateTerrainStatus();

            planetName.text = Game.Current.planet.planetName;
            yearOfDisc.text = "Year of Discovery:" + Game.Current.planet.YearOfDiscovery;
            GalaLong.text = "Galatic Longtitude: " + Game.Current.planet.GalaticLongitude.ToString("F2");
            GalaLat.text = "Galatic Latitude: " + Game.Current.planet.GalaticLatitude.ToString("F2");
            PlanetMass.text = "Planet Mass: " + Game.Current.planet.mass.ToString("F2");
            PlanetRadius.text = "Planet Radius: " + Game.Current.planet.radius.ToString("F2");
            TransitDur.text = "Transit Duration: " + Game.Current.planet.transitDuration.ToString("F2");
            StarMass.text = "Star Mass: " + Game.Current.planet.steallarMass.ToString("F2");
            StarBrightness.text = "Star Brightness: " + Game.Current.planet.sunBrightness.ToString("F2");
        }
    }
	
    public void OnClickLvlSelect()
    {
        if (toggleMenu == 0)
        {
//            lvlSelectMenu.alpha = (1f);
//            lvlSelectMenu.interactable = true;
//
//            lvlSelectInfo.alpha = (1f);
//            lvlSelectInfo.interactable = true;
//
//            Screen.showCursor = true;

            menuA = 1;

            toggleMenu = 1;

            tutStep4 = true;
            
            NextLevel();
        } 
    }

    public void NextLevel()
    {
        if(canSelect && !planetFound)
        {
            errorMsg.gameObject.SetActive(false);
            confirmTestBtn.interactable = false;
            findPlanetbtn.interactable = false;

            wSpeedSound.Play();
            canSelect = false;
            totalMoving = 0;

            WarpSpeed.Play();
            sun.GetComponent<Sun>().worldLight.color = Color.white;
            stars.gameObject.SetActive(false);

            StartCoroutine(moveLeft(planet, planetPos, Vector3.right * 4800));
            StartCoroutine(moveLeft(sun, sunPos, Vector3.right * 4000));
        }
    }

    IEnumerator moveLeft(GameObject obj, Vector3 oPos, Vector3 displacement)
    {
        totalMoving++;

        if (obj.name == "Planet")
        {
            EDManager.getPlanet();
            StartCoroutine(previewTerrain.GenerateHeightsCo(false));
            StartCoroutine(planet.GetComponent<ProcTerrain>().genereateTerrainCo());
            StartCoroutine(stars.NewStars());

            //find radius of planet resize planet and clamp size within 75 units
            float r = Game.Current.planet.radius;
            planet.transform.localScale = Vector3.ClampMagnitude(new Vector3(r, r, r), 150);
            sun.GetComponent<Sun>().setSuns(); 
        }

        while(Vector3.SqrMagnitude(obj.transform.position - (oPos + displacement)) > 25)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, oPos + displacement, 0.1f);
            yield return null;
        }

        obj.transform.position = Tools.setVector3(obj.transform.position, "x", 
                                                  oPos.x - displacement.x);
        if(obj.name == "Planet")
        {

            planetName.text = Game.Current.planet.planetName;
            yearOfDisc.text = "Year of Discovery:" + Game.Current.planet.YearOfDiscovery;
            GalaLong.text = "Galatic Longtitude: " + Game.Current.planet.GalaticLongitude.ToString("F2");
            GalaLat.text = "Galatic Latitude: " + Game.Current.planet.GalaticLatitude.ToString("F2");
            PlanetMass.text = "Planet Mass: " + Game.Current.planet.mass.ToString("F2");
            PlanetRadius.text = "Planet Radius: " + Game.Current.planet.radius.ToString("F2");
            TransitDur.text = "Transit Duration: " + Game.Current.planet.transitDuration.ToString("F2");
            StarMass.text = "Star Mass: " + Game.Current.planet.steallarMass.ToString("F2");
            StarBrightness.text = "Star Brightness: " + Game.Current.planet.sunBrightness.ToString("F2");

            planet.GetComponent<RotateAnimal>().direction = new Vector3(0,Game.Current.planet.transitDuration,0);
            previewTerrain.setHeights();

            pStatus.updateTerrainStatus();

            //Debug.Log(Game.Current.planet.steallarMass);

            //Resize sun and keep it clamped within 200 units
            sun.transform.localScale = Vector3.ClampMagnitude(Vector3.one * Game.Current.planet.steallarMass, 800);

            if (!stars.gameObject.activeSelf)
            {
                stars.gameObject.SetActive(true);
            }
        }

        if (!planet.activeSelf)
            planet.SetActive(true);

        if(!sun.activeSelf)
            sun.SetActive(true);
        
        while(Vector3.SqrMagnitude(obj.transform.position - oPos) > 1)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, oPos, 0.05f);
            yield return null;
        }

        obj.transform.position = oPos;

        totalMoving --;

        if(totalMoving == 0)
        {
            canSelect = true;
            confirmTestBtn.interactable = true;
            findPlanetbtn.interactable = true;
        }
    }

    public void playPlanet()
    {
        if (canSelect && !planetFound)
        {
            if (planetName.text != "")
            {
                errorMsg.gameObject.SetActive(true);
                if (Game.Current.players.Count >= 1)
                {
                    foreach (GameObject player in Game.Current.players)
                    {
                        DontDestroyOnLoad(player);
                    }
                    Application.LoadLevel("Terrain2");
                }
                else
                {
                    errorMsg.text = "No Animals to Test";
                }
            }
            else
            {
                errorMsg.gameObject.SetActive(true);
                errorMsg.text = "No planet!";
            }
        }
        else if(planetFound)
        {
            findPlanetbtn.interactable = true;
            errorMsg.gameObject.SetActive(false);
            planetFound = false;
        }
    }
}
