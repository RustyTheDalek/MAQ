using UnityEngine;
using System.Collections;

public class ReverseKnee : LegJoint {
	
	// Update is called once per frame
    protected void Update () {
        
        if (testing && hingeJoint && canRace)
        {
            if (gPad.isKey(XKeyCode.X))
            {
//                Debug.Log("Backward");
                if (flip)
                {
                    rigidbody.AddTorque(transform.root.right * walkForce * Time.deltaTime * transform.root.localScale.x);
                } 
                else
                {
                    rigidbody.AddTorque(-transform.root.right * walkForce * Time.deltaTime * transform.root.localScale.x);
                }

                if(deltaHingeAngle > minMove)
                {
                    Debug.Log(name + "can push");
                    transform.root.rigidbody.AddForce(transform.root.forward * walkForce * 3.5f * Time.deltaTime * transform.root.localScale.x);
                }

                if(GetComponentInChildren<OnGround>() && 
                   GetComponentInChildren<OnGround>().onGround)
                {
                    rigidbody.AddForce(transform.up * upForce * Time.deltaTime * transform.root.localScale.x);
                }
            }
            
            if (gPad.isKey(XKeyCode.Y))
            {
//                Debug.Log("Forward");
                if (flip)
                {
                    rigidbody.AddTorque(-transform.root.right * walkForce * Time.deltaTime * transform.root.localScale.x);
                }
                else
                {
                    rigidbody.AddTorque(transform.root.right * walkForce * Time.deltaTime * transform.root.localScale.x);
                }

                if(deltaHingeAngle > minMove)
                {
                    Debug.Log(name + "can push");
                    transform.root.rigidbody.AddForce(transform.root.forward * walkForce * 3.5f * Time.deltaTime * transform.root.localScale.x);
                }

                if(GetComponentInChildren<OnGround>() && 
                   GetComponentInChildren<OnGround>().onGround)
                {
                    rigidbody.AddForce(transform.up * upForce * Time.deltaTime * transform.root.localScale.x);
                }
            }
        }

        base.Update();
    }
}
