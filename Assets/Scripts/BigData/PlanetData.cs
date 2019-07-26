using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlanetData : System.Object
{
    //Saved index in the array so that it can be used to calcualate averages
    public int index;
    //Unique Identifier also default name
    public string planetID;

    //Can be default name or custom name set by player
    public string planetName;

    public bool orbitsBinaryStars;

    public float expPlanetMass;

    public float mass
    {
        get
        {
            return Tools.handleExpData(expPlanetMass, SaveLoad.localAvg.lowMass,
                                               SaveLoad.localAvg.highMass, 
                                               SaveLoad.localAvg.averageMass, 20, 75, index*4);// 1 is hilly. 50 is smooth.
        }
    }

    public float expPlanetRadius;

    public float radius
    {
        get
        {
            return Tools.handleExpData(expPlanetRadius, SaveLoad.localAvg.lowRadius,
                                                 SaveLoad.localAvg.highRadius, 
                                                 SaveLoad.localAvg.averageRadius, 50, 200, index);
        }
    }

    public float expTransitDuration;

    public float transitDuration
    {
        get
        {
            return Tools.handleExpData(expTransitDuration, SaveLoad.localAvg.lowTransDur,
                                                    SaveLoad.localAvg.highTransDur, 
                                                    SaveLoad.localAvg.averageTransDur, 0.01f, 1.5f, index);
        }
    }

    public int YearOfDiscovery;
    public int numberOfMoons;
    public string ExoplantLink;

    public float expSunColR;
    public float expSunColG;
    public float expSunColB;

    public Color sunColour
    {
        get
        {
            if(SaveLoad.localAvgAvailable())
            {

                float colR = Tools.handleExpData(expSunColR, SaveLoad.localAvg.lowR, 
                                                 SaveLoad.localAvg.highR,
                                                 SaveLoad.localAvg.averageR,
                                                 TwoGradients.firstRMin,
                                                 TwoGradients.firstRMax, index)/100;

                float colG = Tools.handleExpData(expSunColG, SaveLoad.localAvg.lowG, 
                                                 SaveLoad.localAvg.highG, 
                                                 SaveLoad.localAvg.averageR,
                                                 TwoGradients.firstGMin,
                                                 TwoGradients.firstGMax, index)/100;

                float colB = Tools.handleExpData(expSunColB, SaveLoad.localAvg.lowB, 
                                                 SaveLoad.localAvg.highB,
                                                 SaveLoad.localAvg.averageR,
                                                 TwoGradients.firstBMin,
                                                 TwoGradients.firstBMax, index)/100;

                return new Color(colR, colG, colB);
            }
            else
            {
                Debug.LogError("LocalAvg not available");
                return Color.black;
            }
        }
    }

    public float expSunBrightness;

    public float sunBrightness
    {
        get
        {
            return Tools.handleExpData(expSunBrightness, SaveLoad.localAvg.lowBrightness,
                                       SaveLoad.localAvg.highBrightness, 
                                       SaveLoad.localAvg.averageBrightness, 0, 1, index);
        }
    }

    public float GalaticLongitude;
    public float GalaticLatitude;

    public float expStellarMass;

    public float steallarMass
    {
        get
        {
            return Tools.handleExpData(expStellarMass, SaveLoad.localAvg.lowStellarMass,
                                       SaveLoad.localAvg.highStellarMass,
                                       SaveLoad.localAvg.averageStellarMass, 50, 800, index);
        }
    }

    public PlanetData()
    {

    }

    public PlanetData(PlanetData original)
    {
        index = original.index;
        planetID = original.planetID;
        planetName = original.planetName;
        orbitsBinaryStars = original.orbitsBinaryStars;
        expPlanetMass = original.expPlanetMass;
        expPlanetRadius = original.expPlanetRadius;
        expTransitDuration = original.expTransitDuration;
        YearOfDiscovery = original.YearOfDiscovery;
        numberOfMoons = original.numberOfMoons;
        ExoplantLink = original.ExoplantLink;
        expSunColR = original.expSunColR;
        expSunColG = original.expSunColG;
        expSunColB = original.expSunColB;
        expSunBrightness = original.expSunBrightness;
        GalaticLongitude = original.GalaticLongitude;
        GalaticLatitude = original.GalaticLatitude;
        expStellarMass = original.expStellarMass;
    }

    public void DebugVariables()
    { 
        Debug.Log("index: " + index);

        Debug.Log("planetID: " + planetID);

        Debug.Log("planetName: " + planetName);

        Debug.Log("orbitsBinaryStars: " +orbitsBinaryStars);

        Debug.Log("expPlanetMass: " + expPlanetMass);

        Debug.Log("expPlanetRadius: " + expPlanetRadius);

        Debug.Log("expTransitDuration: " + expTransitDuration);

        Debug.Log("YearOfDiscovery: " + YearOfDiscovery);

        Debug.Log("numberOfMoons: " + numberOfMoons);

        Debug.Log("ExoplantLink: " + ExoplantLink);

        Debug.Log("expSunColR " + expSunColR);
        Debug.Log("expSunColG " + expSunColG);
        Debug.Log("expSunColG " + expSunColG);

        Debug.Log("espSunBrightness: " + expSunBrightness);

        Debug.Log("GalaticLongitude: " + GalaticLongitude);
        Debug.Log("GalaticLatitude: " + GalaticLatitude);

        Debug.Log("expStellarMass: " + expStellarMass);
    }

    public void debugConvertedVariables()
    {   
        Debug.Log("planetMass: " + mass);
        
        Debug.Log("planetRadius: " + radius);
        
        Debug.Log("transitDuration: " + transitDuration);

        Debug.Log("sunColour " + sunColour);

        Debug.Log("sunBrightness: " + sunBrightness);
    }

    public void reset()
    {
        index = 0;
        planetID = "";
        planetName = "";
        orbitsBinaryStars = false;
        expPlanetMass = 0;
        expPlanetRadius = 0;
        expTransitDuration = 0;
        YearOfDiscovery = 0;
        numberOfMoons = 0;
        ExoplantLink = "";
        expSunBrightness = 0f;
        GalaticLongitude = 0;
        GalaticLatitude = 0;
        expStellarMass = 0;
    }
}