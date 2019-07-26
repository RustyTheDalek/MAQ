using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

    float timeSecs = 0f;
    float timeMins = 0f;

    void Update()
    {   
        //Timer
        timeSecs += Time.deltaTime;
        if (timeSecs >= 60.0f)
        {
            timeMins++;
            timeSecs = 0.0f;
        }

    }

	void OnGUI(){

        //Time Display
        GUI.Label(new Rect (300, 0, 100, 50), "Time: ");
        GUI.Label(new Rect (350, 0, 100, 50), timeMins.ToString("F0"));
        GUI.Label(new Rect (360, 0, 100, 50), timeSecs.ToString("F0"));

    }

}