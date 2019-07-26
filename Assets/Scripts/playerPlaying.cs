using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;
using UnityEngine.UI;

public class playerPlaying : MonoBehaviour
{

    Controller gPad;

    public int playerID;

    public bool playing = false, ready = false;

    public List<Light> playerLight;

    public Text playerText;
    //Game object made up of some text and an image to show
    //that a player is connected
    public Text contConc;
    //Gameobject that is an Image and text that appears when a 
    //controller is connected but not Joined
    public GameObject Join;

    bool testingAnimal = false;

    //Since we remove drag when animals are in test tube we need to store it for reapplication later
    public float angularDrag;

    //Positions for the each players TestTube and testing room spawn
    public Transform testTubePosition, testingPosition;

    public GameObject player;

    public LerpTrackCam cam;

    public bool playerEnabled
    {
        get
        {
            return Game.Current.players[playerID-1];
        }
    }

    float totLeft, totRight;
    float numLeft = 0, numRight = 0, lRRatio = 0;

    float x = 0,
        y = 0,
        z = 0;

    bool generatingAnimal;

    float MAXGeneratingTime = .25f;

    float genTimer = 0;

    float rand;

    GameObject newLimb, limbToAdd;

    int randBody, minNumOfLimbs, maxNumOfLimbs, numOfLimbs;

    //List that is used to pick limbs to use when creating an Animal
    List<GameObject> limbPool;
	
	List<GameObject> attractAnims;
    void Awake()
    {
        gPad = new Controller(playerID - 1);
        gPad.updateStates();
    }
    // Use this for initialization
    void Start()
    {
        limbPool = new List<GameObject>();

        Light[] lights = GetComponentsInChildren<Light>();
        playerLight = new List<Light>();
        playerLight.Clear();
        playerLight.AddRange(lights);

		attractAnims = new List<GameObject>(10);
    }

