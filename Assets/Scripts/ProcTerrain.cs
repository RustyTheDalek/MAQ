using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProcTerrain : MonoBehaviour {

    private Texture2D planetTexture;
//    private Texture2D normalPTexture;

    float scale = 6;

    float xStart, yStart;

    Color highCol, lowCol;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void genereateTerrain()
    {
        planetTexture = new Texture2D(1024, 512, TextureFormat.RGBA32, false);
        planetTexture.name = "Planet Texture";
        planetTexture.wrapMode = TextureWrapMode.Clamp;

        //        normalPTexture = new Texture2D(1024, 512, TextureFormat.RGBA32, false);
        //        normalPTexture.name = "Normal Texture";
        //        normalPTexture.wrapMode = TextureWrapMode.Clamp;

        GetComponent<MeshRenderer>().material.mainTexture = planetTexture;

        //TODO:Change x and y Start to use Planet generated
        //xStart = Random.Range (0.0f, 102400.0f);
        //yStart = Random.Range (0.0f, 51200.0f);

        if (Game.Current.planet != null)
        {
            int terrainType = (int)Tools.RangeConvert(Game.Current.planet.radius, 50, 150, 0, AssetManager.terrainMaterials.Count-1);

            terrainType = Mathf.Clamp(terrainType, 0, AssetManager.terrainMaterials.Count - 1);
            Debug.Log(terrainType);
            highCol = AssetManager.terrainMaterials[terrainType].GetColor("_MaxCol");
            lowCol = AssetManager.terrainMaterials[terrainType].GetColor("_MinCol");
        }
        else
        {
            highCol = Color.blue;
            lowCol = Color.red;
        }

        for (int y = 0; y < 512; y++)
        {
            for (int x = 0; x < 1024; x++)
            {
                float col = Mathf.PerlinNoise(
                    (xStart + (float)x) / 1024 * scale,
                    (yStart + (float)y) / 512 * scale);


                Color pixCol = Color.Lerp(lowCol, highCol, col);

                planetTexture.SetPixel(x, y, pixCol);
            }
        }

        planetTexture.Apply();
        //        normalPTexture.Apply();

        //Texture2D norm = planetTexture;
        renderer.material.SetTexture("_MainTex", planetTexture);
        //        renderer.material.SetTexture("_BumpMap", normalPTexture);
    }

    public IEnumerator genereateTerrainCo()
    {
        planetTexture = new Texture2D(1024, 512, TextureFormat.RGBA32, false);
        planetTexture.name = "Planet Texture";
        planetTexture.wrapMode = TextureWrapMode.Clamp;

//        normalPTexture = new Texture2D(1024, 512, TextureFormat.RGBA32, false);
//        normalPTexture.name = "Normal Texture";
//        normalPTexture.wrapMode = TextureWrapMode.Clamp;

        GetComponent<MeshRenderer>().material.mainTexture = planetTexture;

        //TODO:Change x and y Start to use Planet generated
        //xStart = Random.Range (0.0f, 102400.0f);
        //yStart = Random.Range (0.0f, 51200.0f);

        if (Game.Current.planet != null)
        {
            int terrainType = (int)Tools.RangeConvert(Game.Current.planet.radius, 50, 150, 0, AssetManager.terrainMaterials.Count);

            terrainType = Mathf.Clamp(terrainType, 0, AssetManager.terrainMaterials.Count-1);

            highCol = AssetManager.terrainMaterials[terrainType].GetColor("_MaxCol");
            lowCol = AssetManager.terrainMaterials[terrainType].GetColor("_MinCol");
        }
        else
        {
            highCol = Color.blue;
            lowCol = Color.red;
        }

        for (int y = 0; y < 512; y++) 
        {
            for (int x = 0; x < 1024; x++) 
            {
                float col = Mathf.PerlinNoise(
                    (xStart + (float)x)/1024 * scale, 
                    (yStart + (float)y)/512  * scale);


                Color pixCol = Color.Lerp(lowCol, highCol, col);

                planetTexture.SetPixel(x,y, pixCol);
            }

            if(y % 50 == 0)
            {
                yield return null;
            }
        }
        
        planetTexture.Apply();
//        normalPTexture.Apply();
        
        //Texture2D norm = planetTexture;
        renderer.material.SetTexture("_MainTex", planetTexture);
//        renderer.material.SetTexture("_BumpMap", normalPTexture);
    }
}
