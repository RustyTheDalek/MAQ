using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterCreation : MonoBehaviour {

    public static CreationState creationState = CreationState.chooseState;

    Controller gPad;

    // set cutom GUI items
    public GUISkin SciFi_Skin;
    public CanvasGroup Char_Menu;
    public CanvasGroup Char_Stats;

    protected int generationChance = 4;

    public int menuA = 0;

    public TEST_bone tBone;
    public Text CharStats;
    public Text charTimerText;

    public bool tutStep3 = false;
    public static bool newAnimal = false;

    //Lists of players playing
    public List<playerPlaying> players;

    //Cameras for each player
    public List<LerpTrackCam> pCameras;

    static Object[] objs;

    //Menu objects for the Character creation
    public GameObject simpleCreationButton, advancedCreationButton, advancedCreationMenu;

    public static bool startSimpleNow = false, simpleReplace = true, attractOn = false;

    public List<GameObject> splitscreenGUI;

    void Awake()
    {
        Game.Current.totalPlayers = 1;
    }
	// Use this for initialization
	void Start () {

        gPad = new Controller(0);

        LegJoint.wiggleActive = true;

        Debug.Log(AssetManager.animBodies.Count);
        Debug.Log(AssetManager.animLimbs.Count);
	}
	
    void OnLevelWasLoaded(int level)
    {
    }

	// Update is called once per frame
	void Update () {

        gPad.updateStates();

        switch (creationState)
        {
            case CreationState.simple:

                if (gPad.isKeyDown(XKeyCode.Back) || Input.GetKeyDown(KeyCode.Escape))
                {
                    stopSimpleCreation();
                }

                //Foreach loop that checks all players that are playing and returns prematurely if one is not ready
                foreach (playerPlaying player in players)
                {
                    if (player.playing)
                    {
                        if (!player.ready)
                        {
                            return;
                        }
                    }
                }

                BodyPart.testing = false;

                Game.Current.players.Clear();

                foreach (playerPlaying player in players)
                {
                    if (player.playing)
                    {
                        player.player.rigidbody.angularDrag = player.angularDrag;
                        Tools.setAllChildren(Actions.setKinematic, player.player, true);
                        Tools.setAllChildren(Actions.setGravity, player.player, true);
                        Tools.setAllChildren(Actions.setWiggle, player.player, false);
                        Tools.setAllChildren(Actions.setOnGround, player.player, false);
                        player.player.rigidbody.useGravity = false;
                        player.player.rigidbody.constraints = RigidbodyConstraints.None;
                        Game.Current.players.Add(player.player);
                        DontDestroyOnLoad(Game.Current.players[Game.Current.players.Count - 1]);
                    }
                }

                creationState = CreationState.none;
                Application.LoadLevel("Terrain2");

                break;

            case CreationState.advanced:

                if (gPad.isKeyDown(XKeyCode.Back) || Input.GetKeyDown(KeyCode.Escape))
                    stopAdvancedCreation();

                charTimerText.text = Game.creationTime.ToString("F0");

                if ((gPad.leftTrigger != 0 || Input.GetKey(KeyCode.Q)) && Game.creationTime > 30)
                {
                    Game.creationTime--;
                }

                if ((gPad.rightTrigger != 0 || Input.GetKey(KeyCode.E)) && Game.creationTime < 120)
                {
                    Game.creationTime++;
                }

                break;

            case CreationState.chooseState:

                if (pCameras[0].enabled)
                {
                    simpleCreationButton.GetComponent<Button>().interactable = false;
                    advancedCreationButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    simpleCreationButton.GetComponent<Button>().interactable = true;
                    advancedCreationButton.GetComponent<Button>().interactable = true;
                }

                break;
        }

	}

    public void startSimpleCreation(bool replace, bool attract)
    {
        creationState = CreationState.simple;
        MainMenu._mMstate = MainMenuState.none;
        BodyPart.testing = true;
        LegJoint.wiggleActive = true;

        if(GetComponentInChildren<MoveDrone>())
        {
            GetComponentInChildren<MoveDrone>().gameObject.SetActive(false);
        }

        spawnPlayers(replace, attract);

        //Split screen based on Players playing and move them to the correct tube
        switch (Game.Current.totalPlayers)
        {
            case 1: //Singleplayer no screensplitting - Still need to move to Testube

                //Use Main camera for player one
                pCameras[0].enabled = true;
                pCameras[0].setUp(players[0].player, new Rect(0, 0, 1, 1));

                break;

            case 2:

                //Use Main camera for player one
                pCameras[0].enabled = true;
                pCameras[0].setUp(players[0].player, new Rect(0, 0.5f, 1, 0.5f));

                //2nd players controllers
                pCameras[1].gameObject.SetActive(true);
                pCameras[1].setUp(players[1].player, new Rect(0, 0, 1, 0.5f));

                splitscreenGUI[0].SetActive(true);

                break;

            case 3:

                //Use Main camera for player one
                pCameras[0].GetComponent<LerpTrackCam>().enabled = true;
                pCameras[0].GetComponent<LerpTrackCam>().setUp(players[0].player, new Rect(0, 0.5f, 0.5f, 0.5f));

                //2nd players controllers
                pCameras[1].gameObject.SetActive(true);
                pCameras[1].setUp(players[1].player, new Rect(0.5f, 0.5f, 0.5f, 0.5f));

                //3rd players controllers
                pCameras[2].gameObject.SetActive(true);
                pCameras[2].setUp(players[2].player, new Rect(0, 0, 0.5f, 0.5f));

                splitscreenGUI[1].SetActive(true);

                break;

            case 4:

                //Use Main camera for player one
                pCameras[0].GetComponent<LerpTrackCam>().enabled = true;
                pCameras[0].GetComponent<LerpTrackCam>().setUp(players[0].player, new Rect(0, 0.5f, 0.5f, 0.5f));

                //2nd players controllers
                pCameras[1].gameObject.SetActive(true);
                pCameras[1].setUp(players[1].player, new Rect(0.5f, 0.5f, 0.5f, 0.5f));

                //3nd players controllers
                pCameras[2].gameObject.SetActive(true);
                pCameras[2].setUp(players[2].player, new Rect(0, 0, 0.5f, 0.5f));

                //4th player
                pCameras[3].gameObject.SetActive(true);
                pCameras[3].setUp(players[3].player, new Rect(0.5f, 0, 0.5f, 0.5f));

                splitscreenGUI[2].SetActive(true);

                break;
        }
    }

    public void stopSimpleCreation()
    {
        creationState = CreationState.chooseState;
        MainMenu._mMstate = MainMenuState.creatureCreation;
        BodyPart.testing = false;
        LegJoint.wiggleActive = false;

        GetComponentsInChildren<MoveDrone>(true)[0].gameObject.SetActive(true);

        for (int i = 0; i < Game.Current.totalPlayers; i++)
        {
            players[i].setTesting(false);
            pCameras[i].reset();
            pCameras[i].playerCanvas.gameObject.SetActive(false);
        }

        Debug.Log(Game.Current.players.Count);

        foreach (GameObject obj in splitscreenGUI)
        {
            obj.SetActive(false);
        }
    }

    public void startAdvancedCreation()
    {
        foreach (GameObject player in Game.Current.players)
        {
            foreach (Transform child in player.transform)
            {
                child.Recycle();
            }
            player.Recycle();
        }

        Game.Current.players.Clear();

        simpleCreationButton.SetActive(false);
        advancedCreationMenu.SetActive(true);
        advancedCreationButton.SetActive(false);
        CharacterCreation.creationState = CreationState.advanced;
    }

    private void stopAdvancedCreation()
    {
        creationState = CreationState.chooseState;

        simpleCreationButton.SetActive(true);
        advancedCreationButton.SetActive(true);
        advancedCreationMenu.SetActive(false);

    }

    private void spawnPlayers(bool replace, bool attract)
    {
        for (int i = 0; i < Game.Current.totalPlayers; i++)
        {
            players[i].cam = pCameras[i];

            if (replace)
            {
                if(i < Game.Current.players.Count && Game.Current.players[i])
                {
                    players[i].player = Game.Current.players[i];
                    players[i].setup();
                }
                else
                {
                    Game.Current.players.Add(null);
                    DontDestroyOnLoad(Game.Current.players[i]);
                    players[i].addPlayer(AssetManager.animLimbs[Random.Range(0, AssetManager.animLimbs.Count)] as GameObject, false);
                }
            }
            else
            {
                if (Game.Current.players.Count > i && Game.Current.players[i])
                {
                    Debug.Log(Game.Current.players.Count);
                    //players[i].player = Game.Current.players[i];
                }
                else
                {
                    Debug.Log(Game.Current.players.Count);
                    Debug.Log("Adding new player");
                    Game.Current.players.Add(null);
                    DontDestroyOnLoad(Game.Current.players[i]);
                    players[i].addPlayer(AssetManager.animLimbs[Random.Range(0, AssetManager.animLimbs.Count)] as GameObject, false);
                }

                players[i].setup();
            }

            if (!attract)
            {
                players[i].cam.playerCanvas.gameObject.SetActive(true);
            }
            else
            {
                players[i].cam.playerCanvas.gameObject.SetActive(false);
                players[i].gameObject.AddComponent<AutoPlayerCylin>();
                players[i].player.gameObject.AddComponent<AutoPlayer>();
            }

            if (!pCameras[i].gameObject.activeSelf)
                pCameras[i].gameObject.SetActive(true);
        }
    }
}
