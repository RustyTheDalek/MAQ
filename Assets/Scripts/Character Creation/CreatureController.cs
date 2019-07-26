using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreatureController : PlayerObject
{

    Controller gPad;
    
    float turnForce = 1000;

    public bool canSpin = true, changed = false;

    public bool confirmed = false, rotatedX = false, rotatedY;

    Vector3 startPos;

    Ray playerRay;

    RaycastHit hit;

    public GameObject LeftArrow;
    public GameObject RightArrow;
    public GameObject pist;
    public GameObject Screen;

    public Image bodyImage;

    public AudioSource pistSound;

    public Vector3 rotation;

    public BodySize bSize;

    //Number relating to Body of animal
    public int bType = 0;

	// Use this for initialization
	void Start () {

        gPad = new Controller(playerID-1);

        startPos = transform.position;
	    
        if(Game.Current.players.Count < playerID)
        {   
            if (Screen)
            {
                Debug.Log(name + "2");
                Screen.GetComponent<Animate>().Gorunning();
            }

            this.enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {

        gPad.updateStates();

        switch(CreatureCreating.createState)
        {
            case CState.choosingBodies:

                if(!confirmed)
                {
                    if((gPad.leftStick.x > 0.2f || Input.GetKeyDown(KeyCode.D)) && !changed)
                    {
                        Debug.Log("moveLeft");
                        changeBody(true);
                        changed = true;
                        RightArrow.GetComponent<Animate>().Gorunning();
                    }

                    if(gPad.isKeyDown(XKeyCode.DRight))
                    {
                        changeBody(true);
                        RightArrow.GetComponent<Animate>().Gorunning();
                    }

                    if (gPad.leftStick.x < -0.2f && !changed || Input.GetKeyDown(KeyCode.A))
                    {
                        Debug.Log("moveRight");
                        changeBody(false);
                        changed = true;
                        LeftArrow.GetComponent<Animate>().Gorunning();
                    }

                    if (gPad.isKeyDown(XKeyCode.DLeft))
                    {
                        changeBody(false);
						LeftArrow.GetComponent<Animate>().Gorunning();
                    }

                    if (((gPad.leftStick.x <= 0.2f && gPad.leftStick.x >= -0.2f) || 
                        (Input.GetKeyUp(KeyCode.A) && Input.GetKeyUp(KeyCode.D))) 
                        && changed)
                    {
                        changed = false;
                    }
                }

                if ((gPad.isKeyDown(XKeyCode.A) || Input.GetKeyDown(KeyCode.Space))
                    && !confirmed)
                {
                    Debug.Log(playerID);
                    confirmed = true;
                }

                if ((gPad.isKeyDown(XKeyCode.B) || Input.GetKeyDown(KeyCode.LeftControl)) && confirmed)
                {
                    confirmed = false;
                }

                break;

            case CState.choosingSize:

                if(!confirmed)
                {
                    if((gPad.leftStick.x > 0.2f || Input.GetKeyDown(KeyCode.D)) && !changed)
                    {
                        changed = true;
                        changeSize(true);
                        RightArrow.GetComponent<Animate>().Gorunning();
                    }

                    if (gPad.isKeyDown(XKeyCode.DRight))
                    {
                        changeSize(true);
                        RightArrow.GetComponent<Animate>().Gorunning();
                    }

                    if (gPad.leftStick.x < -0.2f && !changed || Input.GetKeyDown(KeyCode.A))
                    {
                        changed = true;
                        changeSize(false);
                        LeftArrow.GetComponent<Animate>().Gorunning();
                    }

                    if (gPad.isKeyDown(XKeyCode.DLeft))
                    {
                        changeSize(false);
						LeftArrow.GetComponent<Animate>().Gorunning();
                    }

                    if (((gPad.leftStick.x <= 0.2f && gPad.leftStick.x >= -0.2f) || 
                        (Input.GetKeyUp(KeyCode.A) && Input.GetKeyUp(KeyCode.D))) 
                        && changed)
                    {
                        changed = false;
                    }
                }

                if ((gPad.isKeyDown(XKeyCode.A) || Input.GetKeyDown(KeyCode.Space))
                    && !confirmed)
                {
                    Debug.Log(playerID);
                    pistSound.Play();
                    pist.GetComponent<Animate>().Gorunning();
                    Screen.GetComponent<Animate>().Gorunning();
                    confirmed = true;
                }

                if ((gPad.isKeyDown(XKeyCode.B) || Input.GetKeyDown(KeyCode.LeftControl)) && confirmed)
                {
                    pistSound.Play();
                    pist.GetComponent<Animate>().GorunningBack();
                    Screen.GetComponent<Animate>().GorunningBack();
                    confirmed = false;
                }

                switch (bSize)
                {
                    case BodySize.small:

                        bodyImage.rectTransform.localScale = new Vector3(3, 3, 0);
                        break;

                    case BodySize.standard:

                        bodyImage.rectTransform.localScale = new Vector3(5, 5, 0);
                        break;

                    case BodySize.large:

                        bodyImage.rectTransform.localScale = new Vector3(7, 7, 0);
                        break;
                }

                //if (bSize == BodySize.small)
                //{
                //    Debug.Log(bSize);
                //    bodyImage.rectTransform.localScale = new Vector3(4, 4, 0);
                //}
                //else if (bSize == BodySize.standard)
                //{
                //    Debug.Log(bSize);
                //    bodyImage.rectTransform.localScale = new Vector3(8, 8, 0);
                //}
                //else if (bSize == BodySize.large)
                //{
                //    Debug.Log(bSize);
                //    bodyImage.rectTransform.localScale = new Vector3(12, 12, 0);
                //}


                break;
                 
            case CState.creating:

                if(canSpin)
                {
//                    rotation.x = Input.GetAxis("RightJoystickX_P" + playerID);
//                    rotation.y = Input.GetAxis("RightJoystickY_P" + playerID);
//
//                    rigidbody.AddTorque(transform.up * rotation.x * turnForce * Time.deltaTime, ForceMode.Impulse);
//                    rigidbody.AddTorque(transform.right * rotation.y * turnForce * Time.deltaTime, ForceMode.Impulse);

                    if ((gPad.rightStick.x > 0.5f || Input.GetKeyDown(KeyCode.RightArrow))
                        && !rotatedX)
                    {
                        transform.eulerAngles -= Vector3.up * 90;
                        rotatedX = true;
                    }

                    if ((gPad.rightStick.x < -0.5f || Input.GetKeyDown(KeyCode.LeftArrow))
                        && !rotatedX)
                    {
                        Destroy(this.hingeJoint);
                        transform.eulerAngles += Vector3.up * 90;
                        rotatedX = true;
                    }

                    if ((gPad.rightStick.x <= 0.5f && gPad.rightStick.x >= -0.5f)
                        || (Input.GetKeyUp(KeyCode.LeftArrow) || 
                            Input.GetKeyUp(KeyCode.RightArrow)) && rotatedX)
                    {
                        rotatedX = false;
                    }

                    if ((gPad.rightStick.y > 0.5f || Input.GetKeyDown(KeyCode.DownArrow))
                        && !rotatedY)
                    {
                        transform.eulerAngles += Vector3.forward * 90;
                        rotatedY = true;
                    }

                    if ((gPad.rightStick.y < -0.5f || Input.GetKeyDown(KeyCode.UpArrow))
                        && !rotatedY)
                    {
                        transform.eulerAngles -= Vector3.forward * 90;
                        rotatedY = true;
                    }

                    if ((gPad.rightStick.y <= 0.5f && gPad.rightStick.y >= -0.5f 
                        || (Input.GetKeyUp(KeyCode.UpArrow)) ||
                          Input.GetKeyUp(KeyCode.DownArrow)) && rotatedY)
                    {
                        rotatedY = false;
                    }
                }

                if(Input.GetKeyDown(KeyCode.R))
                {
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;
                    rigidbody.position = startPos;
                    StartCoroutine(RotateTo(Quaternion.identity, 0.5f));
                }
                break;
        }
	}

    void changeBody(bool forward)
    {
        if (forward)
        {
            if (bType == CreatureCreating.bodies.Count - 3)
            {
                bType = 0;
            }
            else
            {
                bType += 3;
            }
        }
        else
        {
            if (bType == 0)
            {
                bType = CreatureCreating.bodies.Count - 3;
            }
            else
            {
                bType -= 3;
            }
        }
    }

    void changeSize(bool forward)
    {
        if(forward)
        {
            //If on last
            if(bSize == BodySize.small)
            {
                //Make it first
                bSize = BodySize.standard;
            }
            else
            {
                bSize++;
            }
        }
        else
        {
            //If on first
            if (bSize == BodySize.standard)
            {
                //make it last
                bSize = BodySize.small;
            }
            else
            {
                bSize--;
            }
        }
    }

    public IEnumerator RotateTo(Quaternion tRot, float speed)
    {
        canSpin = false;

        while(Vector3.SqrMagnitude(transform.eulerAngles - tRot.eulerAngles) > 1)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation , tRot, speed);
            yield return null;
        }

        canSpin = true;
    }
}
