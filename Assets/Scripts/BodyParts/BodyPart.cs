using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BodyPart : PlayerObject
{
    protected Controller gPad;

    Vector3 CentreOfMass;

    public static bool testing = false;

    private bool canPush = false;

    public bool canRace = true;

    public List<OnGround> parts;

    public bool turning;

    public float turningSpeed = 50;

    //Max speed for body type to rotate
    public float angularVelocMax;

    //Whether the animal is being tased
    bool tasing = false;

    private bool LSPressed = false, RSPressed = false;

    float taseTimer = 0, stickTimer = 0;

    const float maxTaseTime = 1f, STICKTIME = 0.35f;

    private AudioSource taseSound;

    public bool onGround
    {
        get
        {
            foreach (OnGround part in parts)
            {
                if (part.onGround)
                {
                    return true;
                }
            }

            return false;
        }
    }

#if UNITY_EDITOR

    protected float x, y;

#endif

	// Use this for initialization
	protected virtual void Start () {

        gPad = new Controller(playerID-1);

        taseSound = gameObject.AddComponent<AudioSource>();
        taseSound.clip = Resources.Load("Audio/Tase 1") as AudioClip;
        taseSound.playOnAwake = false;

        rigidbody.maxAngularVelocity = angularVelocMax;
        CentreOfMass = new Vector3(-0.1f, -0.4f, -0.8f);
        rigidbody.centerOfMass = CentreOfMass;

        parts = new List<OnGround>();
        parts.Clear();
        findonGround(gameObject);
	}
	
	// Update is called once per frame
	protected virtual void Update () {

        gPad.updateStates(playerID-1);

        if(BodyPart.testing && !GetComponent<LegJoint>() && canRace)
        {
            //If either left stick or right stick pressed
            if (gPad.isKeyDown(XKeyCode.LeftStick) || gPad.isKeyDown(XKeyCode.RightStick))
            {
                //If not already tasing and on ground (To prevent abuse)
                if (!tasing && onGround)
                {
                    //If not already waiting for other analouge
                    if (stickTimer <= 0)
                    {
                        //Save which stick has been pressed
                        if (gPad.isKeyDown(XKeyCode.LeftStick))
                        {
                            LSPressed = true;
                        }
                        else
                        {
                            RSPressed = true;
                        }

                        //start timer
                        stickTimer = STICKTIME;
                    }
                    else
                    {
                        if (LSPressed && gPad.isKeyDown(XKeyCode.RightStick))
                        {
                            tase();
                            stickTimer = 0;
                            LSPressed = false;
                            RSPressed = false;
                        }
                        else if (RSPressed && gPad.isKeyDown(XKeyCode.LeftStick))
                        {
                            tase();
                            stickTimer = 0;
                            LSPressed = false;
                            RSPressed = false;
                        }
                    }
                }
            }

            if (stickTimer > 0)
            {
                stickTimer -= Time.deltaTime;
            }

            if(tasing)
            {
                taseTimer -= Time.deltaTime;

                if(taseTimer <= 0)
                {
                    tase(gameObject, false);
                    tasing = false;
                }
            }

            rotationYLogic();

            turningLogic();

        }   
	}

    void tase()
    {
        taseSound.Play();
        StartCoroutine(gPad.setRumble(maxTaseTime, .5f));
        rigidbody.AddForce(Vector3.up * 375 * transform.childCount * rigidbody.mass/transform.localScale.x * Time.deltaTime, ForceMode.Impulse);

        tase(gameObject, true);

        tasing = true;

        taseTimer = maxTaseTime;
    }

    protected virtual void rotationYLogic()
    {
#if UNITY_EDITOR

        y = 0;

        if (Input.GetKey(KeyCode.W))
        {
            y = 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            y = -1;
        }

        rigidbody.AddTorque(transform.right * y * 100);
#endif
        rigidbody.AddTorque(transform.right * Mathf.Sign(gPad.leftStick.y) * Mathf.Pow(gPad.leftStick.y, 2) * 100);
    }

    protected virtual void turningLogic()
    {
#if UNITY_EDITOR

        if (Input.GetKey(KeyCode.A))
        {
            x = 1;
            turning = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            x = -1;
            turning = true;
        }

        if (Input.GetKeyUp(KeyCode.A) && Input.GetKeyUp(KeyCode.D))
        {
            turning = false;
        }

        rigidbody.AddTorque(transform.up * Mathf.Sign(gPad.leftStick.x) * Mathf.Pow(gPad.leftStick.x, 2) * turningSpeed);

#endif

        if (gPad.leftStick.x > 0.2f
           || gPad.leftStick.x < -0.2f)
        {
            turning = true;
        }

        if (gPad.leftStick.x <= 0.2f &&
           gPad.leftStick.x >= -0.2f)
        {
            turning = false;;
        }

        if(turning)
        {
            rigidbody.AddTorque(transform.up * Mathf.Sign(gPad.leftStick.x) * Mathf.Pow(gPad.leftStick.x, 2) * turningSpeed);
        }
    }
    
    public void findonGround(GameObject obj)
    {
        if (obj.GetComponent<OnGround>())
        {
            parts.Add(obj.GetComponent<OnGround>());
        }

        foreach(Transform child in obj.transform)
        {
            findonGround(child.gameObject);
        }
    }

    public void setPush(bool val)
    {
        canPush = val;
    }

    public void clearLegsOnGround()
    {
        parts.Clear();
    }

    void tase(GameObject obj, bool activate)
    {
        if(activate)
        {
            if(obj.GetComponent<LegJoint>() && obj.GetComponent<LegJoint>().walkForce > 0)
            {
                if(!obj.GetComponent<Taze>())
                {
                    obj.AddComponent<Taze>();
                }
            }
        }
        else
        {
            if(obj.GetComponent<LegJoint>())
            {
                Destroy(obj.GetComponent<Taze>());
            }
        }
        
        foreach(Transform child in obj.transform)
        {
            tase(child.gameObject, activate);
        }
    }
}
