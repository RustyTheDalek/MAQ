using UnityEngine;
using System.Collections;

public class ScoringScript : MonoBehaviour 
{
    GameObject player; 

    Vector3 startPos;
    bool alive = true;
    float timeAlive = 0;

    public float maxDistance = 0.0f; // distance managed
    public float averageSpeed = 0.0f; //average fastness
    public float maxJumpHeight = 0.0f; //maximum upness
    public float timeSpentInAir = 0.0f; //airtime
    public float timeUpsideDown = 0.0f; //topsyturvytime

	// Use this for initialization
	void Start () 
    {
        //This is the start posiion of the player
        player = this.gameObject;
        startPos = player.transform.position;
	}

    public void reSet()
    {
        maxDistance = 0.0f;
        averageSpeed = 0.0f;
        maxJumpHeight = 0.0f;
        timeSpentInAir = 0.0f;
        timeUpsideDown = 0.0f;
    }

    //Calculates thee creatures average speed while alive
    void workoutSpeed()
    {
        averageSpeed = maxDistance / timeAlive;
    }

    //Calculates the maximum distance traveled from the starting point of the player
    void workoutDistance()
    {
        float currentDistance = player.transform.position.x - startPos.x;
        if (maxDistance < currentDistance )
        {
            maxDistance = currentDistance;
            //Rounding to 2DP
            maxDistance = Mathf.Round(currentDistance*100)/100;
        }
        //Debug.Log(maxDistance);
    }



    //Gets the lowest bodypart on the animal that is still attatched
    //1.Sets Current body part to lowest part
    //2.If child is lower will call itself and do the same thing for the child
    Vector3 getLowestPart(GameObject bodyPart)
    {
        Vector3 lowestPoint = bodyPart.transform.position;
        foreach(Transform child in bodyPart.transform)
        { 
            if (child.position.y < lowestPoint.y)
            {
                //Calls itself to find lowest child of child
                getLowestPart(child.gameObject);
            }
        }
        return lowestPoint;
    }

    //Calculate the maximum jump height of the player
    void workoutJump()
    {
        RaycastHit hit;
        Physics.Raycast(getLowestPart(player),-Vector3.up, out hit);


        if(hit.distance>maxJumpHeight)
        {
            maxJumpHeight = Mathf.Round(hit.distance*100)/100;
        }           
    }

	// Update is called once per frame
	void Update () 
    {
   
        if (alive)
        {
            workoutDistance();
            workoutJump();
        }
        else if (!alive)
        {
            workoutSpeed();
        }

        if (alive)
        {
            timeAlive += Time.deltaTime;
        }
        if (GetComponent<Upright>().upright != true)
        {
            timeUpsideDown  += Time.deltaTime;
            //Debug.Log(timeUpsideDown);
        }
		if (GetComponentInChildren<OnGround>() && !GetComponentInChildren<OnGround>().onGround)
        {           
			timeSpentInAir  += Time.deltaTime;
        }
    }   
}
