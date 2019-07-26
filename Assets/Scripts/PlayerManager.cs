using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour 
{
    public int numberOfPlayers;

    protected bool p1_Ready;
    protected bool p2_Ready;
    protected bool p3_Ready;
    protected bool p4_Ready;

    public Vector3 gPadMovement1;
    public Vector3 gPadMovement2;
    public Vector3 gPadMovement3;
    public Vector3 gPadMovement4;

	// Use this for initialization
	void Start () 
    {
        numberOfPlayers = 1;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
//        if (Input.GetButtonDown("A_P1"))
//        {
//            p1_Ready = true;
//
//            //numberOfPlayers++;
//
//            gPadMovement1.x = Input.GetAxis ("LeftJoystickX_P1");
//            gPadMovement1.y = Input.GetAxis ("LeftJoystickY_P1");
//
//            transform.position += new Vector3((gPadMovement1.x * Time.deltaTime * 3), (gPadMovement1.y * Time.deltaTime * 3), 0);
//
//        }
//
//        if (Input.GetButtonDown("A_P2"))
//        {
//            p2_Ready = true;
//
//            //numberOfPlayers++;
//        }

	}
}
