using UnityEngine;
using System.Collections;

public class OnGround : MonoBehaviour 
{
    public bool onGround;

    public bool trigger;
    // Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnCollisionEnter(Collision collider) 
    {
        if(!trigger)
        {
            if(collider.transform.tag == "Ground" || collider.gameObject.tag == "Organism")
            {
    //            Debug.Log("On Ground");
                onGround = true;
            }
        }
    }

    void OnCollisionStay(Collision collider) 
    {
        if (!trigger)
        {
            if (collider.transform.tag == "Ground" || collider.gameObject.tag == "Organism" && !onGround)
            {
                //            Debug.Log("Off Ground");
                onGround = true;
            }
        }
    }
    
    void OnCollisionExit(Collision collider) 
    {
        if (!trigger)
        {
            if (collider.transform.tag == "Ground" || collider.gameObject.tag == "Organism")
            {
                //            Debug.Log("Off Ground");
                onGround = false;
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (trigger)
        {
            if (collider.tag == "Ground" || collider.tag == "Organism")
            {
                //            Debug.Log("On Ground");
                onGround = true;
            }
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Ground" || collider.tag == "Organism" && !onGround)
        {
            //            Debug.Log("Off Ground");
            onGround = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Ground" || collider.tag == "Organism")
        {
            //            Debug.Log("Off Ground");
            onGround = false;
        }
    }
}
