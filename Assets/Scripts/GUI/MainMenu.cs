using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

    Controller gPad;

    public CharacterCreation charCreation;
    public LevelSelect lSelect;

    public string levelName;
    public int pauseMenuA = 0;

    protected bool audio_Open = false;
    protected bool gamePlay_Open = false;
    protected bool controls_Open = false;
    protected int options = 0;

    public CanvasGroup Menu_Canvas;
    public CanvasGroup Audio_Canvas;
    public CanvasGroup GamePlay_Canvas;
    public CanvasGroup Controls_Canvas;
    public CanvasGroup Tutorial_Canvas;

    public Terrain terrain;

    public Slider audioSlide;
   
    bool tutStart = true;
    bool moving;

    Vector3 target;
    static Vector3 planetSelect = new Vector3(221, -40, -182);
    static Vector3 testTubes = new Vector3(15, -40, -182);

    public MainMenuState mMstate
    {
        get
        {
            return _mMstate;
        }
        set
        {
            _mMstate = value;
        }
    }

    public static MainMenuState _mMstate = MainMenuState.none;

    public static bool moveNow = false;

    const float IDLETIME = 60;
    float attractTimer = 0;

    public static bool attract;

    public Canvas AttractGUI;

	// Use this for initialization
	void Start () {

        attractTimer = IDLETIME;
        if (mMstate == MainMenuState.none)
        {
            mMstate = MainMenuState.planetSelect;
        }

        gPad = new Controller(0);

        Audio_Canvas.alpha = (0f);
        Audio_Canvas.interactable = false;
        Audio_Canvas.blocksRaycasts = false;

        Menu_Canvas.alpha = (0f);
        Menu_Canvas.interactable = false;
        Menu_Canvas.blocksRaycasts = false;

        GamePlay_Canvas.alpha = (0f);
        GamePlay_Canvas.interactable = false;
        GamePlay_Canvas.blocksRaycasts = false;

        Controls_Canvas.alpha = (0f);
        Controls_Canvas.interactable = false;
        Controls_Canvas.blocksRaycasts = false;

#if !UNITY_EDITOR
        Screen.showCursor = false;
#else
        Screen.showCursor = true;
#endif
	}
	
	// Update is called once per frame
	void Update () {

        gPad.updateStates();

        if (!Input.anyKeyDown && !gPad.anyKeyDown)
        {
            if (attractTimer < 0)
            {
                if(!attract)
                    attractState(true);
            }
            else
            {
                attractTimer -= Time.deltaTime;
            }
        }
        else
        {
            if (attract)
            {
                attractState(false);
            }
            else
            {
                attractTimer = IDLETIME;
            }
        }

        if (gPad.isKeyDown(XKeyCode.A) || Input.GetKeyDown(KeyCode.Space) && tutStart)
        {
            Tutorial_Canvas.alpha = (0f);
            Tutorial_Canvas.interactable = false;
            Tutorial_Canvas.blocksRaycasts = false;
            tutStart = false;
        }

        MoveDrone.activeMenu = pauseMenuA;

        //if (Input.GetKeyDown(KeyCode.Escape) && options == 0)
        //{
        //    if(options == 0)
        //    {
        //        Menu_Canvas.alpha = (1f);
        //        Menu_Canvas.interactable = true;
        //        Menu_Canvas.blocksRaycasts = true;

        //        options = 1;
        //        pauseMenuA = 1;

        //        Screen.showCursor = true;
        //    }
        //    else
        //    {
        //        pauseMenuA = 0;
        //        CloseAllMenus();
        //    }
        //}

        if (Input.GetKeyUp(KeyCode.Z) || gPad.isKeyDown(XKeyCode.LeftShoulder))
        {
            //If at Creature Creation
            if (mMstate == MainMenuState.creatureCreation)
            {
                mMstate = MainMenuState.planetSelect;
            }

        }

        if (Input.GetKeyUp(KeyCode.C) || gPad.isKeyDown(XKeyCode.RightShoulder))
        {
            //If at Planet Select
            if (mMstate == MainMenuState.planetSelect)
            {
                mMstate = MainMenuState.creatureCreation;
            }
        }

        switch (mMstate)
        {
            case MainMenuState.planetSelect:

                if (Vector3.Distance(this.transform.position, planetSelect) >= 0.1)
                {
                    moving = true;
                    target = planetSelect;
                }
                break;

            case MainMenuState.creatureCreation:

                if (Vector3.Distance(this.transform.position, testTubes) >= 0.1)
                {
                    moving = true;
                    target = testTubes;
                }
                break;
        }

        if(moving)
        {
            if (!moveNow)
            {
                if (Vector3.Distance(this.transform.position, target) >= 0.1)
                {
                    transform.position = Vector3.Lerp(transform.position, target, 0.25f);

                    Color tCol = terrain.materialTemplate.GetColor("_WireColor");

                    float a = Tools.RangeConvert(transform.position.x, testTubes.x, planetSelect.x, 0, 1);

                    Color newCol = new Color(tCol.r, tCol.g, tCol.b, Mathf.Lerp(tCol.a, Mathf.Pow(a, 4), 1));

                    terrain.materialTemplate.SetColor("_WireColor", newCol);
                }
                else
                {
                    transform.position = target;
                    moving = false;
                }
            }
            else
            {
                transform.position = target;

                Color tCol = terrain.materialTemplate.GetColor("_WireColor");
                float a = Tools.RangeConvert(transform.position.x, testTubes.x, planetSelect.x, 0, 1);
                Color newCol = new Color(tCol.r, tCol.g, tCol.b, a);
                terrain.materialTemplate.SetColor("_WireColor", newCol);

                moving = false;
                moveNow = false;

                if (CharacterCreation.startSimpleNow)
                {
                    charCreation.startSimpleCreation(CharacterCreation.simpleReplace, CharacterCreation.attractOn);
                    CharacterCreation.startSimpleNow = false;
                    CharacterCreation.simpleReplace = true;
                    CharacterCreation.attractOn = false;
                }
            }
        }
	}

    private void attractState(bool activate)
    {
        if (activate)
        {
            _mMstate = MainMenuState.creatureCreation;
            moveNow = true;
            moving = true;
            Game.Current.totalPlayers = 1;
            CharacterCreation.startSimpleNow = true;
            CharacterCreation.simpleReplace = true;
            CharacterCreation.attractOn = true;
            LegJoint.wiggleActive = true;
            attract = true;
            AttractGUI.active = true;
        }
        else
        {
            //_mMstate = MainMenuState.planetSelect;
            //moveNow = true;
            //moving = true;
            //Game.Current.totalPlayers = 1;
            CharacterCreation.creationState = CreationState.chooseState;
            attract = false;
            AttractGUI.active = false;
            Application.LoadLevel("Main Menu");
        }
    }

    public void OnClickGamePlay()
    {
        GamePlay_Canvas.alpha = (1f);
        GamePlay_Canvas.interactable = true;
        GamePlay_Canvas.blocksRaycasts = true;

        gamePlay_Open = true;
        
         if (controls_Open == true)
        {
            Controls_Canvas.alpha = (0f);
            Controls_Canvas.interactable = false;
            Controls_Canvas.blocksRaycasts = false;

            controls_Open = false;
        }

        if (audio_Open == true)
        {
            Audio_Canvas.alpha = (0f);
            Audio_Canvas.interactable = false;
            Audio_Canvas.blocksRaycasts = false;

            audio_Open = false;
        }
    }

    public void OnClickGamePlayBack()
    {
        GamePlay_Canvas.alpha = (0f);
        GamePlay_Canvas.interactable = false;
        GamePlay_Canvas.blocksRaycasts = false;

        gamePlay_Open = false;
    }

    public void OnClickControls()
    {
        Controls_Canvas.alpha = (1f);
        Controls_Canvas.interactable = true;
        Controls_Canvas.blocksRaycasts = true;

        controls_Open = true;
       
        if (gamePlay_Open == true)
        {
            GamePlay_Canvas.alpha = (0f);
            GamePlay_Canvas.interactable = false;
            GamePlay_Canvas.blocksRaycasts = false;

            gamePlay_Open = false;
        }
        
        if (audio_Open == true)
        {
            Audio_Canvas.alpha = (0f);
            Audio_Canvas.interactable = false;
            Audio_Canvas.blocksRaycasts = false;

            audio_Open = false;
        }
    }

    public void OnClickControlsBack()
    {
        Controls_Canvas.alpha = (0f);
        Controls_Canvas.interactable = false;
        Controls_Canvas.blocksRaycasts = false;

        controls_Open = false;
    }

    public void OnClickBack()
    {
        CloseAllMenus();
    }

    public void OnClickAudio()
    {
        Audio_Canvas.alpha = (1f);
        Audio_Canvas.interactable = true;
        Audio_Canvas.blocksRaycasts = true;

        audio_Open = true;



        if (gamePlay_Open == true)
        {
            GamePlay_Canvas.alpha = (0f);
            GamePlay_Canvas.interactable = false;
            GamePlay_Canvas.blocksRaycasts = false;
            
            gamePlay_Open = false;
        }
        
        if (controls_Open == true)
        {
            Controls_Canvas.alpha = (0f);
            Controls_Canvas.interactable = false;
            Controls_Canvas.blocksRaycasts = false;
            
            controls_Open = false;
        }
    }

    public void OnClickAudioBack()
    {
        Audio_Canvas.alpha = (0f);
        Audio_Canvas.interactable = false;
        Audio_Canvas.blocksRaycasts = false;

        audio_Open = false;
    }

    void CloseAllMenus()
    {
        Menu_Canvas.alpha = (0f);
        Menu_Canvas.interactable = false;
        Menu_Canvas.blocksRaycasts = false;
        
        Audio_Canvas.alpha = (0f);
        Audio_Canvas.interactable = false;
        Audio_Canvas.blocksRaycasts = false;
        
        GamePlay_Canvas.alpha = (0f);
        GamePlay_Canvas.interactable = false;
        GamePlay_Canvas.blocksRaycasts = false;
        
        Controls_Canvas.alpha = (0f);
        Controls_Canvas.interactable = false;
        Controls_Canvas.blocksRaycasts = false;
        
        options = 0;
        pauseMenuA = 0;
        
        audio_Open = false;
        controls_Open = false;
        gamePlay_Open = false;
    }

    public void SetVolume()
    {
        AudioListener.volume = audioSlide.value;
    }

    public void loadCreatCreation()
    {
        Application.LoadLevel("NewCharCreation");
    }
}
