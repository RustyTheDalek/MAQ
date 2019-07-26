using UnityEngine;
using System.Collections;

public class TrackCam : PlayerObject {

    //Gameobject to track
    public GameObject target;

    //Distance to maintain from target
    public float distance = 2;

    //Angle to view at target
    public Vector3 angle;

    public Vector3 playerOffset;

    public bool canRotate = false;

    public Vector3 targetOffset;

    protected Controller gPad;

    protected float turnSpeed = 80;

	// Use this for initialization
	void Start () {
	}

    public virtual void setUp(GameObject _Target, Rect viewRect) {

        target = _Target;

        playerID = target.GetComponent<BodyPart>().playerID;

        gPad = new Controller(playerID - 1);

        //using scale of object * 10 creates good viewport for each player
        distance = 20;

        this.camera.rect = viewRect;
    }
	
	// Update is called once per frame
	void Update () 
    {
        gPad.updateStates();

        transform.LookAt(target.transform.position);

        if (canRotate)
        {
            playerOffset += new Vector3(1 * gPad.rightStick.y, 1 * gPad.rightStick.x, 0) * Time.deltaTime * turnSpeed;

            playerOffset = new Vector3(Mathf.Clamp(playerOffset.x, -10, 20), playerOffset.y, 0);
            if (gPad.isKeyDown(XKeyCode.RightStick))
            {
                playerOffset = Vector3.zero;
            }
        }

        //Rotate using angle
        transform.localRotation = Quaternion.Euler(angle + playerOffset);

        //Update position
        //Vector3 pos = transform.rotation * new Vector3(0, 0, -distance)
        //    + target.transform.position;

        transform.position = transform.rotation * new Vector3(0, 0, -distance)
            + target.transform.position + targetOffset;
	}
}
