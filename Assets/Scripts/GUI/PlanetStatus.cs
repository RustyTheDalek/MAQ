using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlanetStatus : MonoBehaviour {

    Text tStatusTxt, difficultyTxt;

    TerrainStatus tStatus = TerrainStatus.Null;
    Difficulty diff = Difficulty.Null;

    Dictionary<TerrainStatus, Difficulty> terrainDiffStatus;

	// Use this for initialization
	void Start () {

        setup();	
	}

    void OnLevelWasLoaded(int level)
    {
        setup();
    }

    void setup()
    {
        Text[] texts = GetComponentsInChildren<Text>();

        foreach (Text text in texts)
        {
            switch (text.name)
            {
                case "TerrainTypeOutput":
                    tStatusTxt = text;
                    break;

                case "DifficultyOutput":
                    difficultyTxt = text;
                    break;
            }
        }

        terrainDiffStatus = new Dictionary<TerrainStatus, Difficulty>();

        //Add statuses of terrains and appropriate list 
        terrainDiffStatus.Add(TerrainStatus.ManyAMountain, Difficulty.Suicide);
        terrainDiffStatus.Add(TerrainStatus.SillyNHilly, Difficulty.VeryDifficult);
        terrainDiffStatus.Add(TerrainStatus.GreenHillZone, Difficulty.Difficult);
        terrainDiffStatus.Add(TerrainStatus.NotSoHilly, Difficulty.Average);
        terrainDiffStatus.Add(TerrainStatus.FranklyFlat, Difficulty.Easy);
        terrainDiffStatus.Add(TerrainStatus.PoolTable, Difficulty.VeryEasy);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updateTerrainStatus()
    {
        //Changing terrain status text based on mass
        float mass = Game.Current.planet.mass;

        if (mass <= 5)
        {
            tStatus = TerrainStatus.ManyAMountain;

            setText();
        }
        else if (mass > 5 && mass <= 15)
        {
            tStatus = TerrainStatus.SillyNHilly;

            setText();
        }
        else if (mass > 15 && mass <= 25)
        {
            tStatus = TerrainStatus.GreenHillZone;

            setText();
        }
        else if (mass > 25 && mass <= 40)
        {
            tStatus = TerrainStatus.NotSoHilly;

            tStatusTxt.text = tStatus.ToString();

            setText();
        }
        else if (mass > 40 && mass <= 46)
        {
            tStatus = TerrainStatus.FranklyFlat;

            setText();
        }
        else
        {
            tStatus = TerrainStatus.PoolTable;

            setText();
        }
    }

    void setText()
    {
        tStatusTxt.text = Tools.textSplit(tStatus.ToString());

        if (terrainDiffStatus.TryGetValue(tStatus, out diff))
        {
            difficultyTxt.text = Tools.textSplit(diff.ToString());
        }
    }
}
