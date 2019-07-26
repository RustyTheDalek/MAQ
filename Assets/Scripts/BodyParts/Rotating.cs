using UnityEngine;
using System.Collections;

public class Rotating : LegJoint {

    //Store axis of rotation for hinge when removing and destroying
    public Vector3 axis;

    public float forwardSpeed = 20;
	// Use this for initialization
	void Start () {
	
        rigidbody.maxAngularVelocity = angularVelocMax;

        placePref = PlacePrefrence.LeftAndRight;

        base.Start();
	}
	
	// Update is called once per frame
    void Update()
    {
        base.Update();
    }

	void FixedUpdate () {

        if(canRace)
        {
            if (testing)
            {
                if (gPad.isKey(XKeyCode.B) || Input.GetKey(KeyCode.RightArrow))
                {
                    rigidbody.AddTorque(-transform.up * Mathf.Sign(transform.localPosition.x)
                        * walkForce * scalingStrength * Time.deltaTime);

                    if (GetComponentInChildren<OnGround>().onGround &&
                        rigidbody.angularVelocity.sqrMagnitude > minMove &&
                        transform.root.GetComponent<Upright>().upright)
                    {
                        transform.root.rigidbody.AddForce(PUp.uprightF *
                            transform.root.forward * walkForce * scalingStrength *
                            forwardSpeed * Time.deltaTime);
                    }
                }
            }
        }
        else if(canWiggle && wiggleLength > 0)
        {
            rigidbody.AddTorque(wiggleDir * walkForce * 
                Mathf.Sign(transform.localPosition.x) * scalingStrength * 
                transform.root.localScale.x);

            if (GetComponentInChildren<OnGround>().onGround &&
                    rigidbody.angularVelocity.sqrMagnitude > minMove &&
                    transform.root.GetComponent<Upright>().upright)
            {
                transform.root.rigidbody.AddForce(PUp.uprightF *
                    transform.root.forward * walkForce * scalingStrength *
                    forwardSpeed * Time.deltaTime);
            }
        }

        base.Update();
	}

    public override void setWiggleDir()
    {
        if(Random.value > 0.5f)
        {
            wiggleDir = transform.up;
        }
        else
        {
            wiggleDir = -transform.up;
        }
    }
}
