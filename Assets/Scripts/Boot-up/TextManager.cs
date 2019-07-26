/// <summary>
/// Text manager. Script to manage child slowText objects, allowing you to 
/// skip the text and move from screen to ship
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

    Controller gPad;

    //List of all slow text objects in child
    public SlowText[] sTexts;

    //Progress bar Panel
    Image loadingBar, loadingBarBackGround;

    //Whether text has finished
    public bool finished;

    //Audio for text addition
    public static AudioSource keyLog;

    AsyncOperation loadLevel;

	// Use this for initialization
	void Start () {

        gPad = new Controller(0);

        Screen.showCursor = false;

        //Referenced attached Audio Source
        keyLog = GetComponent<AudioSource>();

        //Load clip from Resources
        keyLog.clip = Resources.Load("Audio/beep") as AudioClip;

        //Retrieve all child slowText objects including inactive objects
        sTexts = GetComponentsInChildren<SlowText>(true);

        Image[] loadstuff = GetComponentsInChildren<Image>();

        loadingBarBackGround = loadstuff[1];
        loadingBar = loadstuff[2];

        loadingBar.gameObject.SetActive(false);
        loadingBarBackGround.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        gPad.updateStates();

        if (loadLevel != null)
        {
            loadingBar.fillAmount = loadLevel.progress;
        }

        //Allows player to skip if they wish
        if(gPad.isKeyDown(XKeyCode.A) || Input.GetKeyDown(KeyCode.Space))
        {
            if(!finished)
            {
                SlowText sText;
                //Finish text
                for(int i = 0 ; i <= sTexts.Length -1; i++)
                {
                    sText = sTexts[i];
                    //Finish of text early
                    sText.GetComponent<Text>().text = sText.finalMessage;
                    //Acviate gameObject
                    sText.gameObject.SetActive(true);
                    //Set blinkMax to 0 to prevent blinking on all but last text
                    if(i != sTexts.Length-1)
                        sText.blinkMax = 0;

                    Application.LoadLevelAsync("Main Menu");
                }
                finished = true;
            }
            else
            {
                //Find last text
                SlowText lastText = sTexts[sTexts.Length-1];

                //If Boot-up screen finished
                if(lastText && lastText.GetComponent<Text>().text.Contains
                   (lastText.finalMessage))
                {
                    loadingBar.gameObject.SetActive(true);
                    loadingBarBackGround.gameObject.SetActive(true);

                    Application.LoadLevelAsync("Main Menu");
                }
            }
        }
	}
}
