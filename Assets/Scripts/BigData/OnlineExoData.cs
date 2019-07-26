using UnityEngine;
using System.Collections;
using System.IO;

public static class OnlineExoData
{
    //URL of Exoplanet Archive
    public static string URL = "http://exoplanetarchive.ipac.caltech.edu/cgi-bin/nstedAPI/nph-nstedAPI?";

    #region API variables
    //parameters fed to the API for above variables
    //Planet specific variables
    static string plName = "pl_name"; //name                                     0
    static string plCBFlag = "pl_cbflag"; //Whether it orbits binary system      1
    static string plMasse = "pl_masse"; //Mass of planet in Earth units          2
    static string plRade = "pl_rade"; //Radius of planet in Earth units          3
    static string plTranDur = "pl_trandur"; //Duration of Orbit                  4
    static string plDisc =  "pl_disc"; //Year of Discovery                       5
    static string plMNum = "pl_mnum"; //Number of Moons                          6
    static string plPELink = "pl_pelink"; // Exoplanet Link                      7
    //Star specific columns 
    static string stJmH2 = "st_jmh2"; //Colour of Star using J & H (2MASS) bands 8
    static string stHmK2 = "st_hmk2"; //Colour of Star using H & K (2MASS) bands 9
    static string stJmK2 = "st_jmk2"; //Colour of Star using J & K (2MASS) bands10
    static string stJ = "st_j"; // Brightness of Star using the J(2MASS) bands  11
    static string stGLon = "st_glon"; //Galatic Longitude in decimal degrees    12
    static string stGLat = "st_glat"; //Galatic Latitude in decimal degrees     13
    static string stMass = "st_mass"; //Mass of Star in relation to Sun         14

    const int TOTALSTATS = 15;

    #endregion
    
    //Table to retrieve webRequest from
    static string table = "&table=exoplanets";

    static string select = "&select=";

    static string where = "&where=pl_name";

    static string like = " like '";

    static WWW webRequest, nameRequest, statsRequest;
    
    static string retrievedData;

    static string[] sortedIndexData;

    static string[] sortedPlanetData;

    static int numberOfPlanets;

    public static int totalPlanets;

    public static bool PlanetBulkDownloaded = false, 
    PlanetNamesDownloaded = false, 
    planetStatsDownloaded = false,
    totalPlanetsFound = false,
    planetsAssigned = false;

    public static Ping testPing;

    public static PlanetData tempDownloadPlanet;

    /*Retrieves string of planet names and splits into an array of planet names 
    which acts as an index*/
    public static IEnumerator GetPlanetNames()
    {
        nameRequest = new WWW(URL + table + select + plName);

        yield return nameRequest;

        Debug.Log("Planet name download complete");

        retrievedData = nameRequest.text;

        Debug.Log(retrievedData);
        
        sortedIndexData = retrievedData.Split("\n"[0]);
        
        //Reduce by 1 because of first line
        numberOfPlanets = sortedIndexData.Length-1;
        
        Debug.Log("Total number of planets: " + numberOfPlanets);
        
        PlanetNamesDownloaded = true;
    }

    //Saves all Exoplanet webRequest to local copy
    public static IEnumerator GetLocalCopy()
    {
        webRequest = new WWW(URL + table + select + plName + "," + plCBFlag  + 
                             "," + plMasse + "," + plRade + "," + plTranDur + 
                             "," + plDisc + "," + plMNum + "," + plPELink + "," 
                             + stJmH2 + "," + stHmK2 + "," + stJmK2 + "," + stJ 
                             + "," + stGLon + "," + stGLat + "," + stMass);


        Debug.Log(webRequest.progress);

        yield return webRequest;

        Debug.Log("Local copy downloaded");

        retrievedData = webRequest.text;

        Debug.Log(retrievedData);

        PlanetBulkDownloaded = true;
    }

