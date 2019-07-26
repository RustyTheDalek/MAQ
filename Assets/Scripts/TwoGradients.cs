using UnityEngine;
using System.Collections;

public static class TwoGradients 
{
    //First gradient Yellow to pink
    public static float firstRMin = 82;
    public static float firstRMax = 100;

    public static float firstGMin = 0;
    public static float firstGMax = 92;

    public static float firstBMin = 58;
    public static float firstBMax = 4;

    //Second gradient Green to Blue
    public static float secondRMin = 0;
    public static float secondRMax = 0;
    
    public static float secondGMin = 100;
    public static float secondGMax = 0;
    
    public static float secondBMin = 22;
    public static float secondBMax = 70;

    public static Color firstGMinCol
    {
        get
        {
            return new Color(firstRMin, firstGMin, firstBMin, 100) * 2.5f;
        }
    }

    public static Color firstGMaxCol
    {
        get
        {
            return new Color(firstRMax, firstGMax, firstBMax,  100) * 2.5f;
        }
    }

    public static Color secondGMinCol
    {
        get
        {
            return new Color(secondRMin, secondGMin, secondBMin,  100) * 2.5f;
        }
    }
    
    public static Color secondGMaxCol
    {
        get
        {
            return new Color(secondRMax, secondGMax, secondBMax,  100) * 2.5f;
        }
    }
}
