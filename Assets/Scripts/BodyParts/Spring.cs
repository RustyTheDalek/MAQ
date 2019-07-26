using UnityEngine;
using System.Collections;

public class Spring : LegJoint {

    public float minSize = 0.25f, maxSize = 1;
    float minSpeed = 0.1f, maxSpeed = 0.4f;

    float newSize;

	// Use this for initialization
	protected void Start () {

        placePref = PlacePrefrence.LeftAndRight & PlacePrefrence.AboveAndBelow & PlacePrefrence.FrontAndBack;
        base.Start();
	}
	
    public void setScaling()
    {
        maxSize = transform.localScale.x;
        minSize = maxSize * .25f;
    }
	// Update is called once per frame
    protected void Update()
    {
        base.Update();
	}

    protected void FixedUpdate()
    {
        if(canRace)
        {
            if (testing)
            {
                if (gPad.isKey(XKeyCode.A) || Input.GetKey(KeyCode.DownArrow))
                {
                    lerpSize(minSize, minSpeed);
                }
                else if (transform.localScale.y < maxSize)
                {
                    lerpSize(maxSize, maxSpeed);

                    if (GetComponent<OnGround>().onGround)
                    {
                        float strength = 1 - transform.localScale.y * 1.125f;

                        strength = Mathf.Clamp(strength, 0, 1);

                        strength *= walkForce * scalingStrength;

                        rigidbody.AddForce(transform.up * strength * Time.deltaTime, ForceMode.Impulse);
                        rigidbody.AddForce(transform.root.forward * strength * 2 * transform.root.localScale.x * Time.deltaTime, ForceMode.Impulse);
                    }
                }
            }
        }
        else if (canWiggle)
        {
            if (wiggleLength > 0)
            {
                lerpSize(minSize, minSpeed);
            }
            else
            {
                lerpSize(maxSize, maxSpeed);

                float strength = 1 - transform.localScale.y * 1.125f;

                strength = Mathf.Clamp(strength, 0, 1);

                strength *= walkForce;

                rigidbody.AddForce(transform.up * strength * Time.deltaTime * transform.root.localScale.x, ForceMode.Impulse);
                rigidbody.AddForce(transform.root.forward * strength * 2 * Time.deltaTime, ForceMode.Impulse);
            }
        }
        else
        {
            lerpSize(maxSize, maxSpeed);
        }
    }

    void lerpSize(float to, float speed)
    {
        if(transform.localScale.y != to)
        {
            newSize = Mathf.Lerp(transform.localScale.y, to, speed);
            transform.localScale = Tools.setVector3(transform.localScale, "y", newSize);
            
            if(Mathf.Abs(transform.localScale.y - to) < 0.01f)
            {
                transform.localScale = Tools.setVector3(transform.localScale, "y", to);
            }
        }
    }
}