    public static IEnumerator AssignLocalCopy()
    {
        //Split data for every new line
        string[] PlanetsBulk = retrievedData.Split("\n"[0]);
        
        //temp variable to store bulk data of planet
        string planetBulk;
        
        //Temp variable to store planet data using struct
        PlanetData tempPlanet = new PlanetData();
        
        Debug.Log(PlanetsBulk.Length);
        
        /*Start at 1 as 0 would be names of data not the data itself eg pl_name
            instead of Kepler 202 b*/
        
        for(int i = 1; i < PlanetsBulk.Length - 1; i++)
        {
            planetBulk = PlanetsBulk[i];
            
            //Split data for every comma
            sortedPlanetData = planetBulk.Split(","[0]);
            
            //assign variables correctly

            tempPlanet.index = i;
            tempPlanet.planetID =  sortedPlanetData[0];
            tempPlanet.planetName = tempPlanet.planetID;
            
            tempPlanet.orbitsBinaryStars = Tools.stringToBool(
                sortedPlanetData[1]);
            
            if(!float.TryParse(sortedPlanetData[2], out tempPlanet.expPlanetMass))
            {
                tempPlanet.expPlanetMass = 0;
            }
            
            if(!float.TryParse(sortedPlanetData[3],
                               out tempPlanet.expPlanetRadius))
            {
                tempPlanet.expPlanetRadius = 0;
            }
            
            if(!float.TryParse(sortedPlanetData[4],
                               out tempPlanet.expTransitDuration))
            {
                tempPlanet.expTransitDuration = 0;
            }
            
            tempPlanet.YearOfDiscovery = int.Parse(sortedPlanetData[5]);
            
            tempPlanet.numberOfMoons = int.Parse(sortedPlanetData[6]);
            
            tempPlanet.ExoplantLink = sortedPlanetData[7];
            
            if(!float.TryParse(sortedPlanetData[8],
                               out tempPlanet.expSunColR))
            {
                tempPlanet.expSunColR = 0;
            }

            if(!float.TryParse(sortedPlanetData[9],
                               out tempPlanet.expSunColG))
            {
                tempPlanet.expSunColG = 0;
            }

            if(!float.TryParse(sortedPlanetData[10],
                               out tempPlanet.expSunColB))
            {
                tempPlanet.expSunColB = 0;
            }
            
            if(!float.TryParse(sortedPlanetData[11],
                               out tempPlanet.expSunBrightness))
            {
                tempPlanet.expSunBrightness = 0;
            }
            
            tempPlanet.GalaticLongitude = float.Parse(sortedPlanetData[12]);
            
            tempPlanet.GalaticLatitude = float.Parse(sortedPlanetData[13]);

            if(!float.TryParse(sortedPlanetData[14],
                               out tempPlanet.expStellarMass))
            {
                tempPlanet.expStellarMass = 0;
            }
            
            SaveLoad.LocalExoData.Add(new PlanetData(tempPlanet));
            
//            Debug.Log("Planet Added. Total: " + i);

            //if 100  has been proccessed
            if(i % 25 == 0)
            {
                yield return null;
            }
        }

        SaveLoad.SavePlanetDataList(ref SaveLoad.LocalExoData, "ExoPlanetData", ".txt");

        planetsAssigned = true;
    }
    
    //TODO:Calculate Colour
    static Color CalcColour(float a, float b, float c)
    {
        return Color.yellow;
    }
     
    //TODO:Calculate Brightness
    static float CalcBrightness(float a)
    {
        return 1;
    }

    public static PlanetData GetPlanetStats(string randPl)
    {
        PlanetData tempPlanet = new PlanetData();

        string escWhere = WWW.EscapeURL(like + randPl + "'");

        Debug.Log(escWhere);

        statsRequest = new WWW(URL + table + select + plCBFlag  + "," + plMasse + "," + 
                       plRade + "," + plTranDur + "," + plDisc + "," + plMNum + 
                       "," + plPELink + "," + stJmH2 + "," + stHmK2 + "," + 
                       stJmK2 + "," + stJ + "," + stGLon + "," + stGLat + "," + 
                       where + escWhere);

        Debug.Log (statsRequest.progress);

        retrievedData = statsRequest.text;

        Debug.Log(retrievedData);

        string[] temp = retrievedData.Split("\n"[0]);

        string temp2 = temp[1];

        sortedPlanetData = temp2.Split(","[0]);

        planetStatsDownloaded = true;

        //assign variables correctly
        tempPlanet.planetID =  sortedPlanetData[0];
        tempPlanet.planetName = tempPlanet.planetID;
        
        tempPlanet.orbitsBinaryStars = Tools.stringToBool(
            sortedPlanetData[1]);
        
        if(!float.TryParse(sortedPlanetData[2], out tempPlanet.expPlanetMass))
        {
            tempPlanet.expPlanetMass = 0;
        }
        
        if(!float.TryParse(sortedPlanetData[3],
                           out tempPlanet.expPlanetRadius))
        {
            tempPlanet.expPlanetRadius = 0;
        }
        
        if(!float.TryParse(sortedPlanetData[4],
                           out tempPlanet.expTransitDuration))
        {
            tempPlanet.expTransitDuration = 0;
        }
        
        tempPlanet.YearOfDiscovery = int.Parse(sortedPlanetData[5]);
        
        tempPlanet.numberOfMoons = int.Parse(sortedPlanetData[6]);
        
        tempPlanet.ExoplantLink = sortedPlanetData[7];
        
        if(!float.TryParse(sortedPlanetData[8],
                           out tempPlanet.expSunColR))
        {
            tempPlanet.expSunColR = 0;
        }
        
        if(!float.TryParse(sortedPlanetData[9],
                           out tempPlanet.expSunColG))
        {
            tempPlanet.expSunColG = 0;
        }
        
        if(!float.TryParse(sortedPlanetData[10],
                           out tempPlanet.expSunColB))
        {
            tempPlanet.expSunColB = 0;
        }
        
        if(!float.TryParse(sortedPlanetData[11],
                           out tempPlanet.expSunBrightness))
        {
            tempPlanet.expSunBrightness = 0;
        }
        
        tempPlanet.GalaticLongitude = float.Parse(sortedPlanetData[12]);
        
        tempPlanet.GalaticLatitude = float.Parse(sortedPlanetData[13]);

        if(!float.TryParse(sortedPlanetData[14],
                           out tempPlanet.expStellarMass))
        {
            tempPlanet.expStellarMass = 0;
        }

        return new PlanetData(tempPlanet);
    }

