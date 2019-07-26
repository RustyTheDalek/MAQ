using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleCreationGUI : MonoBehaviour {

    public Text playerName, readyInfo, 
        AButton, 
        BButton, 
        XButton, 
        YButton;

	// Use this for initialization
    void Awake () {

        Object[] objs = GetComponentsInChildren<Text>();

        playerName = objs[0] as Text;
        readyInfo = objs[1] as Text;
        AButton = objs[2] as Text;
        BButton = objs[3] as Text;
        XButton = objs[4] as Text;
        YButton = objs[5] as Text;
    }
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {


	
	}

    public void setControls(bool testRoom)
    {
        if (testRoom)
        {
			if(YButton)
			{
	            YButton.text = "Test Tube";
	            XButton.text = "Move Knees";
	            AButton.text = "Contract Springs";
	            BButton.text = "Activate Motors";
			}
        }
        else
        {
			if(YButton)
			{
	            YButton.text = "To Testing";
	            XButton.text = "New Knee Animal";
	            AButton.text = "New Spring Animal";
	            BButton.text = "New Motor Animal";
			}
        }
    }

    public void setReady(bool ready)
    {
        if (ready)
            readyInfo.text = "to Un-ready";
        else
            readyInfo.text = "to Ready Up";

    }
}
