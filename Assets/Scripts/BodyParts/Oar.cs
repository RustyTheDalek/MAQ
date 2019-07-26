using UnityEngine;
using System.Collections;

public class Oar : LegJoint {

    float minX, minY, minZ,
    maxX, maxY, maxZ;

	// Use this for initialization
    protected void Start()
    {
	}
	
	// Update is called once per frame
    protected void Update()
    {

        if(testing)
        {
//            movement.x = Input.GetAxis ("LeftJoystickX_P" + playerID);
//            movement.y = Input.GetAxis ("LeftJoystickY_P" + playerID);
//            
//            if(Input.GetAxis ("LeftJoystickY_P" + playerID) < 0f && GetComponentInChildren<OnGround>().onGround)
//            {
//                transform.root.rigidbody.AddForce(Vector3.up * walkForce * Time.deltaTime * transform.root.localScale.x, ForceMode.Impulse);
//            }

//            if(Input.GetAxis ("LeftJoystickY_P" + playerID) > 0f && GetComponentInChildren<OnGround>().onGround)
//            {
//                GetComponent<ConfigurableJoint>().connectedBody.AddForce(-Vector3.up * walkForce * Time.deltaTime * transform.root.localScale.x, ForceMode.Impulse);
//
//                oarPush = true;
//            }
            
            if(deltaHingeAngle > minMove && 
               deltaHingeAngle2 > minMove2)
            {
                transform.root.rigidbody.AddForce(Vector3.forward * walkForce * 75 * Time.deltaTime * transform.root.localScale.x);
            }

            rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, 20);
        }

        base.Update();
	}

    void FixedUpdate()
    {

    }
}