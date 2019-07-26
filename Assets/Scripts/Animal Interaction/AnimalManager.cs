using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalManager : MonoBehaviour 
{
    //Refernce to player
    public PlayerAnimal player;

    //Templates for enemies and Plants
    public AnimalAI AnimalAiTemplate;
    public Organism plantTemplate;

    public GameObject relSpawn;

    //Lists for Animals and Plants
    List<AnimalAI> animals;
    List<Organism> plants;

    //Variables for spawning variables
    bool bSpawningAnimal = true;
    float HOCLevelToSpawn = 50;
    float spawnHunger;
    string spawnNutritionalMass = "50";
    string spawnPos = "0";

    AnimalVariables spawnAV;

    //Variables for gamelogic spawning
    //Distance at which we start spawning things
    float spawningDistance = 100;

    //How many Animals and Vegetation spawn, a lower value means higher density as it relates to how far apart things are spawned
    float plantDensity = 20;
    float animalDesnsity = 40;

    //Variable to randomise the spawning a little
    float positionVariance;
    //range of randomisation
    float plantPVRange = 15;
    float animalPVRange = 30;

    Vector3 previousPlantSpawn;
    Vector3 previousAnimalSpawn;

    //Relative spawning position to the player
    Vector3 relSpawnPos;

    RaycastHit spawnRayHitInfo;
   
	// Use this for initialization
	void Start () 
    {
        animals = new List<AnimalAI>();
        plants = new List<Organism>();

        spawnAV = new AnimalVariables();

        spawnAV.reset();

        previousPlantSpawn = player.transform.position;
        previousAnimalSpawn = player.transform.position;

        Random.seed = System.Environment.TickCount;

	}
	
	// Update is called once per frame
	void Update () 
    {   

        if(Input.anyKeyDown)
        {
            processInput();
        }

        for(int fA = 0 ; fA < animals.Count; fA++)
        {
            animals[fA].biggestDanger = null;
            animals[fA].bestFood = null;

            if(!animals[fA].hideable)    
            {
                //Add player to list of animals animals[fA] sees if player is in range or remove if player moves out of sight
                if(animals[fA].canSee(player.transform.position))
                {
                    //TODO:Monitor Players HOC level so reactions change  
                    //Add player if not already in list
                    //Checks to see if player is dangerous to Animal
                    if(animals[fA].isInDanger(player) && animals[fA].moreDangerous(player))
                    {
                        animals[fA].biggestDanger = player;
                    }
                    else if(animals[fA].canEat(player) && animals[fA].betterFood(player))
                    {
                        animals[fA].bestFood = player;
                    } 
                }
                
                //Add second animal to list of animals animals[fA] see if animals[sA] is in range or remove if animals[sA] moves out of sight
                for(int sA = 0; sA < animals.Count; sA++)
                {
                    if(animals[fA].canSee(animals[sA].transform.position) && animals[fA].name != animals[sA].name)
                    {
                        //Add second animal to first animals reactions if not in list
                        if(animals[fA].isInDanger(animals[sA]) && animals[fA].moreDangerous(animals[sA]))
                        {
                            animals[fA].biggestDanger = animals[sA];
                        }
                        else if(animals[fA].canEat(animals[sA]) && animals[fA].betterFood(animals[sA]))
                        {
                            animals[fA].bestFood = animals[sA];
                        }                       
                    }
                }
                
                for(int pL = 0; pL < plants.Count; pL ++)
                {
                    if(animals[fA].canSee(plants[pL].transform.position) && !plants[pL].hideable)
                    {
                        //Add plants[pL] if not in list
                        if(animals[fA].canEat(plants[pL]) && animals[fA].betterFood(plants[pL]))
                        {
                            animals[fA].bestFood = plants[pL];
                        }                    
                    }
                }
            }
        }

        spawningLogic();
	}

//    void OnGUI()
//    {       
//        if(GUI.Button(new Rect(0, Screen.height - 22, 100, 22), "Spawn Menu"))
//        {
//            if(bSpawningAnimal)
//            {
//                bSpawningAnimal = false;
//            }
//            else
//            {
//                bSpawningAnimal = true;
//            }
//        }
//
//        //GUI setup for spawning animals with differing variables (TESTING ONLY)
//        if(bSpawningAnimal)
//        {
//            GUI.Box(new Rect(0, Screen.height/2 - 50, 110, 200), "Animal To Spawn");
//
//            HOCLevelToSpawn = GUI.HorizontalSlider(new Rect(0, Screen.height/2 - 22, 110, 22), HOCLevelToSpawn, 0, 100);
//
//            spawnPos = GUI.TextField(new Rect(0, Screen.height/2, 110, 22), spawnPos);
//
//            spawnHunger = GUI.HorizontalSlider(new Rect(0, Screen.height/2 + 22, 110, 22), spawnHunger, 0, 100);
//
//            spawnNutritionalMass = GUI.TextField(new Rect(0, Screen.height/2 + 44, 110, 22), spawnNutritionalMass);
//
//            if(GUI.Button(new Rect(0, Screen.height/2 + 66, 100, 22), "Spawn Animal"))
//            {
//
//                Debug.Log("Spawned with " + HOCLevelToSpawn + " HOC Level");
//                Debug.Log("Spawned with " + spawnHunger + " Hunger");
//                Debug.Log("Spawned with " + spawnNutritionalMass + " NutritionalMass");
//
//                spawnAV.reset();
//
//                spawnAV.nutritionMass = int.Parse(spawnNutritionalMass);
//                spawnAV.HOCLevel = HOCLevelToSpawn;
//                spawnAV.hunger = spawnHunger;
//
//                spawnAnimal(relSpawn.transform.position + new Vector3(int.Parse(spawnPos), 2,0), 
//                            spawnAV);         
//            }
//
//        }
//    }

    void processInput()
    {
        //Spawning animals (TESTING)
        if(Input.GetKeyDown(KeyCode.Z))
        {
            spawnAV.reset();

            spawnAV.HOCLevel = 0;

            //Herbivore
            Debug.Log(relSpawn.transform.position);
            spawnAnimal(relSpawn.transform.position + new Vector3(0,2,0), spawnAV);
//            animals.Add(Instantiate(AnimalAiTemplate, relSpawn.transform.position + new Vector3(-5, 2,0), Quaternion.identity) as AnimalAI);
//            animals[animals.Count-1].Initialise((animals.Count-1), -1, -1, -1, -1, 0, -1, -1, -1);
        }
        
        if(Input.GetKeyDown(KeyCode.X))
        {
            spawnAV.reset();
            
            spawnAV.HOCLevel = 50;

            Debug.Log(relSpawn.transform);
            //Omnivore
            spawnAnimal(relSpawn.transform.position + new Vector3(0,2,0), spawnAV);
//            animals.Add(Instantiate(AnimalAiTemplate, relSpawn.transform.position + new Vector3(0,2,0), Quaternion.identity) as AnimalAI);
//            animals[animals.Count-1].Initialise((animals.Count-1), 
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            spawnAV.reset();
            
            spawnAV.HOCLevel = 100;
            //Carnivore
            spawnAnimal(relSpawn.transform.position + new Vector3(5,2,0), spawnAV);
        }

        //Spawning food 
        if(Input.GetKeyDown(KeyCode.V))
        {
            spawnPlant(relSpawn.transform.position + new Vector3(0, -1,0), 25);
        }


        if(Input.GetKeyDown(KeyCode.R))
        {
            reset();
        }
    }

    void spawningLogic()
    {
        //TODO: update this code so that is spawns plants and animals based on the planet we're on

        relSpawnPos = player.transform.position + Vector3.forward * spawningDistance;

        //Debug.DrawLine(relSpawnPos + Vector3.up * 50, spawnRayHitInfo.point, Color.red, 20);

        if(Vector3.Distance(relSpawnPos, previousPlantSpawn) > plantDensity)
        {
            positionVariance = Random.Range(-plantPVRange, plantPVRange);

            //Debug.Log(positionVariance);

            relSpawnPos += Vector3.forward * positionVariance;
            
            if(Physics.Raycast(relSpawnPos + Vector3.up * 50, Vector3.down, out spawnRayHitInfo))
            {
                //Debug.DrawLine(relSpawnPos + Vector3.up * 50, spawnRayHitInfo.point, Color.red, 20);

                spawnPlant(new Vector3(relSpawnPos.x, spawnRayHitInfo.point.y, relSpawnPos.z), 50);
                previousPlantSpawn = new Vector3(relSpawnPos.x, spawnRayHitInfo.point.y, relSpawnPos.z);
            }
        }

        if(Vector3.Distance(relSpawnPos, previousAnimalSpawn) > animalDesnsity)
        {
            positionVariance = Random.Range(-animalPVRange, animalPVRange);
            
            //Debug.Log(positionVariance);
            
            relSpawnPos += Vector3.forward * positionVariance;
            
            if(Physics.Raycast(relSpawnPos + Vector3.up * 50, Vector3.down, out spawnRayHitInfo))
            {
                //Debug.DrawLine(relSpawnPos + Vector3.up * 50, spawnRayHitInfo.point, Color.red, 20);

                Vector3 finalSpawn = new Vector3(relSpawnPos.x, spawnRayHitInfo.point.y + 5, relSpawnPos.z);

                spawnAV.nutritionMass = Random.Range(10, 40);
                spawnAV.moveForce = Random.Range(600, 900);
                spawnAV.maxVelocity = Random.Range(6, 15);
                spawnAV.jumpStrength = Random.Range(25, 60);
                spawnAV.attackStrength = Random.Range(20, 40);
                spawnAV.maxHealth = Random.Range(70, 120);
                spawnAV.health = spawnAV.maxHealth - Random.Range(0, 20);
                spawnAV.maxHunger = Random.Range(60, 150);
                spawnAV.hunger = spawnAV.maxHunger - Random.Range(0, 30);
                spawnAV.HOCLevel = Random.Range(0, 100);

                spawnAnimal(finalSpawn, spawnAV);
                previousAnimalSpawn = finalSpawn;
            }
        }
    }

    //Spawns animal but checks if there are inactive animals before added new one
    void spawnAnimal(Vector3 spawnPos, AnimalVariables sAV)
    {
        for(int a = 0; a < animals.Count; a++)
        {
            if(!animals[a].gameObject.activeSelf)
            {
                animals[a].transform.position = spawnPos;
                animals[a].transform.rotation = Quaternion.identity;
                animals[a].Initialise(a, sAV);

                Color temp = animals[a].renderer.material.color;

                temp.a = 1;
                animals[a].renderer.material.color = temp; 

                animals[a].hideable = false;
                animals[a].gameObject.SetActive(true);

                animals[a].HOCLevelColouring();

                return;
            }
        }

        animals.Add(Instantiate(AnimalAiTemplate, spawnPos, Quaternion.identity) as AnimalAI);
        animals[animals.Count-1].Initialise((animals.Count-1), sAV);
    }

    void spawnPlant(Vector3 spawnPos, float nutMass)
    {
        for(int p = 0; p < plants.Count; p++)
        {
            if(!plants[p].gameObject.activeSelf)
            {
                plants[p].transform.position = spawnPos;
                plants[p].transform.rotation = Quaternion.identity;
                plants[p].Initialise(p, nutMass);

                Color temp = plants[p].renderer.material.color;
                
                temp.a = 1;
                plants[p].renderer.material.color = temp; 
                
                plants[p].hideable = false;
                plants[p].gameObject.SetActive(true);

                return;
            }
        }

        plants.Add(Instantiate(plantTemplate, spawnPos, Quaternion.identity) as Organism);            
        plants[plants.Count-1].Initialise((plants.Count-1), nutMass);
    }

    public void reset()
    {
        Debug.Log("Reset called");
        foreach(Animal animal in animals.ToArray())
        {
            Destroy(animal.gameObject);
        }

        animals.Clear();

        foreach(Organism org in plants.ToArray())
        {
            Destroy(org.gameObject);
        }

        plants.Clear();
    }
}
