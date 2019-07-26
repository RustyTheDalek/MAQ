using UnityEngine;
using System.Collections;

public class SimpleTerrain : MonoBehaviour {

    float Tiling = 20.0f;//changed from 2.5
    int Layers = 3;
    int vertNum = 0;

    Terrain terrain;

    float[,] heights;

	// Use this for initialization
	void Start () {

        terrain = GetComponent<Terrain>();
        Reset();
	}

    void OnLevelWasLoaded(int level)
    {
        terrain = GetComponent<Terrain>();
        Reset();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator GenerateHeightsCo(bool setNow)
    {
        heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
        float[,] heightsCheck = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
        //int[] tileLocation = new int[0];

        float variance = 50;

        if (Game.Current.planet != null)
        {
            variance = Game.Current.planet.mass;
            variance = Mathf.Clamp(variance, 1, 50);
        }

        float smoothGrowth = 2f;
        float y = 0.02f;
        for (int x = 1; x <= (Layers*2); x+=2)
        {
            for (int i = 0; i < terrain.terrainData.heightmapWidth; i++)
            {
                for (int k = 0; k < terrain.terrainData.heightmapHeight; k++)
                {
                    //float purlinOffsetX = 19.961f;

                    heightsCheck[i, k] = Mathf.PerlinNoise(((((float)i / ((float)terrain.terrainData.heightmapWidth*2)) * ((Tiling) / x) + variance)),//(float)terrain.transform.position.z*terrain.terrainData.heightmapResolution)+ variance),
                                                           (((float)k / ((float)terrain.terrainData.heightmapHeight*2)) * ((Tiling) / x) + variance)) / smoothGrowth;
                    if ((heightsCheck[i, k] - y) >= heights[i, k])
                    {
                        heights[i, k] = (heightsCheck[i, k] - y);
                        heights[i, k] = Mathf.Clamp(heights[i, k], 0, terrain.terrainData.heightmapHeight);
                    }
                }
                smoothGrowth *= 1.004f + (variance/20000);
                if (x == 1)
                    {
                        vertNum++;
                    }
                
                if(i % 20 == 0)
                {
                    yield return null;
                }
            }
            y*=2f;
        }


        if (setNow)
        {
            terrain.terrainData.SetHeights(0, 0, heights);
        }
    }

    public void setHeights()
    {
        terrain.terrainData.SetHeights(0, 0, heights);
    }
    public void Reset()
    {
        float[,] heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];

        for (int i = 0; i < terrain.terrainData.heightmapWidth; i++)
        {
            for (int k = 0; k < terrain.terrainData.heightmapHeight; k++)
            {
                heights[i, k] = 0;
            }

            //if (i % 20 == 0)
            //{
            //    yield return null;
            //}
        }

        terrain.terrainData.SetHeights(0, 0, heights);

        Color tCol = terrain.materialTemplate.GetColor("_WireColor");

        Color newCol = new Color(tCol.r, tCol.g, tCol.b, 1);

        terrain.materialTemplate.SetColor("_WireColor", newCol);
    }
}
