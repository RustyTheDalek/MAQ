using UnityEngine;
using System.Collections;

public class Angles : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    //Set angle for component
    public void SetAngle(float val)
    {
        if(transform.localEulerAngles.y != val)
        {   
            //If angle isn't 0 changing it slightly more complicated
            if(transform.localEulerAngles.y != 0)
            {
//                Debug.Log(transform.localEulerAngles.y);
                float diff = val - transform.localEulerAngles.y;

                transform.RotateAround(transform.parent.position, Vector3.right, diff);
            }
            else
            {
                transform.RotateAround(transform.parent.position, Vector3.right, val);  
            }
        }
        else
        {
            Debug.Log("Angle already set");
        }
    }
}
