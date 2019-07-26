using UnityEngine;
using System.Collections;

public class Eyes : MonoBehaviour {

    bool moving = false;

    Vector2 leftLook, rightLook;

    float speed = 0.1f;

    public GameObject leftEye, rightEye;

    float timer = 0;

    const float LOOKFREQ = 4f;

    float edgeOfEye
    {
        get
        {
            return (.65f - transform.localScale.x / 4);
        }
    }

    public static float eyeLimit(float scale)
    {
        return 0.175f * scale;
    }

	// Use this for initialization
	void Start () {

        ConfigurableJoint[] cJoints = GetComponentsInChildren<ConfigurableJoint>();

        leftEye = cJoints[0].gameObject;
        rightEye = cJoints[1].gameObject;

        scaleEyes();
	}
	
	// Update is called once per frame
	void Update () {

        ////Cross-eyed
        //if (Input.GetKeyDown(KeyCode.Space))
        //{

        //    moving = true;
        //    timer = LOOKFREQ;

        //    leftLook = Vector2.right * edgeOfEye;
        //    rightLook = -Vector2.right * edgeOfEye;
        //}

        //if (timer > 0)
        //{
        //    timer -= Time.deltaTime;
        //}
        //else
        //{
        //    float randAngle = Random.value * 360;
        //    leftLook = new Vector2(Mathf.Sin(randAngle), Mathf.Cos(randAngle)) * ( edgeOfEye * Random.Range(.5f, 1.1f));
        //    rightLook = new Vector2(Mathf.Sin(randAngle), Mathf.Cos(randAngle)) * ( edgeOfEye * Random.Range(.5f, 1.1f));
        //    moving = true;

        //    timer = Random.Range(0, LOOKFREQ);
        //}
        
        //if (moving)
        //{
        //    moveEye(leftEye, leftLook, speed);
        //    moveEye(rightEye, rightLook, speed);
        //}
	}

    public void scaleEyes()
    {
        //Corrcetly set up limits for Goggly eyes when scaling
        SoftJointLimit limit;

        limit = leftEye.GetComponent<ConfigurableJoint>().linearLimit;

        limit.limit = 0.175f * transform.root.localScale.x;

        leftEye.GetComponent<ConfigurableJoint>().linearLimit = limit;
        rightEye.GetComponent<ConfigurableJoint>().linearLimit = limit;
    }


    void moveEye(GameObject eye, Vector2 loc, float t)
    {
        eye.rigidbody.useGravity = false;
        eye.transform.localPosition = Vector3.Lerp(eye.transform.localPosition, new Vector3(loc.x, eye.transform.localPosition.y, loc.y), t);

        if (Vector3.SqrMagnitude(eye.transform.localPosition - new Vector3(loc.x, eye.transform.localPosition.y, loc.y)) < 0.0000000000001)
        {
            moving = false;
            eye.rigidbody.useGravity = true;
        }
    }

}
