using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Game : System.Object 
{
    static Game current;

    public static Game Current
    {
        get
        {
            if(current == null)
            {
                current = new Game();
            }

            return current;
        }

        set
        {
            current = value;
        }
    }

    public List<PlanetData> savedPlanets = new List<PlanetData>();

    public AnimalVariables animal;

    public static int creationTime = 30;

    public List<GameObject> players
    {
        get
        {
            if (_Players == null)
            {
                _Players = new List<GameObject>();
            }

            return _Players;
        }
    }
    public List<GameObject> _Players;

    public int totalPlayers = 1;

    //public GameObject p1, p2, p3, p4;
    //public bool p1A, p2A, p3A, p4A;

    public PlanetData planet;

    public float volume;

    public enum DataPref
    {
        Local,
        Online,
    };

    public DataPref dataPref;

}
