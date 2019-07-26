using UnityEngine;
using System.Collections;

public class ExoDataManager : MonoBehaviour 
{
    
    public PlanetData currentPlanet;
    
    bool currentPlanetActive = false;

    static bool analysedData = false;

    // Use this for initialization
    void Start () 
    {

        /*Checks if there's a copy of the local data and whether the data is up 
            to date*/
        LocalExoDataCheck();

        /*Same as before but regarding a save*/
//        LocalSaveDataCheck();
    }
    
    // Update is called once per frame
    void Update () 
    {
//        if(Input.GetKeyDown(KeyCode.D))
//        {
//            Debug.Log("Starting Download");
//            
//            //StartCoroutine(GetPlanetNames());
//            
//            StartCoroutine(OnlineExoData.GetLocalCopy());
//        }
//        
//        if(Input.GetKeyDown(KeyCode.Space))
//        {
//            
//            //            StartCoroutine(OnlineExoData.AssignLocalCopy());
//            
//            OnlineExoData.AssignLocalCopy();
//            
//            //            getPlanet();
//            //
//            //            Debug.Log(Game.Current.dataPref);
//            
//        }
//        
//        if(Input.GetKeyDown(KeyCode.S))
//        {
//            Debug.Log("S pressed");
//            
//            SaveLoad.SavePlanetDataList(ref SaveLoad.LocalExoData, "ExoPlanetData", ".txt");
//            
//        }
//
//        if(Input.GetKeyDown(KeyCode.M))
//        {
//            Debug.Log("M pressed");
//            
//            SaveLoad.SaveGame();
//
//        }
//
//        if(Input.GetKeyDown(KeyCode.N))
//        {
//            Debug.Log("N pressed");
//            
//            SaveLoad.LoadGame();
//
//            Debug.Log(Game.Current.dataPref);
//            
//        }
//        
//        if(Input.GetKeyDown(KeyCode.L))
//        {
//            Debug.Log("L pressed");
//            
//            SaveLoad.LoadPlanetDataList(ref SaveLoad.LocalExoData, "ExoPlanetData", ".txt");
//            
//            SaveLoad.LocalExoData[0].DebugVariables();
//            
//            if(SaveLoad.LocalDataAvailable())
//            {
//                Debug.Log(SaveLoad.LocalExoData.Count);
//                
//                AnaylseData();
//            }
//        }
//        
//        if(Input.GetKeyDown(KeyCode.Alpha1))
//        {
//            SaveLoad.SavePlanetAverages(ref SaveLoad.localAvg, "ExoStats", ".txt");
//            
//            SaveLoad.localAvg.debugVariables();
//        }
//        
//        if(Input.GetKeyDown(KeyCode.Alpha2))
//        {
//            if(SaveLoad.LoadPlanetAverages(ref SaveLoad.localAvg, "ExoStats", ".txt"))
//            {
//                SaveLoad.localAvg.debugVariables();
//            }
//            else
//            {
//                Debug.LogError("Could not load file");
//            }
//            
//        }
//
//        if(Input.GetKeyDown(KeyCode.C))
//        {
//            //HD 152079 b
//            SaveLoad.LocalExoData[0].debugConvertedVariables();
//        }
    }
    
    public void LocalExoDataCheck()
    {
        //Check if a local copy exists?
        if(SaveLoad.LocalDataExists("ExoPlanetData", ".txt") && SaveLoad.LocalExoData != null)
        {
            //Debug.Log("Local file exists");
            
            //Load said data
            SaveLoad.LoadPlanetDataList(ref SaveLoad.LocalExoData, 
                                        "ExoPlanetData", ".txt");

            //See if file needs updating
            if(OnlineExoData.HaveNetworkAccess())
            {
                StartCoroutine(LocalDataOutOfDate());
            }
        }
        else
        {
            //Debug.Log("No Local File, downloading local copy");
            StartCoroutine(updateLocalData());
        }
    }

    public IEnumerator LocalDataOutOfDate()
    {
        StartCoroutine(OnlineExoData.TotalPlanets());

        if(!OnlineExoData.totalPlanetsFound)
        {
            yield return null;
        }

        if(SaveLoad.LocalExoData.Count < OnlineExoData.totalPlanets -1)
        {
            //Debug.Log("Local Data out of date");
            StartCoroutine(updateLocalData());
        }
        else
        {
            //Debug.Log("Local Data up to date");
            if(SaveLoad.LocalDataExists("ExoStats", ".txt"))
            {
                //Debug.Log("Local Averages exists");
                
                SaveLoad.LoadPlanetAverages(ref SaveLoad.localAvg, "ExoStats", ".txt");

            }
            else //Analyse loaded data in case ExoStats doesn't exist
            {
                StartCoroutine(AnaylseData());
            }
        }
    }