    //Picks a random planet name from downloaded list and finds it stats
    public static IEnumerator GetPlanetStats()
    {
        while(!PlanetNamesDownloaded)
        {
            yield return new WaitForSeconds(1f);
        }

        string planetName = sortedIndexData[Random.Range(0,sortedIndexData.Length-1)];

        Debug.Log(planetName);

        string escWhere = WWW.EscapeURL(like + planetName + "'");
        
//        Debug.Log(escWhere);
        
        webRequest = new WWW(URL + table + select + plCBFlag  + "," + plMasse + "," + 
                             plRade + "," + plTranDur + "," + plDisc + "," + plMNum + 
                             "," + plPELink + "," + stJmH2 + "," + stHmK2 + "," + 
                             stJmK2 + "," + stJ + "," + stGLon + "," + stGLat + "," + 
                             where + escWhere);

        yield return webRequest;

        Debug.Log("Download complete");

        retrievedData = webRequest.text;
        
        Debug.Log(retrievedData);
        
        string[] temp = retrievedData.Split("\n"[0]);
        
        string temp2 = temp[1];
        
        sortedPlanetData = temp2.Split(","[0]);

        tempDownloadPlanet = new PlanetData();

        //assign variables correctly
        tempDownloadPlanet.planetID =  planetName;
        tempDownloadPlanet.planetName = tempDownloadPlanet.planetID;
        
        tempDownloadPlanet.orbitsBinaryStars = Tools.stringToBool(
            sortedPlanetData[0]);
        
        if(!float.TryParse(sortedPlanetData[1], out tempDownloadPlanet.expPlanetMass))
        {
            tempDownloadPlanet.expPlanetMass = 0;
        }
        
        if(!float.TryParse(sortedPlanetData[2],
                           out tempDownloadPlanet.expPlanetRadius))
        {
            tempDownloadPlanet.expPlanetRadius = 0;
        }
        
        if(!float.TryParse(sortedPlanetData[3],
                           out tempDownloadPlanet.expTransitDuration))
        {
            tempDownloadPlanet.expTransitDuration = 0;
        }
        
        tempDownloadPlanet.YearOfDiscovery = int.Parse(sortedPlanetData[4]);
        
        tempDownloadPlanet.numberOfMoons = int.Parse(sortedPlanetData[5]);
        
        tempDownloadPlanet.ExoplantLink = sortedPlanetData[6];
        
        //            if(!float.TryParse(sortedPlanetData[7],
        //                               out tempDownloadPlanet.sunColourUnConverted.x) || 
        //               !float.TryParse(sortedPlanetData[8],
        //                            out tempDownloadPlanet.sunColourUnConverted.y) || 
        //               !float.TryParse(sortedPlanetData[9],
        //                            out tempDownloadPlanet.sunColourUnConverted.z))
        //            {
        //                tempDownloadPlanet.sunColourUnConverted = Vector3.zero;
        //                tempDownloadPlanet.sunColour = new Color(0,0,0);
        //            }
        
        if(!float.TryParse(sortedPlanetData[10],
                           out tempDownloadPlanet.expSunBrightness))
        {
            tempDownloadPlanet.expSunBrightness = 0;
        }

        tempDownloadPlanet.GalaticLongitude = float.Parse(sortedPlanetData[11]);
        
        tempDownloadPlanet.GalaticLatitude = float.Parse(sortedPlanetData[12]);

        if(!float.TryParse(sortedPlanetData[13],
                           out tempDownloadPlanet.expStellarMass))
        {
            tempDownloadPlanet.expStellarMass = 0;
        }

        planetStatsDownloaded = true;

    }

    public static void HandleNoData(int i)
    {
        switch(i)
        {
            case 2:
                
                break;
                
            case 3:
                
                break;

            case 4:

                break;

            case 5:

                break;

            case 6:

                break;

            case 8:

                break;

            case 9:

                break;

            case 10:

                break;

            case 11:

                break;

            case 12:

                break;

            case 13:

                break;
        }
    }

    public static IEnumerator HandleNoDataFloat()
    {
        yield return null;
    }

    public static string randomPlanet()
    {
        int rand = Random.Range(1, numberOfPlanets);
        return (sortedIndexData[rand]);
    }

    public static IEnumerator TotalPlanets()
    {
        totalPlanetsFound = false;

        webRequest = new WWW(URL + table + select + plCBFlag);

        yield return webRequest;

        retrievedData = webRequest.text;

        sortedIndexData = retrievedData.Split("\n"[0]);
        
        //Reduce by 1 because of first line
        numberOfPlanets = sortedIndexData.Length-1;

        //Debug.Log("Number of planets: " + numberOfPlanets);
        
        totalPlanets = numberOfPlanets;

        totalPlanetsFound = true;
    }

    //TODO:Actually check for Network Access
    public static bool HaveNetworkAccess()
    {
        return true;
    }
}
