/// <summary>
/// Slow text. Script to have text slowly appear on screen
/// Created by Ian.J 14/05/15
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlowText : MonoBehaviour {

    //Final Message to be displayed
    public string finalMessage;

    //Text component this script is attached to
    Text screenMessage;

    public Text ScreenMessage
    {
        get
        {
            return screenMessage;
        }
    }

    //Delay for text to appear and cursor to blink
    const float textDelay = .06f, cursorBlinkDelay = .25f;

    //How much you want the cursor to blink;
    public int blinkMax = 3;

    //Count of how many times cursor has blinked;
    int blinkAmount = 0;

    //Timers for text and cursor
    float textTimer = 0, cursorTimer = 0;

    //Next slow Text we want to activate
    public SlowText nextText = null;

	// Use this for initialization
	void Start () {

        //Assign screenMessage to attached TextComponent
        screenMessage = GetComponent<Text>();
	
	}
	
	// Update is called once per frame
	void Update () {

        //If message still needs to be completed
        if(screenMessage.text.Length < finalMessage.Length)
        {
            if(textTimer <=0) //If ready to add another part of the message
            {
                //Add single character to Text component
                screenMessage.text += finalMessage[screenMessage.text.Length];
                //Reset timer
                textTimer = textDelay;

                TextManager.keyLog.Play();
            }
            else //Not ready to add next char so Reduce timer
            {
                textTimer -= Time.deltaTime;
            }
        }
        /*Message complete so add blink cursor so long as blinkMax has not been 
            reached*/
        else if(blinkAmount < blinkMax)
        {
            if(cursorTimer <= 0 ) //If ready too alternate blink
            {
                //Get index of last character
                int lastChar = screenMessage.text.Length-1;

                //If message ends with cursor
                if(screenMessage.text[lastChar] == '_')
                {
                    //Remove cursor 
                    screenMessage.text = screenMessage.text.Remove(lastChar);
                    //Increment amount of blinks
                    blinkAmount ++;
                }
                else //Add cursor
                {
                    screenMessage.text += "_";
                    TextManager.keyLog.Play();
                }
                cursorTimer = cursorBlinkDelay;
            }
            else //Not ready to blink so reduce timer
            {
                cursorTimer -= Time.deltaTime;
            }
        }
        else
        {
            if(nextText)
            {
                nextText.gameObject.SetActive(true);
            }
        }
	}
}
