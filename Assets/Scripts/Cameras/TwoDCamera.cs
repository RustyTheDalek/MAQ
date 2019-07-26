using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TwoDCamera : MonoBehaviour 
{
    public Vector3 target;
    public Vector3 averageTarget = Vector3.zero;
    public List<GameObject> playerList = new List<GameObject>();

//    public GameObject sun;

    public int xOffset;
    public int yOffset;
    public int zOffset;
    public float maxZoom;
    public float zoomDistance;
    public float prevZoomDistance;
    public float deltaZoom;
    public float furthestAnimal;

    float distanceToGround;

    public bool follow;

	// Use this for initialization
	void Start () 
    {
        //if(target)
        {
//            transform.position = target;
        }
	}

    //Get the average position of all the objects in the list 
    //Sets the target as average position
    void AverageTarget ()
    {
        float totalX = 0.0f, totalY = 0.0f, totalZ = 0.0f;

        for(int i=0;i<playerList.Count;i++)
        {
            totalX += playerList[i].transform.position.x;
            totalY += playerList[i].transform.position.y;
            totalZ += playerList[i].transform.position.z;
        }

        averageTarget.x = totalX / playerList.Count;
        averageTarget.y = totalY / playerList.Count;
        averageTarget.z = totalZ / playerList.Count;
        target = averageTarget;
    }

    void RaceZoom()
    {
//        zoomDistance = 0;
        furthestAnimal = 0;
        foreach (GameObject creature in playerList)
        {
            if(Vector3.SqrMagnitude(creature.transform.position - averageTarget) > furthestAnimal)
            {
                furthestAnimal = Vector3.SqrMagnitude(creature.transform.position - averageTarget);
            }
//            Debug.Log(Vector3.SqrMagnitude(creature.transform.position - averageTarget));
//            if (Vector3.SqrMagnitude(creature.transform.position - averageTarget) > 100)
//            {
//                //Increase distance
//                zoomDistance +=1;
//            }
//            else if (zoomDistance> 0)
//            {
//                //Decrease distance
//                zoomDistance -= 1;
//            }
        }
        prevZoomDistance = zoomDistance;
        zoomDistance = furthestAnimal;

        deltaZoom = zoomDistance - prevZoomDistance;
    }

    Vector3 camPosRay()
    {
            Ray r;
            r = new Ray(transform.position,-target);
           
            return r.GetPoint(deltaZoom/4);
        Debug.Log(r.GetPoint(deltaZoom));
        
    }
	
	// Update is called once per frame
	void Update () 
    {
        if(follow)
        {
            if(playerList.Count > 0)
            {
                AverageTarget();
                RaceZoom();
                transform.LookAt(averageTarget);
            }
            camPosRay();
            if (zoomDistance < 1)
            {
                transform.position = Vector3.Lerp(transform.position, averageTarget + new Vector3(xOffset, yOffset, zOffset), 0.03f);
            }
            else
            { 
                 transform.position = Vector3.Lerp(transform.position,camPosRay(), 0.03f);
            }
        }
        //if(target)
//        {
//            RaycastHit hit;
//            if (Physics.Raycast(transform.position, -Vector3.up, out hit))
//            {
//                distanceToGround = hit.distance;
//
//              if(hit.point.y<target.y)
//                {
//                    transform.position = new Vector3(target.x + xOffset, (target.y + yOffset), target.z + zOffset);
//                }
//                else
//                {
//                    transform.position = new Vector3(target.x + xOffset,(hit.point.y + 5), target.z + zOffset);
//                }
//            }      
//    }

        if(Input.GetKeyDown(KeyCode.F))
        {
            GetComponentInChildren<Light>().enabled = 
                !GetComponentInChildren<Light>().enabled;
        }
	}
}