    public void LocalSaveDataCheck()
    {
        //Check if a local copy exists?
        if(SaveLoad.LocalDataExists("save", ".sg"))
        {
            //Debug.Log("Local Save found");
            
            SaveLoad.LoadGame();

            //Debug.Log(Game.Current.dataPref);
        }
        else //Otherwise create a new save game
        {
            //Debug.Log("No save found");
        }
    }
    
    //TODO:Handle no data
    public void getPlanet()
    {
//        switch(Game.Current.dataPref)
//        {
            //case Game.DataPref.Local:
                
                Game.Current.planet = getLocalPlanet();
//                
//                currentPlanet.DebugVariables();
//                
//                break;
//                
//            case Game.DataPref.Online:
//                
//                StartCoroutine(getOnlinePlanet());
//                
//                break;
//                
//            default:
//                
//                currentPlanet = null;
//                
//                Debug.LogError("dataPref not set");
//                
//                break;
 //       }
    }
    
    public static PlanetData getLocalPlanet()
    {
        return SaveLoad.LocalExoData[Random.Range(0,SaveLoad.LocalExoData.Count)];
    }
    
    public IEnumerator getOnlinePlanet()
    {
        Debug.Log("Getting online planets");
        
        PlanetData temp = new PlanetData();
        
        StartCoroutine(OnlineExoData.GetPlanetNames());
        
        StartCoroutine(OnlineExoData.GetPlanetStats());
        
        while(!OnlineExoData.planetStatsDownloaded)
        {
            yield return null;
        }
        
        Debug.Log("Stats Downloaded");
        currentPlanet = OnlineExoData.tempDownloadPlanet;
        
        currentPlanet.DebugVariables();
        
    }

    public IEnumerator updateLocalData()
    {
        StartCoroutine(OnlineExoData.GetLocalCopy());

        while(!OnlineExoData.PlanetBulkDownloaded)
        {
            yield return null;
        }

        StartCoroutine(OnlineExoData.AssignLocalCopy());

        while(!OnlineExoData.planetsAssigned)
        {
            yield return null;
        }

        StartCoroutine(AnaylseData());

        while(!analysedData)
        {
            yield return null;
        }
    }
    
    public void downloadPlanetNames()
    {
        StartCoroutine(OnlineExoData.GetPlanetNames());
    }
    
    public void setLocalPref()
    {
        Game.Current.dataPref = Game.DataPref.Local;
    }
    
    public void setOnlinePref()
    {
        Game.Current.dataPref = Game.DataPref.Online;
    }
      
    public static IEnumerator AnaylseData()
    {
        Debug.Log("Starting Anal");

        Debug.Log(SaveLoad.LocalExoData.Count);
        for(int i = 0 ; i < SaveLoad.LocalExoData.Count; i++)
        {
            highsAndLows(i);
            
            averages(i);

            if(i % 25 == 0)
            {
                yield return null;
            }

            Debug.Log(i);
        }

        Debug.Log("Ending Anal");

        
        divideByTotal();

        SaveLoad.localAvg.debugVariables();

        SaveLoad.SavePlanetAverages(ref SaveLoad.localAvg, "ExoStats", ".txt");

        analysedData = true;
    }
    