    void OnLevelWasLoaded(int level)
    {
        Light[] lights = GetComponentsInChildren<Light>();
        playerLight = new List<Light>();
        playerLight.Clear();
        playerLight.AddRange(lights);

        if (Game.Current.players.Count > (playerID - 1) && gPad.IsConnected)
        {
            playing = true;
            playerText.enabled = true;
            contConc.enabled = false;

            playerLight[0].enabled = true;
            playerLight[1].enabled = true;

            if(Join)
                Join.gameObject.SetActive(false);

            if (player)
            {
                foreach(Transform child in player.transform)
                {
                    child.Recycle();
                }
                player.Recycle();
            }

            player = Game.Current.players[playerID - 1];

            if (player)
            {
                if (player.GetComponent<AutoPlayer>())
                {
                    Destroy(player.GetComponent<AutoPlayer>());
                }

                setTesting(false);
            }

            if(playerID > 1)
                Game.Current.totalPlayers++;
        }
        else
        {
            playing = false;
            playerText.enabled = false;
            contConc.enabled = true;

            playerLight[0].enabled = false;
            playerLight[1].enabled = false;
            if(Join)
                Join.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        gPad.updateStates();

        switch (CharacterCreation.creationState)
        {
            case CreationState.simple:

                if (playing && !MainMenu.attract)
                {
                    //Pressing start says that you are ready
                    if (gPad.isKeyDown(XKeyCode.Start) || Input.GetKeyDown(KeyCode.Space))
                    {
                        ready = !ready;

                        cam.playerCanvas.setReady(ready);
                    }
                    //Pressing A to toggle between test tube view and testing in room
                    if (gPad.isKeyDown(XKeyCode.Y) || Input.GetKeyDown(KeyCode.UpArrow) & !generatingAnimal)
                    {
                        testingAnimal = !testingAnimal;

                        setTesting(testingAnimal);
                    }

                    if(!testingAnimal)
                    {
                        //Generate new Knee Animal
                        if (gPad.isKeyDown(XKeyCode.X) || Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            //First limb is Knee
                            genAnimalLogic(AssetManager.animLimbs[0]);
                        }

                        //Generate new Spring Aniaml
                        if (gPad.isKeyDown(XKeyCode.A) || Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            //3rd Limb is spring
                            genAnimalLogic(AssetManager.animLimbs[2]);
                        }
                        //Generate new Motor Joint Animal
                        if (gPad.isKeyDown(XKeyCode.B) || Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            //2nd Limb is Motor 
                            genAnimalLogic(AssetManager.animLimbs[1]);
                        }
                    }

                    if (generatingAnimal)
                    {
                        genTimer -= Time.deltaTime;

                        if (genTimer < 0)
                        {
                            genTimer = 0;

                            generatingAnimal = false;

                            addPlayer(limbPool);
                            //addPlayer(limbPool);
                        }
                    }
                }

                break;

            case CreationState.chooseState:

                if (gPad.IsConnected)
                {
                    playerText.enabled = true;
                    contConc.enabled = false;

                    if (playerID > 1)
                    {
                        if (playing)
                        {
                            playerText.color = new Color(playerText.color.r, playerText.color.g, playerText.color.b, 1);
                            Join.SetActive(false);
                        }
                        else
                        {
                            playerText.color = new Color(playerText.color.r, playerText.color.g, playerText.color.b, .25f);
                            Join.SetActive(true);
                        }

                        if (gPad.isKeyDown(XKeyCode.A) && !playing)
                        {
                            Game.Current.totalPlayers++;
                            playing = true;
                            playerLight[0].enabled = playing;
                            playerLight[1].enabled = playing;
                        }

                        if (gPad.isKeyDown(XKeyCode.B) && playing)
                        {
                            Game.Current.totalPlayers--;
                            playing = false; ;
                            playerLight[0].enabled = playing;
                            playerLight[1].enabled = playing; ;
                        }
                    }
                    else
                    {
                        playing = true;
                        playerLight[0].enabled = playing;
                        playerLight[1].enabled = playing;
                    }
                }
                else
                {
                    playerText.enabled = false;
                    if(Join)
                        Join.SetActive(false);
                    contConc.enabled = true;
                }

                break;
        }
    }

    private void genAnimalLogic(GameObject gameObject)
    {
        if (!generatingAnimal)
        {
            genTimer = MAXGeneratingTime;
            generatingAnimal = true;
        }

        if(!limbPool.Contains(gameObject))
        {
            limbPool.Add(gameObject);
        }
    }

    public void addPlayer(GameObject limb, bool attract)
    {
        //Remove original
        if (player && !attract)
        {
            Debug.Log("here");
            foreach (Transform child in player.transform)
            {
                child.gameObject.Recycle();
            }
            player.Recycle();
        }

        //Add random Body
        int randBody = Random.Range(0, AssetManager.animBodies.Count);

        player = AssetManager.animBodies[randBody].Spawn();

        if (!attract)
        {
            Game.Current.players[playerID - 1] = player;
        }
		else
		{
			if(attractAnims.Count == attractAnims.Capacity)
			{
				foreach(Transform child in attractAnims[0].transform)
				{
					child.Recycle();
				}
				attractAnims[0].Recycle();
				attractAnims.RemoveAt(0);
			}

			attractAnims.Add(player);
		}

        int minNumOfLimbs = (int)(4 * player.transform.localScale.x);
        int maxNumOfLimbs = (int)(5 * player.transform.localScale.x);

        int numOfLimbs = Random.Range(minNumOfLimbs, maxNumOfLimbs);

        totLeft = Mathf.Ceil((float)numOfLimbs / 2);
        totRight = Mathf.Floor((float)numOfLimbs / 2);

        numLeft = 0;
        numRight = 0;

        /*TODO: If  other cases for other sides of animals are added need to refine logic 
        by adding limbs to each side and the deciding on there positions after all have been placed*/
        for (int i = 0; i < numOfLimbs; i++)
        {
            //TODO: Add other cases for other placing preferences
            switch (limb.GetComponent<LegJoint>().placePref)
            {
                case PlacePrefrence.LeftAndRight:

                    placeLeftAndRight(limb, player, numOfLimbs);

                    break;
            }
        }

        //Set body up correctly
        player.transform.position = testTubePosition.position;
        player.transform.rotation = Quaternion.identity;

        Tools.setAllChildren(Actions.setGravity, player, false, "Eyes");
        Tools.setAllChildren(Actions.canRace, player, false);
        Tools.setAllChildren(Actions.setWiggle, player, true);
        Tools.setAllChildren(Actions.setKinematic, player, false);

        if (!attract)
        {
            Tools.setAllChildren(Actions.setID, player, playerID);
            Tools.setAllChildren(Actions.setMaterial, player, AssetManager.playerMaterials[playerID - 1]);
        }
        else
        {
            int randPlayer = Random.Range(1, 5);
            Tools.setAllChildren(Actions.setID, player, randPlayer);
            Tools.setAllChildren(Actions.setMaterial, player, AssetManager.playerMaterials[randPlayer-1]);
            Tools.setAllChildren(Actions.setID, player, playerID);
        }

        player.rigidbody.constraints = RigidbodyConstraints.FreezePosition;

        angularDrag = player.rigidbody.angularDrag;
        player.rigidbody.angularDrag = 0;

        cam.target = player;
    }

    public void setup()
    {
        player.transform.position = testTubePosition.position;
        player.transform.rotation = Quaternion.identity;

        Tools.setAllChildren(Actions.setGravity, player, false, "Eyes");
        Tools.setAllChildren(Actions.canRace, player, false);
        Tools.setAllChildren(Actions.setWiggle, player, true);
        Tools.setAllChildren(Actions.setKinematic, player, false);
        Tools.setAllChildren(Actions.setID, player, playerID);
        Tools.setAllChildren(Actions.setMaterial, player, AssetManager.playerMaterials[playerID - 1]);

        player.rigidbody.constraints = RigidbodyConstraints.FreezePosition;

        angularDrag = player.rigidbody.angularDrag;
        player.rigidbody.angularDrag = 0;

        cam.target = player;
	}

    public void addPlayer(List<GameObject> limbs)
    {
        //Remove original
        if (player)
        {
            Debug.Log("here");
            foreach (Transform child in player.transform)
            {
                child.gameObject.Recycle();
            }
            player.Recycle();
        }

        //Add random Body
        randBody = Random.Range(0, AssetManager.animBodies.Count);

        player = AssetManager.animBodies[randBody].Spawn();

        Game.Current.players[playerID - 1] = player;

        minNumOfLimbs = (int)(4 * player.transform.localScale.x);
        maxNumOfLimbs = (int)(5 * player.transform.localScale.x);

        numOfLimbs = Random.Range(minNumOfLimbs, maxNumOfLimbs);

        totLeft = Mathf.Ceil((float)numOfLimbs / 2);
        totRight = Mathf.Floor((float)numOfLimbs / 2);

        numLeft = 0;
        numRight = 0;

        /*TODO: If  other cases for other sides of animals are added need to refine logic 
        by adding limbs to each side and the deciding on there positions after all have been placed*/
        for (int i = 0; i < numOfLimbs; i++)
        {
            limbToAdd = limbs[Random.Range(0, limbs.Count)];
            //TODO: Add other cases for other placing preferences
            switch (limbToAdd.GetComponent<LegJoint>().placePref)
            {
                case PlacePrefrence.LeftAndRight:

                    placeLeftAndRight(limbToAdd, player, numOfLimbs);

                    break;
            }
        }

        //Set body up correctly
        player.transform.position = testTubePosition.position;
        player.transform.rotation = Quaternion.identity;

        Tools.setAllChildren(Actions.setGravity, player, false, "Eyes");
        Tools.setAllChildren(Actions.canRace, player, false);
        Tools.setAllChildren(Actions.setWiggle, player, true);
        Tools.setAllChildren(Actions.setKinematic, player, false);
        Tools.setAllChildren(Actions.setID, player, playerID);
        Tools.setAllChildren(Actions.setMaterial, player, AssetManager.playerMaterials[playerID - 1]);

        player.rigidbody.constraints = RigidbodyConstraints.FreezePosition;
        angularDrag = player.rigidbody.angularDrag;
        player.rigidbody.angularDrag = 0;

        cam.target = player;

        player.GetComponent<BodyPart>().parts.Clear();
        player.GetComponent<BodyPart>().findonGround(player);

        limbPool.Clear();


    }

    void placeLeftAndRight(GameObject limb, GameObject parent, int numOfLimbs)
    {
        //Add new Limb instance
        newLimb = limb.Spawn(limb.transform.position, limb.transform.rotation);

        //Adjust its scale to vary slightly
        rand = Random.Range(0.75f, 1.25f);
        newLimb.transform.localScale *= rand;
        newLimb.GetComponent<LegJoint>().scalingStrength = rand;

        //Remove hinges to allow the limb to be moved.
        Tools.removeHinges(newLimb);

        //Reset its local position
        newLimb.transform.parent = player.transform;
        newLimb.transform.localPosition = Vector3.zero;

        /*Find it's X position by using the X extents of the parent object multiplied by its scale
        and the X Extents of the object multiplied by its scale*/
        x = (parent.renderer.bounds.extents.x / parent.transform.localScale.x) + 
            limb.renderer.bounds.extents.x * 1/ parent.transform.localScale.x;

        //Uses pyramid logic to decide where objects should be placed
        //LeftBias
        if (numLeft <= numRight)
        {
            //Spawn left
            //Debug.Log("Place on Left");
            numLeft++;
            lRRatio = (numLeft / (totLeft + 1));
        }
        else
        {
            //Debug.Log("Place on Right");
            x *= -1;
            numRight++;
            lRRatio = (numRight / (totRight + 1));
            newLimb.transform.localEulerAngles += Vector3.up * 180;
        }

        //Randomise heights to be be within the Parent object
        y = Random.Range(-parent.renderer.bounds.extents.y/2, 0) / 2 * parent.transform.localScale.x;

        //Use the parents Z extents to place the limbs along with the Ratio
        z = Mathf.Lerp(-parent.renderer.bounds.extents.z * newLimb.renderer.bounds.size.z * 1.1f / parent.transform.localScale.x,
            parent.renderer.bounds.extents.z * newLimb.renderer.bounds.size.z * 1.1f / parent.transform.localScale.x, lRRatio) * Random.Range(.9f, 1.1f);

        //Finally set the position
        newLimb.transform.localPosition = new Vector3(x, y, z);
        newLimb.rigidbody.isKinematic = false;
		StartCoroutine(Tools.reAddHinges(newLimb, RigidbodyConstraints.None, false));

    }

    public void setTesting(bool testingAnimal)
    {
        //Move to test room
        if (testingAnimal)
        {
            player.transform.position = testingPosition.transform.position;
            player.transform.rotation = testingPosition.transform.rotation;

            player.rigidbody.constraints = RigidbodyConstraints.None;
            player.rigidbody.angularDrag = angularDrag;
            
            Tools.setAllChildren(Actions.canRace, player, true);
            Tools.setAllChildren(Actions.setGravity, player, true);
            Tools.setAllChildren(Actions.setWiggle, player, false);

            if (cam)
            {
                cam.angle = cam.originalRotation.eulerAngles - testingPosition.transform.rotation.eulerAngles;
                cam.transform.position = testingPosition.transform.position;
                cam.speed = 1;
                cam.canRotate = true;
                cam.playerOffset = Vector3.zero;
            }
        }
        else //Move to TestTube
        {
            player.transform.position = testTubePosition.position;

            player.rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            player.rigidbody.angularDrag = 0;

            Tools.setAllChildren(Actions.canRace, player, false);
            Tools.setAllChildren(Actions.setGravity, player, false);
            Tools.setAllChildren(Actions.setWiggle, player, true);

            if (cam)
            {
                cam.angle = Vector3.zero;
                cam.transform.position = testTubePosition.transform.position;
                cam.canRotate = false;
                cam.speed = cam.originalSpeed;
                cam.playerOffset = Vector3.zero;
            }
        }

        if (cam)
        {
            cam.playerCanvas.setControls(testingAnimal);
        }
    }

}