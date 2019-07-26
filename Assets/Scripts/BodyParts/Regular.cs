/// <summary>
/// Standard class inherited from Bodypart class. 
/// Used in conjunction with Standard body shape and allows body part to be self righted using Upright script
/// Created by - Ian Jones
/// </summary>
using UnityEngine;
using System.Collections;

public class Regular : BodyPart {

    //Whether the player is trying to right the body
    bool righting;

    public float rightingSpeed = 250;
    //Referncce to upright script used to test whether obj is upside or not
    private Upright upright;

    public float dRatio;

    //Determines strength of rolling - Being upside down results in a stronger force than when in the air
    float rollFactor;

	// Use this for initialization
	protected override void Start () {

        upright = GetComponent<Upright>();
        
        base.Start();
	
	}
	
	// Update is called once per frame
    protected override void Update()
    {
        dRatio = upright.DownRatio;

        base.Update();
	}

    protected override void turningLogic()
    {

#if UNITY_EDITOR

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (onGround && upright.upright)
            {
                turning = true;
                righting = false;
            }
            else if(!onGround)
            {
                righting = true;
                turning = false;
            }

            if (Input.GetKey(KeyCode.A))
            {
                x = 1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                x = -1;
            }
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            turning = false;
            righting = false;
        }

        if (turning)
        {
            rigidbody.AddTorque(transform.up * -x / 4 * turningSpeed);
        }
        else if (righting)
        {
            rigidbody.AddTorque(transform.forward * x * transform.childCount * (12 - upright.DownRatio) * rightingSpeed);
        }
#endif

        if (gPad.leftStick.x > 0.2f || gPad.leftStick.x < -0.2f)
        {
            //If right way up then turn as usual 
            if(onGround && upright.upright)
            {
                turning = true;
                righting = false;
            }
            //If upside down the begin self righting OR sideways and still trying to right
            else if (!onGround || upright.upsideDown || (upright.sideways && righting) || (upright.sideways && rigidbody.velocity.sqrMagnitude < 2))
            {
                if (!onGround)
                {
                    rollFactor = .1f;
                }
                else
                {
                    rollFactor = 1;
                }

                righting = true;
                turning = false;
            }
        }

        if (gPad.leftStick.x <= 0.2f &&
           gPad.leftStick.x >= -0.2f)
        {
            turning = false;
            righting = false;
        }
        
        if(turning)
        {
            rigidbody.AddTorque(transform.up * Mathf.Sign(gPad.leftStick.x) * Mathf.Pow(gPad.leftStick.x, 2) * turningSpeed);
        }
        else if(righting)
        {
            rigidbody.AddTorque(transform.forward * -Mathf.Sign(gPad.leftStick.x) * Mathf.Pow(gPad.leftStick.x, 2) * transform.childCount * (12 - upright.DownRatio) * rightingSpeed * rollFactor); 
        }
    }

    public void turningLogic(Vector2 leftStick)
    {
        if (leftStick.x > 0.2f || leftStick.x < -0.2f)
        {
            //If right way up then turn as usual 
            if (onGround && upright.upright)
            {
                turning = true;
                righting = false;
            }
            //If upside down the begin self righting OR sideways and still trying to right
            else if (!onGround || upright.upsideDown || (upright.sideways && righting) || 
                (upright.sideways && rigidbody.velocity.sqrMagnitude < 2))
            {
                if (!onGround)
                {
                    rollFactor = .1f;
                }
                else
                {
                    rollFactor = 1;
                }

                righting = true;
                turning = false;
            }
        }

        if (leftStick.x <= 0.2f &&
           leftStick.x >= -0.2f)
        {
            turning = false;
            righting = false;
        }

        if (turning)
        {
            rigidbody.AddTorque(transform.up * Mathf.Sign(leftStick.x) * Mathf.Pow(leftStick.x, 2) * turningSpeed);
        }
        else if (righting)
        {
            rigidbody.AddTorque(transform.forward * -Mathf.Sign(leftStick.x) * Mathf.Pow(leftStick.x, 2) * transform.childCount * (12 - upright.DownRatio) * rightingSpeed * rollFactor);
        }
    }
}