    //Searches for the highest and lowest value in a list
    static void highsAndLows(int i)
    {
        //Start of list
        SaveLoad.localAvg.lowR = Tools.lowCheck(SaveLoad.LocalExoData[i].expSunColR, SaveLoad.localAvg.lowR);
        SaveLoad.localAvg.highR = Tools.highCheck(SaveLoad.LocalExoData[i].expSunColR, SaveLoad.localAvg.highR);
        
        SaveLoad.localAvg.lowG = Tools.lowCheck(SaveLoad.LocalExoData[i].expSunColG, SaveLoad.localAvg.lowG);
        SaveLoad.localAvg.highG = Tools.highCheck(SaveLoad.LocalExoData[i].expSunColG, SaveLoad.localAvg.highG);
        
        SaveLoad.localAvg.lowB = Tools.lowCheck(SaveLoad.LocalExoData[i].expSunColB, SaveLoad.localAvg.lowB);
        SaveLoad.localAvg.highB = Tools.highCheck(SaveLoad.LocalExoData[i].expSunColB, SaveLoad.localAvg.highB);
        
        SaveLoad.localAvg.lowMass = Tools.lowCheck(SaveLoad.LocalExoData[i].expPlanetMass, SaveLoad.localAvg.lowMass);
        SaveLoad.localAvg.highMass  = Tools.highCheck(SaveLoad.LocalExoData[i].expPlanetMass, SaveLoad.localAvg.highMass);
        
        SaveLoad.localAvg.lowRadius = Tools.lowCheck(SaveLoad.LocalExoData[i].expPlanetRadius, SaveLoad.localAvg.lowRadius);
        SaveLoad.localAvg.highRadius  = Tools.highCheck(SaveLoad.LocalExoData[i].expPlanetRadius, SaveLoad.localAvg.highRadius);
        
        SaveLoad.localAvg.lowTransDur = Tools.lowCheck(SaveLoad.LocalExoData[i].expTransitDuration, SaveLoad.localAvg.lowTransDur);
        SaveLoad.localAvg.highTransDur  = Tools.highCheck(SaveLoad.LocalExoData[i].expTransitDuration, SaveLoad.localAvg.highTransDur);
        
        SaveLoad.localAvg.lowBrightness = Tools.lowCheck(SaveLoad.LocalExoData[i].expSunBrightness, SaveLoad.localAvg.lowBrightness);
        SaveLoad.localAvg.highBrightness = Tools.highCheck(SaveLoad.LocalExoData[i].expSunBrightness, SaveLoad.localAvg.highBrightness);

        SaveLoad.localAvg.lowStellarMass = Tools.lowCheck(SaveLoad.LocalExoData[i].expStellarMass, SaveLoad.localAvg.lowStellarMass);
        SaveLoad.localAvg.highStellarMass = Tools.highCheck(SaveLoad.LocalExoData[i].expStellarMass, SaveLoad.localAvg.highStellarMass);

    }
    
    //Finds average value for each planet variable
    static void averages(int i)
    {

        mean(SaveLoad.LocalExoData[i].expSunColR,ref SaveLoad.localAvg.averageR, ref SaveLoad.localAvg.totalR);
        
        mean(SaveLoad.LocalExoData[i].expSunColG, ref SaveLoad.localAvg.averageG, ref SaveLoad.localAvg.totalG);
        
        mean(SaveLoad.LocalExoData[i].expSunColB, ref SaveLoad.localAvg.averageB, ref SaveLoad.localAvg.totalB);
        
        mean(SaveLoad.LocalExoData[i].expPlanetMass, ref SaveLoad.localAvg.averageMass, ref SaveLoad.localAvg.totalMass);
        
        mean(SaveLoad.LocalExoData[i].expPlanetRadius, ref SaveLoad.localAvg.averageRadius, ref SaveLoad.localAvg.totalRadius);
        
        mean(SaveLoad.LocalExoData[i].expTransitDuration, ref SaveLoad.localAvg.averageTransDur, ref SaveLoad.localAvg.totalTransDur);
        
        mean(SaveLoad.LocalExoData[i].expSunBrightness, ref SaveLoad.localAvg.averageBrightness, ref SaveLoad.localAvg.totalBrightness);

        mean(SaveLoad.LocalExoData[i].expStellarMass, ref SaveLoad.localAvg.averageStellarMass, ref SaveLoad.localAvg.totalStellarMass);
        
    }
    
    static void mean(float val, ref float avg, ref float total)
    {
        if(val != 0)
        {
            avg +=val;
            total ++;
        }
    }
    
    static void divideByTotal()
    {
        SaveLoad.localAvg.averageR /= SaveLoad.localAvg.totalR;
        SaveLoad.localAvg.averageG /= SaveLoad.localAvg.totalG;
        SaveLoad.localAvg.averageB /= SaveLoad.localAvg.totalB;
        
        SaveLoad.localAvg.averageMass /= SaveLoad.localAvg.totalMass;
        SaveLoad.localAvg.averageRadius /= SaveLoad.localAvg.totalMass;
        SaveLoad.localAvg.averageTransDur /= SaveLoad.localAvg.totalTransDur;
        SaveLoad.localAvg.averageBrightness /= SaveLoad.localAvg.totalBrightness;
        SaveLoad.localAvg.averageStellarMass /= SaveLoad.localAvg.totalStellarMass;
    }
}
