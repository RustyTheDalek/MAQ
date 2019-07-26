//Simple script to act as magnet, using forces to slowly move object towards target position with given strength
using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour {

    public Vector3 targetPos;

    public float strength;

    Vector3 vToTarget, direction;

    float distance;

    Transform from;

    Quaternion targetRot;

    // Maximum turn rate in degrees per second.
    public float turningRate = 30f;

    void start()
    {
        targetRot = transform.rotation;
    }
	
	// Update is called once per frame
    void Update()
    {
        if(!rigidbody.isKinematic)
        {
            //Find Vector towards the target
            vToTarget = targetPos - transform.position;

            //Find distance
            distance = Mathf.Abs(vToTarget.magnitude);

            //Get normalised direction
            direction = Vector3.Normalize(vToTarget);

            //Apply force in that direction using distance with added strength force
            rigidbody.AddForce(direction * distance * strength);

            //Clamp speed of rigidbody using distance (with a little boost) to exponetially reduce speed
            rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, distance * 10);

            // Turn towards target rotation.
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turningRate * Time.deltaTime);
        }

        //Freeze for body
        if (this.tag == "Body parts")
        {
            float dist = Vector3.Distance(transform.position, targetPos);
            Debug.Log(dist);

            //If distance from target pos is small enough to be considered the same
            if(dist<0.1f)
            {
                rigidbody.isKinematic = true;
                Destroy(this);
            }  
            else
            {
                rigidbody.isKinematic = false;
            }
        }
    }
   
}
