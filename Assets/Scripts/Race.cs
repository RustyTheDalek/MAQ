using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Race : MonoBehaviour {

    Controller gPad;

    public AnimalLoader ALoader;

    public Text start, Victory, race;
    public float startTimer = 0, raceTimer = 0, endingTimer = 0;

    const float StartTimerLength = 3.5f, endTimerLength = 10, totalRaceTime = 90;
    public float finishLine = 500, slowmoDist = 20;

    bool slowing;

    public List<Text> winText;

    public List<GameObject> lines;
    public List<CanvasGroup> EndScreen;
    public List<Text> Stats;

    public GameObject EndRaceGUI;

    public CanvasGroup continueButton;
    public GameObject loadingScreen;

    public AudioSource music;
    public AudioSource countdown;

    public DistanceBar distBars;

	float minX = 126, maxX = 503, minZ = 1, maxZ = 487;

	List<GameObject> compAnims;

	float totLeft, totRight;
	float numLeft = 0, numRight = 0, lRRatio = 0;
	
	float x = 0,
	y = 0,
	z = 0;
	
    public enum raceState
    {
        loadingLevel,
        starting,
        racing,
        finishing,
        finished,
        none,
    };

    public static raceState rState = raceState.loadingLevel;

    bool pauseActive;

    bool second, third;
	// Use this for initialization
	void Start () {

        gPad = new Controller(0);

        startTimer = StartTimerLength;
        endingTimer = endTimerLength;

        SetUpLines();

		compAnims = new List<GameObject>();

		spawnAnims(Random.Range(10,50));

        rState = raceState.loadingLevel;
	}

	void spawnAnims(int total)
	{
		for(int i = 0; i < total; i++)
		{
			addPlayer(AssetManager.animLimbs[Random.Range(0,AssetManager.animLimbs.Count)]);
		}
	}
	
	void OnLevelWasLoaded(int level)
	{
	}
	
	public void startRace()
	{
        resetCompAnims();

        music.Stop();
		countdown.Play();
		
		//Disable post race GUI
		EndRaceGUI.SetActive(false);
		
		//start of race make end screen invisible
		EndScreen[0].alpha = 0;
		EndScreen[1].alpha = 0;
		EndScreen[2].alpha = 0;
		EndScreen[3].alpha = 0;
		
		//set all winning text to lose
		winText[0].text = "DNF";
		winText[1].text = "DNF";
		winText[2].text = "DNF";
        winText[3].text = "DNF";

        //Reset Timers
        startTimer = StartTimerLength;
        endingTimer = endTimerLength;
        raceTimer = totalRaceTime;
        race.transform.parent.gameObject.SetActive(false);

        //Reset Text
        race.text = "0";
        start.text = "";
        Victory.text = "";

        second = false;
        third = false;

        start.color = start.color.setColAlpha(1);

        distBars.activate();

        foreach (GameObject player in Game.Current.players)
        {
            if (!player.GetComponent<ScoringScript>())
            {
                player.AddComponent<ScoringScript>();
            }
        }

        SetUpLines();
        
        rState = raceState.starting;

        loadingScreen.SetActive(false);

    }

    void resetCompAnims()
    {
        foreach (GameObject anim in compAnims)
        {
            anim.transform.position = transform.position = new Vector3(Random.Range(minX, maxX), 20, Random.Range(minZ, maxZ));

            Tools.setAllChildren(Actions.setKinematic, anim, false);
        }
    }

    void SetUpLines()
    {
        //Take Z position from start line 
        float linesPosition = lines[0].transform.position.z;

        //foreach (GameObject animal in Game.Current.players)
        //{
        //    linesPosition += 12.5f;
        //}

        lines [0].transform.position = new Vector3(125, lines [0].transform.position.y, linesPosition);
        lines [1].transform.position = new Vector3(finishLine, lines [1].transform.position.y, linesPosition);

    }
	
	// Update is called once per frame
	void Update () {

        switch(rState)
        {
            case raceState.loadingLevel:

                gPad.updateStates();

                if ((gPad.isKeyDown(XKeyCode.A) || Input.GetKeyDown(KeyCode.Space)) && continueButton.alpha >= 1)
                {
                    loadingScreen.SetActive(false);
                    startRace();
                }

                break;

            case raceState.starting:

                if(startTimer > 1)
                {
                    start.text = startTimer.ToString("F0");
                    startTimer -= Time.deltaTime;
                }
                else
                {
                    start.text = "Go!";

                    foreach (GameObject player in Game.Current.players)
                    {
                        if (player)
                        {
                            activate(player);
                        }
                    }

                    music.Play();

                    StartCoroutine(Tools.lerpTextAlpha(start, 0, .1f));
                    race.transform.parent.gameObject.SetActive(true);
                    BodyPart.testing = true;
                    rState = raceState.racing;
                }

                break;

            case raceState.racing:

                gPad.updateStates();

                race.text = raceTimer.ToString("F2");
                raceTimer -= Time.deltaTime;

                if (raceTimer > 0)
                {
                    foreach (GameObject animal in Game.Current.players)
                    {
                        if(animal.transform.position.x  > (finishLine - slowmoDist) && animal.rigidbody.velocity.magnitude > 40)
                        {
                            if(!slowing)
                                StartCoroutine(changeTime(.25f, .05f));
                        }
                        if (animal.transform.position.x > finishLine)
                        {
                            slowing = false;
                            StopCoroutine(changeTime(.25f, .05f));
                            StartCoroutine(changeTime(1f, .5f));
                            int id = animal.GetComponent<BodyPart>().playerID;
                            //Victory.text = getPlayerName(id) + " wins!";
                            winText[id - 1].text = "1st";

                            rState = raceState.finishing;
                            //Save Animals finish Time

                            Tools.setAllChildren(Actions.canRace, animal, false);

                        }
                    }
                }
                else
                {
                    raceTimer = 0;
                    race.text = raceTimer.ToString("F2");
                    rState = raceState.none;
                    BodyPart.testing = false;
                    StartCoroutine(fin());
                }

                pauseCheck();

                break;

            case raceState.finishing:

                if (Game.Current.players.Count > 1)
                {
                    endingTimer -= Time.deltaTime;
                    race.text = endingTimer.ToString("F2");

                    foreach (GameObject animal in Game.Current.players)
                    {
                        if (animal.GetComponent<BodyPart>().canRace)
                        {
                            if (animal.transform.position.x > finishLine)
                            {
                                animal.GetComponent<BodyPart>().canRace = false;

                                if (!second)
                                {
                                    winText[animal.GetComponent<BodyPart>().playerID - 1].text = "2nd";
                                    second = true;
                                }
                                else if (!third)
                                {
                                    winText[animal.GetComponent<BodyPart>().playerID - 1].text = "3rd";
                                    third = true;
                                }
                                else
                                {
                                    winText[animal.GetComponent<BodyPart>().playerID - 1].text = "4th";
                                }
                            }
                        }
                    }

                    if (endingTimer < 0)
                    {
                        finishRace();
                    }
                }
                else
                {
                    finishRace();
                }

                break;

            case raceState.finished:

                gPad.updateStates(0);

                //Restart race
                if (gPad.isKeyDown(XKeyCode.A) || Input.GetKeyDown(KeyCode.U))
                {
                    ALoader.reset();
                    startRace();
                }

                //New creatures
                if (gPad.isKeyDown(XKeyCode.X) || Input.GetKeyDown(KeyCode.I))
                {
                    //Main menu at Test tubes
                    MainMenu._mMstate = MainMenuState.creatureCreation;
                    MainMenu.moveNow = true;
                    Application.LoadLevel("Main Menu");
                }

                //New planet
                if (gPad.isKeyDown(XKeyCode.Y) || Input.GetKeyDown(KeyCode.O))
                {
                    //Main menu at planet select
                    Application.LoadLevel("Main Menu");
                }

                //Quit
                if (gPad.isKeyDown(XKeyCode.B) || Input.GetKeyDown(KeyCode.P))
                {
                    Application.Quit();
                }

                break;
        }
	}

    private void pauseCheck()
    {
        if (gPad.isKeyDown(XKeyCode.Start) || Input.GetKeyDown(KeyCode.Escape))
        {
            pauseActive = !pauseActive;

            if (pauseActive)
            {
                Time.timeScale = 0;
                music.Pause();
                EndRaceGUI.SetActive(true);
                EndRaceGUI.GetComponent<Canvas>().sortingOrder = 111;
            }
            else
            {
                Time.timeScale = 1;
                music.Play();
                EndRaceGUI.SetActive(false);
                EndRaceGUI.GetComponent<Canvas>().sortingOrder = 1;
            }
        }

        if (pauseActive)
        {
            //Restart race
            if (gPad.isKeyDown(XKeyCode.A) || Input.GetKeyDown(KeyCode.U))
            {
                Time.timeScale = 1;
                ALoader.reset();
                startRace();
                pauseActive = false;
            }

            //New creatures
            if (gPad.isKeyDown(XKeyCode.X) || Input.GetKeyDown(KeyCode.I))
            {
                Time.timeScale = 1;
                //Main menu at Test tubes
                MainMenu._mMstate = MainMenuState.creatureCreation;
                MainMenu.moveNow = true;
                Application.LoadLevel("Main Menu");
            }

            //New planet
            if (gPad.isKeyDown(XKeyCode.Y) || Input.GetKeyDown(KeyCode.O))
            {
                Time.timeScale = 1;
                //Main menu at planet select
                MainMenu._mMstate = MainMenuState.planetSelect;
                MainMenu.moveNow = true;
                Application.LoadLevel("Main Menu");
            }

            //Quit
            if (gPad.isKeyDown(XKeyCode.B) || Input.GetKeyDown(KeyCode.P))
            {
                Application.Quit();
            }
        }
    }

    void finishRace()
    {
        endingTimer = 0;
        race.text = endingTimer.ToString("F2");
        rState = raceState.none;
        BodyPart.testing = false;
        StartCoroutine(fin());
    }

    IEnumerator changeTime(float targetTime, float speed)
    {
        slowing = true;
        
        while (Mathf.Abs(Time.timeScale - targetTime) > .01f)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetTime, speed);
            music.pitch = Mathf.Lerp(music.pitch, targetTime, speed);
            yield return null;
        }

        Time.timeScale = targetTime;
        music.pitch = targetTime;
    }

    IEnumerator fin()
    {
        yield return new WaitForSeconds(1);

        foreach(GameObject animal in Game.Current.players)
        {  
            Tools.setAllChildren(Actions.canRace, animal, true);
        }

        EndRaceGUI.SetActive(true);
        EndRaceGUI.GetComponent<Canvas>().sortingOrder = 111;

        for (int i = 0; i < Game.Current.players.Count; i++)
        {
            EndScreen[i].alpha = 1;
            Stats[(i * 5)].text = "distance managed: " + Game.Current.players[i].GetComponent<ScoringScript>().maxDistance.ToString("F2") + "m";
            //work out the average speed
            float speedSum = Game.Current.players[i].GetComponent<ScoringScript>().maxDistance / raceTimer;
            Stats[(i * 5) + 1].text = "average fastness: " + speedSum.ToString("F2") + "mps";
            Stats[(i * 5) + 2].text = "maximum upness: " + Game.Current.players[i].GetComponent<ScoringScript>().maxJumpHeight.ToString("F2") + "m";
            Stats[(i * 5) + 3].text = "airtime: " + Game.Current.players[i].GetComponent<ScoringScript>().timeSpentInAir.ToString("F2") + "s";
            Stats[(i * 5) + 4].text = "topsyturvytimes: " + Game.Current.players[i].GetComponent<ScoringScript>().timeUpsideDown.ToString("F2") + "s";
            Game.Current.players[i].GetComponent<ScoringScript>().reSet();
        }
        rState = raceState.finished;
    }

    void activate(GameObject obj)
    {
        Tools.setAllChildren(Actions.setGravity, obj, true);
        Tools.setAllChildren(Actions.setKinematic, obj, false);
        Tools.setAllChildren(Actions.canRace, obj, true);
    }

    public void addPlayer(GameObject limb)
    {

        //Add random Body
        int randBody = Random.Range(0, AssetManager.animBodies.Count);

        compAnims.Add(AssetManager.animBodies[randBody].Spawn());

        int last = compAnims.Count - 1;

        int minNumOfLimbs = (int)(4 * compAnims[last].transform.localScale.x);
        int maxNumOfLimbs = (int)(5 * compAnims[last].transform.localScale.x);

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

                    placeLeftAndRight(limb, compAnims[last], numOfLimbs);

                    break;
            }
        }

        //Set body up correctly
        compAnims[last].transform.position = new Vector3(Random.Range(minX, maxX), 20, Random.Range(minZ, maxZ));
        compAnims[last].transform.rotation = Quaternion.identity;

        Tools.setAllChildren(Actions.setGravity, compAnims[last], true, "Eyes");
        Tools.setAllChildren(Actions.canRace, compAnims[last], false);
        Tools.setAllChildren(Actions.setWiggle, compAnims[last], true);
        Tools.setAllChildren(Actions.setKinematic, compAnims[last], true);

        int randPlayer = Random.Range(1, 5);
        Tools.setAllChildren(Actions.setID, compAnims[last], randPlayer);
        Tools.setAllChildren(Actions.setMaterial, compAnims[last], AssetManager.playerMaterials[randPlayer - 1]);
        Tools.setAllChildren(Actions.setID, compAnims[last], 5);

        compAnims[last].rigidbody.constraints = RigidbodyConstraints.None;
        compAnims[last].AddComponent<AutoPlayer>();
    }

    void placeLeftAndRight(GameObject limb, GameObject parent, int numOfLimbs)
    {
        //Add new Limb instance
        GameObject newLimb = limb.Spawn(limb.transform.position, limb.transform.rotation);

        //Adjust its scale to vary slightly
        float rand = Random.Range(0.75f, 1.25f);
        newLimb.transform.localScale *= rand;
        newLimb.GetComponent<LegJoint>().scalingStrength = rand;

        //Remove hinges to allow the limb to be moved.
        Tools.removeHinges(newLimb);

        //Reset its local position
        newLimb.transform.parent = parent.transform;
        newLimb.transform.localPosition = Vector3.zero;

        /*Find it's X position by using the X extents of the parent object multiplied by its scale
        and the X Extents of the object multiplied by its scale*/
        float x = (parent.renderer.bounds.extents.x / parent.transform.localScale.x) +
            limb.renderer.bounds.extents.x * 1 / parent.transform.localScale.x;

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
        float y = Random.Range(-parent.renderer.bounds.extents.y / 2, 0) / 2 * parent.transform.localScale.x;

        //Use the parents Z extents to place the limbs along with the Ratio
        float z = Mathf.Lerp(-parent.renderer.bounds.extents.z * newLimb.renderer.bounds.size.z * 1.1f / parent.transform.localScale.x,
                       parent.renderer.bounds.extents.z * newLimb.renderer.bounds.size.z * 1.1f / parent.transform.localScale.x, lRRatio) * Random.Range(.9f, 1.1f);

        //Finally set the position
        newLimb.transform.localPosition = new Vector3(x, y, z);
        newLimb.rigidbody.isKinematic = false;
        StartCoroutine(Tools.reAddHinges(newLimb, RigidbodyConstraints.None, true));

    }
}
