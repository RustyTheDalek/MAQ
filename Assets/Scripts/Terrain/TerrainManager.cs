using UnityEngine;
using System.Collections;

public class TerrainManager : MonoBehaviour
{
    public GameObject PlayerObject;
    
    private Terrain[] terrainList = new Terrain[3];
    
    void Start ()
    {
        Terrain linkedTerrain = gameObject.GetComponent<Terrain>();
        
        terrainList[0] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();
        terrainList[1] = linkedTerrain;
        terrainList[2] = Terrain.CreateTerrainGameObject(linkedTerrain.terrainData).GetComponent<Terrain>();

        terrainList[0].materialTemplate = linkedTerrain.materialTemplate;
        terrainList[2].materialTemplate = linkedTerrain.materialTemplate;

        terrainList[0].gameObject.AddComponent<TerrainPerlinNoise>();
        terrainList[2].gameObject.AddComponent<TerrainPerlinNoise>();

        terrainList[0].gameObject.GetComponent<TerrainPerlinNoise>().terrain =  terrainList[0].gameObject.GetComponent<Terrain>();
        terrainList[2].gameObject.GetComponent<TerrainPerlinNoise>().terrain =  terrainList[2].gameObject.GetComponent<Terrain>();
        
        UpdateTerrainPositionsAndNeighbors();
    }
    
    private void UpdateTerrainPositionsAndNeighbors()
    {
        terrainList[0].transform.position = new Vector3(
            terrainList[1].transform.position.x,
            terrainList[1].transform.position.y,
            terrainList[1].transform.position.z - terrainList[1].terrainData.size.z);
        terrainList[2].transform.position = new Vector3(
            terrainList[1].transform.position.x,
            terrainList[1].transform.position.y,
            terrainList[1].transform.position.z + terrainList[1].terrainData.size.z);

//        terrainList[0].gameObject.GetComponent<TerrainPerlinNoise>().GenerateHeights(50.0f,5);
//        terrainList[1].gameObject.GetComponent<TerrainPerlinNoise>().GenerateHeights(50.0f,5);
//        terrainList[2].gameObject.GetComponent<TerrainPerlinNoise>().GenerateHeights(50.0f,5);

        terrainList[0].SetNeighbors(null, null, terrainList[1], null);
        terrainList[1].SetNeighbors(terrainList[0], null, terrainList[2], null);
        terrainList[2].SetNeighbors(terrainList[1], null, null, null);
    }
    
    void Update ()
    {
        Vector3 playerPosition = new Vector3(PlayerObject.transform.position.x, PlayerObject.transform.position.y, PlayerObject.transform.position.z);
        Terrain playerTerrain = null;
        int xOffset = 0;
        for (int x = 0; x < 3; x++)
        {
            if ((playerPosition.x >= terrainList[x].transform.position.x) &&
                (playerPosition.x <= (terrainList[x].transform.position.x + terrainList[x].terrainData.size.x)) &&
                (playerPosition.z >= terrainList[x].transform.position.z) &&
                (playerPosition.z <= (terrainList[x].transform.position.z + terrainList[x].terrainData.size.z)))
            {
       		     playerTerrain = terrainList[x];
      		     xOffset = 1 - x;
                 break;
            }
            if (playerTerrain != null) break;
        }
        
        if (playerTerrain != terrainList[1])
        {
            Terrain[] newTerrainList = new Terrain[3];
            for (int x = 0; x < 3; x++)
            {
                int newX = x + xOffset;
                if (newX < 0) newX = 2;
                else if (newX > 2) newX = 0;
                newTerrainList[newX] = terrainList[x];
            }
            terrainList = newTerrainList;
            UpdateTerrainPositionsAndNeighbors();
        }
    }
}
