using UnityEngine;
using System.Collections;

public class LegJoint : BodyPart {

    HingeJoint hinge;

    JointMotor forward, backward;

    public bool flip, moving, controlled;

    public static bool wiggleActive = true;

    public bool canWiggle = false;

    //NOTE: minMove for Knee Joint 12.75f
    public float walkForce, upForce, breakForce, breakTorque, minMove, minMove2;

    public float scalingStrength
    {
        get
        {
            if (scalingStr != 0)
                return scalingStr;
            else
                return 1;
        }
        set
        {
            if (value != 0)
            {
                scalingStr = value; 
            }
        }
    }

    protected float scalingStr = 1;

    public Vector3 restAngle;

    public JointSpring spring;

    public JointLimits limits;

    public float wiggleTimer = 0, currentWiggleTimer = 0, 
                 wiggleLength = 0, currentWiggleLength = 0;

    public const float MINWiggleLength = 0.5f, MAXWiggleTimer = 5;

    public Vector3 wiggleDir;

    float prevHingeAngle, prevHingeAngle2;

    public PlacePrefrence placePref;

    public float deltaHingeAngle
    {
        get
        {
            if(hingeJoint)
            {
                return Mathf.Abs(hingeJoint.angle - prevHingeAngle);
            }
            else if(GetComponent<ConfigurableJoint>())
            {
//                Debug.Log(Mathf.Abs(transform.localEulerAngles.y - prevHingeAngle));
                return Mathf.Abs(transform.localEulerAngles.y - prevHingeAngle);
            }
            else
            {
                Debug.LogError("No Hinge or Configurable Joint");
                return 0;
            }
        }
    }

    public float deltaHingeAngle2
    {
        get
        {
            return Mathf.Abs(transform.localRotation.z - prevHingeAngle2);
        }
    }

    //Parent bodys upright script
    protected Upright PUp
    {
        get
        {
            return GetComponentInParent<Upright>();
        }
    }

	// Use this for initialization
	protected override void Start () {

        gPad = new Controller(playerID - 1);

        currentWiggleTimer = MAXWiggleTimer;

        currentWiggleLength = MINWiggleLength;
    }
	
	// Update is called once per frame
	protected void Update () {
	
        if(gameObject.activeSelf && (hingeJoint || GetComponent<ConfigurableJoint>()))
        {
            if(canRace)
            {
                gPad.updateStates(playerID-1);

                if(walkForce > 0)
                {
                    if(hingeJoint)
                    {
                        if(deltaHingeAngle > minMove)
                        {
                            transform.root.GetComponent<BodyPart>().setPush(true);

                        }
                    }
                    else
                    {
                        if(deltaHingeAngle > minMove && 
                           deltaHingeAngle2 > minMove2)
                        {
                            transform.root.GetComponent<BodyPart>().setPush(true);

                        }
                    }

                    if(hingeJoint)
                    {
                        prevHingeAngle = hingeJoint.angle;
                    }
                    else
                    {
                        prevHingeAngle = transform.localEulerAngles.y;
                        prevHingeAngle2 = transform.localEulerAngles.z;
                    }
                }
            }
            else if(canWiggle && wiggleActive && walkForce > 0)
            {
                //Randomly move
                if(wiggleTimer > 0)
                {
                    wiggleTimer -= Time.deltaTime;

//                    if(!hingeJoint.useSpring)
//                    {
//                        Tools.setSpring(gameObject, true);
//                    }
                }
                else
                {
                    //Reset Wiggle Timer
                    wiggleTimer = Random.Range(0, currentWiggleTimer);
//                    Debug.Log("Stop wiggling for " + wiggleTimer);

                    //Reduce time for next wiggle by a max of 3/4
//                    currentWiggleTimer *= Random.Range(0.75f, 1);

                    currentWiggleTimer = Mathf.Clamp(
                        currentWiggleTimer, MAXWiggleTimer/10, MAXWiggleTimer);
//                    Debug.Log("Next max Wiggle timer " + currentWiggleTimer);

                    //Set Time for wiggling
                    wiggleLength = Random.Range(0, MINWiggleLength);

                    //Increase wiggleLength
//                    currentWiggleLength *= Random.Range(1, 1.25f);

//                    currentWiggleLength = Mathf.Clamp(
//                        currentWiggleLength, MINWiggleLength, MINWiggleLength*10);

                    //Set random direction
                    setWiggleDir();
//                    Debug.Log("Start wiggling for " + wiggleLength);
                }

                if(wiggleLength > 0)
                {
                    wiggleLength -= Time.deltaTime;
                }
            }
        }
	}

    public virtual void setWiggleDir()
    {
        if(Random.value > 0.5f)
        {
            wiggleDir = transform.right;
        }
        else
        {
            wiggleDir = -transform.right;
        }
    }

    float AngleClamp(float val)
    {
        if (val > 360)
        {
            return val - 360;
        } 
        else
        {
            return val;
        }
    }
}
