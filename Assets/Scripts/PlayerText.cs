using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerText : PlayerObject {

    Text text;

    bool playerReady = false;

    public bool ready
    {
        get
        {
            return playerReady;
        }
    }
    Controller gPad;

	// Use this for initialization
	void Start () {

        text = GetComponent<Text>();
        text.color = playerColour / 255;

        gPad = new Controller(playerID-1);
	}
	
	// Update is called once per frame
	void Update () {

        gPad.updateStates();

        if (gPad.isKeyDown(XKeyCode.Start))
        {
            if (!playerReady)
            {
                playerReady = true;
            }
            else
            {
                playerReady = false;
            }
        }

        if (playerReady)
        {
            text.text = playerName + " is Ready";
        }
        else
        {
            text.text = playerName;
        }
	}
}
