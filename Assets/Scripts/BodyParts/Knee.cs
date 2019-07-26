using UnityEngine;
using System.Collections;

public class Knee : LegJoint {

    public float forwardSpeed = 2;

    Vector3 walkDir;

    private float walkTimer = 0;

    private float MAXWalkTime = .5f;

    private float flipBias = 1;

	// Use this for initialization
	protected void Start () 
    {
        rigidbody.maxAngularVelocity = angularVelocMax;

        walkDir = transform.right;

        placePref = PlacePrefrence.LeftAndRight & PlacePrefrence.AboveAndBelow;
        base.Start();
	}
	
	// Update is called once per frame
	protected void Update () 
    {
        base.Update();
	}

    void FixedUpdate()
    {
        if(canRace)
        {
            if (testing)
            {
                if (gPad.isKeyDown(XKeyCode.X) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    flipBias *= -1;
                    walkTimer = MAXWalkTime;
                }

                if (walkTimer > 0)
                {
                    rigidbody.AddTorque(transform.right * flipBias * walkForce * scalingStrength * Time.deltaTime);

                    if (GetComponentInChildren<OnGround>().onGround)
                    {
                        transform.root.rigidbody.AddForce(Vector3.up * upForce * Time.deltaTime);

                        if (rigidbody.angularVelocity.sqrMagnitude > minMove)
                        {
                            /*Always add a force with a bit of up to keep Animals with knee 
                            joints stable*/
                            Vector3 dir = new Vector3(transform.root.forward.x, 1f,
                                transform.root.forward.z);

                            transform.root.rigidbody.AddForce(dir * walkForce * forwardSpeed
                                * scalingStrength * Time.deltaTime);
                        }
                    }

                    walkTimer -= Time.deltaTime;
                }
                else
                {
                    walkTimer = 0;
                }
            }
        }
        else if(hingeJoint && canWiggle && wiggleLength > 0)
        {
            if(hingeJoint.angle != hingeJoint.limits.min && hingeJoint.angle != hingeJoint.limits.max)
            {
                rigidbody.AddTorque(wiggleDir * walkForce * scalingStrength * transform.root.localScale.x);

                if (GetComponentInChildren<OnGround>().onGround)
                {
                    transform.root.rigidbody.AddForce(Vector3.up * upForce * Time.deltaTime);

                    if (rigidbody.angularVelocity.sqrMagnitude > minMove)
                    {
                        /*Always add a force with a bit of up to keep Animals with knee 
                        joints stable*/
                        Vector3 dir = new Vector3(transform.root.forward.x, .85f,
                            transform.root.forward.z);

                        transform.root.rigidbody.AddForce(dir * walkForce * forwardSpeed
                            * scalingStrength * Time.deltaTime);
                    }
                }
            }
        }
    }
}
