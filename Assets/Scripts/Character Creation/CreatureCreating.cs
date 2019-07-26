using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreatureCreating : MonoBehaviour {

    Controller gPad;

    const float maxFinishTime = 1.5f;

    static float MaxCreationTime = 30;

    float creatingTimer, finishTimer;

    public CanvasGroup S1, S2, S3;

    int screenState = 0;

    static GameObject partParentPrefab;

    public static GameObject pParentPrefab
    {
        get
        {
            return partParentPrefab;
        }
    }

    public static CState createState = CState.loadingScreen;

    //List of available body types to be chosen
    public static List<GameObject> bodies;

    public List<Sprite> bodyImages;

    //List of available Limbs types to be spawned
    public List<GameObject> limbs;

    int totalSpawned = 0;

    const int maxSpawned = 300;

    Object[] objs;

    public List<GameObject> creatureControllers;

    public List<GameObject> cursors;

    public List<GameObject> pipes;

    //Text elementing relating to Timer
    public Text time;
    
    public List<Text> playersBodyText;

    public List<Image> playersConfirmed;

    int playersActive, totPlayersReady;

    bool playersReady
    {
        get
        {
            playersActive = 0;
            totPlayersReady = 0;

            foreach (GameObject player in Game.Current.players)
            {
                playersActive++;
            }

            for (int i = 0; i < playersConfirmed.Count; i++)
            {
                if (playersBodyText[i].gameObject.activeSelf)
                {
                    if (playersConfirmed[i].enabled)
                    {
                        totPlayersReady++;
                    }
                }
            }

            if (playersActive == totPlayersReady)
            {
                //Debug.Log(playersActive + " " + totPlayersReady);
                return true;
            }
            else
            {
                //Debug.Log("Players active" + playersActive);
                //Debug.Log("Total players ready" + totPlayersReady);
            }
            return false;
        }
    }

    public AudioSource creatingMusic, finishSound, pipePop;

    public bool overidePlayers;

    public int totPlayers;

	// Use this for initialization
	void Start () {

        gPad = new Controller(0);

        MaxCreationTime = Game.creationTime;

        createState = CState.loadingScreen;
        Screen.showCursor = false;

        bodies = new List<GameObject>();
        bodyImages = new List<Sprite>();

        partParentPrefab = Resources.Load("Prefabs/partParent") as GameObject;

        objs = Resources.LoadAll("BodyParts/CoreBodyTypes");
        
        GameObject temp;
        
        foreach(object obj in objs)
        {
            temp = (GameObject)obj;
            
            if(temp.tag == "Body parts")
            {
                bodies.Add(temp);
            }
        }

        objs = Resources.LoadAll("BodyParts/Limbs");
        
        foreach(object obj in objs)
        {
            temp = (GameObject)obj;
            
            if(temp.tag == "Body parts")
            {
                limbs.Add(temp);
            }
        }

        objs = Resources.LoadAll<Sprite>("Images/Body Images");

        foreach (object obj in objs)
        {
            bodyImages.Add((Sprite)obj);
        }

        if (overidePlayers)
            Game.Current.totalPlayers = totPlayers;

        Game.Current.players.Clear();

        for (int i = 0; i < 4; i++)
        {
            if (i < Game.Current.totalPlayers)
            {
                playersBodyText[i].gameObject.SetActive(true);
                playersConfirmed[i].gameObject.SetActive(true);
                cursors[i].SetActive(true);
                Game.Current.players.Add(creatureControllers[i]);
            }

            creatureControllers[i].SetActive(true);
        }
    }
    
    void OnLevelWasLoaded(int level)
    {
        overidePlayers = false;
        totPlayers = 0;
        totalSpawned = 0;
    }
	
	// Update is called once per frame
	void Update () {

        gPad.updateStates();

        switch(createState)
        {
            case CState.loadingScreen:

                if((gPad.isKeyDown(XKeyCode.A) || Input.GetKeyDown(KeyCode.Space))
                    && screenState == 0)
                {
                    S1.alpha = 0;
                    screenState = 1;
                }

                else if ((gPad.isKeyDown(XKeyCode.A) || Input.GetKeyDown(KeyCode.Space))
                    && screenState == 1)
                {
                    S2.alpha = 0;
                    screenState++;
                }

                else if ((gPad.isKeyDown(XKeyCode.A) || Input.GetKeyDown(KeyCode.Space))
                    && screenState == 2)
                {
                    S3.alpha = 0;
                    screenState++;
                }

                else if(screenState == 3)
                {
                    createState = CState.choosingBodies;
                }

                break;

            case CState.choosingBodies:

                //Display names of creatures currently being selected by player
                for(int i = 0; i < playersBodyText.Count; i++)
                {
                    if (playersBodyText[i].gameObject.activeSelf)
                    {
                        playersBodyText[i].text = animalName(bodies[creatureControllers[i].creatureController().bType]);

                        playersBodyText[i].GetComponentInChildren<Image>().sprite = bodyImages[creatureControllers[i].creatureController().bType / 3];

                        playersConfirmed[i].enabled = creatureControllers[i].creatureController().confirmed;
                    }
                }

                //If everyone ready
                if(playersReady)
                {
                    //Move to choosing Size
                    createState = CState.choosingSize;

                    foreach (GameObject cController in creatureControllers)
                    {
                        cController.creatureController().confirmed = false;
                    }
                }

                break;

            case CState.choosingSize:

                for (int i = 0; i < playersBodyText.Count; i++)
                {
                    if (playersBodyText[i].gameObject.activeSelf)
                    {
                        playersBodyText[i].text = getBodySize(creatureControllers[i].creatureController().bSize);

                        playersConfirmed[i].enabled = creatureControllers[i].creatureController().confirmed;
                    }
                }


                if (playersReady)
                {
                    //Move to choosing Size
                    activatePlayers();

                    startCreating();
                }

                break;

            case CState.creating:

                creatingTimer -= Time.deltaTime;

                time.text = creatingTimer.ToString("F0");

                if(creatingTimer <= 1)
                {
                    stopCreating();
                }

                if(Random.value < 0.1)
                {
                    //spawnPart(limbs[1], pipes[Random.Range(0, pipes.Count)], pipePop);
                    spawnPart(limbs[Random.Range(0, limbs.Count)], pipes[Random.Range(0, pipes.Count)], pipePop);
                }

                break;

            case CState.finish:

                finishTimer -= Time.deltaTime;

                if(finishTimer <=0)
                {
                    MainMenu._mMstate = MainMenuState.creatureCreation;
                    MainMenu.moveNow = true;
                    CharacterCreation.startSimpleNow = true;
                    CharacterCreation.simpleReplace = false;
                    Application.LoadLevel("Main Menu");
                }

                break;
        }

	}

    public string animalName(GameObject animal)
    {
        animal.name = animal.name.Replace("1", "");
        return animal.name.Replace("Standard", "");
    }

    void activatePlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < Game.Current.totalPlayers)
            {
                creatureControllers[i] = addBody(creatureControllers[i]);
            }
        }
    }

    GameObject addBody(GameObject pCreature)
    {
        int id = pCreature.GetComponent<CreatureController>().playerID;

        CreatureController cController = pCreature.GetComponent<CreatureController>();
        Destroy(pCreature);

        pCreature = Instantiate(bodies[(int)cController.bType + (int)cController.bSize], pCreature.transform.position, Quaternion.Euler(Vector3.up * 90)) as GameObject;

        pCreature.name = bodies[(int)cController.bType].name + id;
        pCreature.GetComponent<BodyPart>().playerID = id;
        pCreature.GetComponent<CreatureController>().playerID = id;

        pCreature.renderer.material = AssetManager.playerMaterials[id-1];

        return pCreature;
    }

    public void spawnPart(GameObject _Part, GameObject pipe, AudioSource pipePop)
    {
        //Spawns Animal part at position with it's starting rotation
        //GameObject temp = Instantiate(_Part, Vector3.up*10, _Part.transform.rotation) as GameObject;
        if(totalSpawned < maxSpawned)
        {
            Vector3 baseScale = _Part.transform.localScale;

            float rand = Random.Range(0.75f, 1.25f);

            GameObject temp = _Part.Spawn(pipe.transform.position, _Part.transform.rotation);
            totalSpawned++;

            //    = Instantiate(_Part, pipe.transform.position, _Part.transform.rotation) as GameObject;
            //totalSpawned++;

            temp.transform.localScale *= rand;

            temp.GetComponent<LegJoint>().scalingStrength = rand;

            if(temp.GetComponent<Spring>())
            {
                temp.GetComponent<Spring>().setScaling();
            }

            Animator animate = pipe.GetComponentInChildren<Animator>();
            pipe.GetComponent<AudioSource>().Play();
            
            animate.Play("Pipe");
            
            //Only Bodies have magnet script can be used to identify it 
            //        if(temp.GetComponent<Magnet>())
            //        {
            //            GameObject.FindGameObjectWithTag("instancing").
            //                GetComponent<CreatureManager>().animal = temp;
            //        }
            
            temp.name = _Part.name;  
            
            //Sets up part parent for hingeJoints
            if(temp.hingeJoint)
            {
                GameObject pParent = Instantiate(pParentPrefab, pipe.transform.position, Quaternion.identity) as GameObject;
                temp.hingeJoint.connectedBody = pParent.rigidbody;
            }
            else //Or Configurable Joints if they are used
            {
                GameObject pParent = Instantiate(pParentPrefab, temp.GetComponent<ConfigurableJoint>().connectedAnchor, Quaternion.identity) as GameObject;
                temp.GetComponent<ConfigurableJoint>().connectedBody = pParent.rigidbody;
            }
            //        partToAdd.layer = LayerMask.NameToLayer("Ignore Raycast");
            
            //        setMaterial(partToAdd, RTransparent);
            
            //        partToAdd.SetActive(true);
            
            //Adds random force downwards;
            Vector3 pipeForce = -pipe.transform.up * Random.Range(10,50);
            temp.rigidbody.AddForce(pipeForce, ForceMode.Impulse);


        }
    }

    void startCreating()
    {
        time.gameObject.SetActive(true);
        createState = CState.creating;

        creatingTimer =  MaxCreationTime;
    }

    void stopCreating()
    {
        time.gameObject.SetActive(false);

        creatingMusic.Stop();
        finishSound.Play();

        createState = CState.finish;

        for (int i = 0; i < Game.Current.players.Count; i++)
        {
            Game.Current.players[i] = creatureControllers[i];
            Game.Current.players[i].creatureController().RotateTo(Quaternion.identity, 0.1f);
            Game.Current.players[i].GetComponent<BodyPart>().parts.Clear();
            Tools.setAllChildren(Actions.findOnGround, Game.Current.players[i], Game.Current.players[i].GetComponent<BodyPart>());
            DontDestroyOnLoad(Game.Current.players[i]);
        }

        finishTimer = maxFinishTime;
    }

    string getBodySize(BodySize bType)
    {
        switch(bType)
        {
            case BodySize.standard:

                return "Standard";

            case BodySize.large:

                return "Large";

            case BodySize.small:
                
                return "Small";
                
            default:

                return "error";
        }
    }
}
