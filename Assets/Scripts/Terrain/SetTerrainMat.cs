using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetTerrainMat : MonoBehaviour {

    public List<Terrain> terrain;

    int WhatMat;

	// Use this for initialization
	void Start () {
        //Game.Current.planet.expPlanetRadius;
        if (Game.Current.planet != null)
            WhatMat = (int)Tools.RangeConvert(Game.Current.planet.radius, 50, 150, 0, AssetManager.terrainMaterials.Count - 1);
        else
            WhatMat = 0;

        WhatMat = Mathf.Clamp(WhatMat, 0, AssetManager.terrainMaterials.Count - 1);
        Debug.Log(WhatMat);
        for (int x = 0; x < 5; x++)
        {
            terrain[x].materialTemplate = AssetManager.terrainMaterials[WhatMat];
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
