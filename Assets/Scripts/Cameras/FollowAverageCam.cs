using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowAverageCam : MonoBehaviour
{

    //The target for this camera will be an average from multiple animals
    Vector3 avgTarget;

    //Distance from the target
    float distance;

    //Minimum distance camera can be from target
    const float minDistancs = 8;

    //Angle to view at target
    public Vector3 angle;

    //List of targets for Camera to follow
    public List<GameObject> targets;

    //Variables used to calculate averages
    float totX, totY, totZ, furthestTarget;

    //Temp variable to tranlsate Camera
    Vector3 newPos;

	// Use this for initialization
	void Start () {
		this.camera.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {

        if(targets.Count > 0)
        {
            //Reset variables for average
            totX = 0;
            totY = 0;
            totZ = 0;
            furthestTarget = 0;

            foreach(GameObject target in targets)
            {
                //Mean calculation
                totX += target.transform.position.x;
                totY += target.transform.position.y;
                totZ += target.transform.position.z;

                //Finding furthest target
                if(Vector3.Magnitude(target.transform.position - avgTarget) > furthestTarget)
                {
                    furthestTarget = Vector3.Magnitude(target.transform.position - avgTarget);
                }
            }

            //Finish mean calculation
            avgTarget.x = totX / targets.Count;
            avgTarget.y = totY / targets.Count;
            avgTarget.z = totZ / targets.Count;

            //Look at new average target
            transform.LookAt(avgTarget);

            //Rotate using angle
            transform.localRotation = Quaternion.Euler(angle);

            //Apply small amplication to ensure all targets are in view
            distance = furthestTarget * 1.5f;

            //Clamp distance within reasonable range
            distance = Mathf.Clamp(distance, minDistancs, 50);

            //Transform camera smoothtly to keep follow the target at a given distance and roation
            Vector3 pos = transform.rotation * new Vector3(0,0,-distance) + avgTarget;
            transform.position = Vector3.Lerp(transform.position, pos, .06f);
        }
	}
}
