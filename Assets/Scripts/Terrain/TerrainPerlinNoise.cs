using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TerrainPerlinNoise : MonoBehaviour {
    
    float Tiling = 20.0f;
    int Layers = 3;
    int vertNum = 0;

    public Image loadingBar;
   
    public Terrain terrain;

    public CanvasGroup loadingScreen;

    void Start()
    {
        StartCoroutine(GenerateHeightsCo(Tiling, Layers));
    }

    public IEnumerator GenerateHeightsCo(float tileSize, int Layers)
    {
        float[,] heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
        float[,] heightsCheck = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];

        float variance = 40;

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
                //Debug.Log("Width: "+terrain.terrainData.heightmapWidth);
                //Debug.Log("Height: "+terrain.terrainData.heightmapHeight);
                for (int k = 0; k < terrain.terrainData.heightmapHeight; k++)
                {
                    float purlinOffsetX=0;
                    if (terrain.transform.position.x == 0){
						purlinOffsetX = 19.961f;
					}else if(terrain.transform.position.x < 0){
                        purlinOffsetX = 39.922f;
					}else if(terrain.transform.position.x > 0){
                        purlinOffsetX = 0;
					}



					heightsCheck[i, k] = Mathf.PerlinNoise(((((float)i / ((float)terrain.terrainData.heightmapWidth)) * ((tileSize) / x) + variance)),//(float)terrain.transform.position.z*terrain.terrainData.heightmapResolution)+ variance),
                                                           (((float)k / ((float)terrain.terrainData.heightmapHeight)) * ((tileSize) / x) + variance) + purlinOffsetX) / smoothGrowth;
                    if ((heightsCheck[i, k] - y) >= heights[i, k])
                    {
                        heights[i, k] = (heightsCheck[i, k] - y);
                    }
                    //513
                    // 3947535 + 789507
                    //loadingBar.fillAmount += 1.0f/3158028.0f;
                    loadingBar.fillAmount += 1.0f / 4737042f;
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

        if (terrain.transform.position.z == 0 )
        {
            //mirrors the terrain 
            int o = vertNum - 1;
            for (int i=0; i < (heights.GetLength(0))/2; i++)
            {
                int p = vertNum-1;
                for (int k= 0;k <= heights.GetLength(1)-1; k++)
                {
                    float temp;
                    temp = heights[i,k];
                    heights[i,k] = heights[o,p];
                    heights[o,p] = temp;
                    p--;
                }
                o--;
            }
        }

		if (terrain.transform.position.z != 0 )
		{
			//mirrors the terrain 
			//int o = vertNum - 1;
			int o = 0;
			for (int i=0; i < (heights.GetLength(0)); i++)
			{
				int p = vertNum-1;
				//int p = 0;
				for (int k= 0;k < heights.GetLength(1)/2; k++)
				{
					float temp;
					temp = heights[i,k];
					heights[i,k] = heights[o,p];
					heights[o,p] = temp;
					p--;
				}
				o++;
			}
		}
        terrain.terrainData.SetHeights(0, 0, heights);
        if (terrain.GetComponent<ObjectSpawner>())
        {
            terrain.GetComponent<ObjectSpawner>().spawnObjs();
        }
    }
}

//note: cannot change width, length and hight at runtime.