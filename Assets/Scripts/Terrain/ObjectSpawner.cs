using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour {
    public Terrain terrain;
    float[,] heights;
	public List<GameObject> desertObjectsSmall;
	public List<GameObject> desertObjectsBig;
    public List<GameObject> normalObjectsSmall;
	public List<GameObject> normalObjectsBig;
	public List<GameObject> particals;
    public Image loadingBar;
    public CanvasGroup loadingBarBack;
    public CanvasGroup loadingScreen, cont;
    int x, y;
    int WhatWorld
    {
        get
        {
            if (Game.Current.planet != null)
            {
                int wWorld = (int)Tools.RangeConvert(Game.Current.planet.radius, 50, 150, 0, AssetManager.terrainMaterials.Count - 1);
                return Mathf.Clamp(wWorld, 0, AssetManager.terrainMaterials.Count - 1);
            }
            else
            {
                int wWorld = 0;
                return wWorld;
            }
            //return 0;
        }
    }
    int RainbowChance;

    float o, p, spawnRange, Item, ItemScale, RandomRotation;
    GameObject InstantiatingItem, InstantiatedItem;
    // Use this for initialization
    void Start () {
        //WhatWorld = (int)Tools.RangeConvert(Game.Current.planet.radius, 50, 150, 0, AssetManager.terrainMaterials.Count);
    }

    public void spawnObjs () {
        GetHeights();
        StartCoroutine(SpawnObjects());
    }
    
    void GetHeights() {
        heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
        for (int x = terrain.terrainData.heightmapWidth-1;x>=0;x--)
        {
            for(int y = terrain.terrainData.heightmapWidth-1;y>=0;y--)
            {
                heights[x,y] = terrain.terrainData.GetHeight(x,y);
            }
        }
    }
    
    public IEnumerator SpawnObjects(){
        RainbowChance = Random.Range(1, 100);
		for (x = 0; x <= terrain.terrainData.heightmapWidth - 1; x++) {
			for (y = terrain.terrainData.heightmapHeight-1; y>=0; y--) {
				o = Tools.RangeConvert (x, 0, terrain.terrainData.heightmapWidth - 1, terrain.transform.position.x, terrain.transform.position.x + 1000);
				p = Tools.RangeConvert (y, 0, terrain.terrainData.heightmapHeight - 1, terrain.transform.position.z, terrain.transform.position.z + 1000);

				if (o > -200 && o < 1000) {

					if (p < 1000) {
						spawnRange = Random.Range (1, 200);
					} else {
						spawnRange = Random.Range (1, 600);
					}

					if (WhatWorld == 3 && particals.Count > 0){
						particals[0].SetActive(true);
					}
				
					if (WhatWorld != 6) {
						Item = Random.Range (1, (normalObjectsSmall.Count + normalObjectsBig.Count) + 1);
						if (spawnRange == 1) {
							InstantiatedItem = null;
							//if (Item == 1){
							if (Item <= normalObjectsBig.Count) {
								if (p < 200) 
                                {
									//InstantiatingItem = normalObjectsSmall [Random.Range (0, normalObjectsSmall.Count)];
                                    Item = Random.Range(normalObjectsBig.Count, (normalObjectsSmall.Count + normalObjectsBig.Count)+1);
								} else {
									InstantiatingItem = normalObjectsBig [Random.Range (0, normalObjectsBig.Count)];
								}
							}
                            if (Item > normalObjectsBig.Count) 
                            {
								InstantiatingItem = normalObjectsSmall [Random.Range (0, normalObjectsSmall.Count)];
							}
                        
							InstantiatedItem = Instantiate (InstantiatingItem, new Vector3 (o, heights [x, y] , p), InstantiatingItem.transform.rotation) as GameObject;
                            if (Item <= normalObjectsBig.Count)
                            {
                                InstantiatedItem.renderer.material = AssetManager.ObjectMaterials[WhatWorld + 11];
                            }
                            else
                            {
                                InstantiatedItem.renderer.material = AssetManager.ObjectMaterials[WhatWorld];
                            }
                            //if (p > 500) {
                            //    if (InstantiatedItem.rigidbody)
                            //        Destroy (InstantiatedItem.rigidbody);
                            //}

                            //change the object just spawned
							ItemScale = Random.Range (0.5f, 1);
							RandomRotation = Random.Range (1, 361);
							InstantiatedItem.transform.eulerAngles = InstantiatedItem.transform.eulerAngles + Vector3.up * RandomRotation;
							InstantiatedItem.transform.localScale = new Vector3 ((InstantiatingItem.transform.localScale.x * ItemScale),
							                                                     (InstantiatingItem.transform.localScale.y * ItemScale),
							                                                     (InstantiatingItem.transform.localScale.z * ItemScale));

                            if (RainbowChance == 1)
                            {
                                InstantiatedItem.renderer.material = AssetManager.ObjectMaterials[Random.Range(0, AssetManager.ObjectMaterials.Count)];
                            }

						}

					} else {
						Item = Random.Range (1, (desertObjectsSmall.Count + desertObjectsBig.Count) + 1);
						if (spawnRange == 1) {
							InstantiatedItem = null;
							//if (Item == 1){
							if (Item <= desertObjectsBig.Count) {
								if (p < 200) {
									InstantiatingItem = desertObjectsSmall [Random.Range (0, desertObjectsSmall.Count)];
								} else {
									InstantiatingItem = desertObjectsBig [Random.Range (0, desertObjectsBig.Count)];
								}
							} else if (Item > desertObjectsBig.Count) {
								InstantiatingItem = desertObjectsSmall [Random.Range (0, desertObjectsSmall.Count)];
							}
							InstantiatedItem = Instantiate (InstantiatingItem, new Vector3 (o, heights [x, y], p), InstantiatingItem.transform.rotation) as GameObject;
							if (p > 500) {
								if (InstantiatedItem.rigidbody)
									Destroy (InstantiatedItem.rigidbody);
							}
							ItemScale = Random.Range (0.5f, 1);
							RandomRotation = Random.Range (1, 361);
							InstantiatedItem.transform.eulerAngles = InstantiatedItem.transform.eulerAngles + Vector3.up * RandomRotation;
							InstantiatedItem.transform.localScale = new Vector3 ((InstantiatingItem.transform.localScale.x * ItemScale),
							                                                     (InstantiatingItem.transform.localScale.y * ItemScale),
							                                                     (InstantiatingItem.transform.localScale.z * ItemScale));
                            if (RainbowChance == 1)
                            {
                                InstantiatedItem.renderer.material = AssetManager.ObjectMaterials[Random.Range(0, AssetManager.ObjectMaterials.Count)];
                            }
                            
						}

				

					}
					//loadingBar.fillAmount += 1.0f / 3158028.0f;

				}

				//if (x % 5 == 0) {
				//	yield return null;
				//} 789507
				//loadingBar.fillAmount += 1.0f / 3158028.0f;
                loadingBar.fillAmount += 1.0f / 4737042f;
			}
			if (x % 5 == 0) {
				yield return null;
			}
		}
		loadingBarBack.alpha = 0;
		loadingBar.fillAmount = 0;
		cont.alpha = 1;
	}
    
    // Update is called once per frame
    void Update () {
        
    }
}

