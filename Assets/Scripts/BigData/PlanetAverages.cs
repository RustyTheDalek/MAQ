using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlanetAverages : System.Object
{
    
    #region highs and lows
    
    public float lowR = 1000000;
    public float lowG = 1000000;
    public float lowB = 1000000;
    
    public float highR = 0;
    public float highG = 0;
    public float highB = 0;
    
    public float lowRadius = 1000000;
    public float highRadius = 0;
    
    public float lowMass = 1000000;
    public float highMass = 0;
    
    public float lowTransDur = 1000000;
    public float highTransDur = 0;
    
    public float lowBrightness = 1000000;
    public float highBrightness = 0;

    public float lowStellarMass = 1000000;
    public float highStellarMass = 0;
    
    #endregion 
    
    #region averages
    
    public float averageR = 0;
    public float averageG = 0;
    public float averageB = 0;
    
    public float averageRadius = 0;
    
    public float averageMass = 0;
    
    public float averageTransDur = 0;
    
    public float averageBrightness = 0;

    public float averageStellarMass = 0;
    
    #endregion
    
    #region totals
    
    public float totalR = 0;
    public float totalG = 0;
    public float totalB = 0;
    
    public float totalRadius = 0;
    
    public float totalMass = 0;
    
    public float totalTransDur = 0;
    
    public float totalBrightness = 0;

    public float totalStellarMass = 0;
    
    #endregion
    
    
    public void debugVariables()
    {
        Debug.Log("Lowest R: " + lowR);
        Debug.Log("Highest R: " + highR);
        
        Debug.Log("Lowest G: " + lowG);
        Debug.Log("Highest G: " + highG);
        
        Debug.Log("Lowest B: " + lowB);
        Debug.Log("Highest B: " + highB);
        
        Debug.Log("Lowest Radius: " + lowRadius);
        Debug.Log("Highest Radius: " + highRadius);
        
        Debug.Log("Lowest Mass: " + lowMass);
        Debug.Log("Highest Mass: " + highMass);
        
        Debug.Log("low Transit duration: " + lowTransDur);
        Debug.Log("High Transit duration: " + highTransDur);
        
        Debug.Log("Low Brightness: " + lowBrightness);
        Debug.Log("High Brightness: " + highBrightness);

        Debug.Log("Low Stellar Mass: " + lowStellarMass);
        Debug.Log("High Stellar Mass: " + highStellarMass);
        
        Debug.Log("Average R: " + averageR);
        
        Debug.Log("Average G: " + averageG);
        
        Debug.Log("Average B: " + averageB);
        
        Debug.Log("Average Radius: " + averageRadius);
        
        Debug.Log("Average Mass: " + averageMass);
        
        Debug.Log("average Transit duration: " + averageTransDur);
        
        Debug.Log("average Brightness: " + averageBrightness);

        Debug.Log("average Stellar Mass: " + averageStellarMass);
    }
}
