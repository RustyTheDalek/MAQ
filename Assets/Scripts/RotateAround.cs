using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        RaycastHit hit;
        Vector3 mid = this.transform.FindChild("ground").transform.position;
        if(Physics.Raycast(mid, Vector3.down, out hit))
        {
            if(hit.transform.tag == "Ground" && hit.distance <2)
            {
//            Debug.Log("Collided");
                Debug.DrawLine (mid, hit.point, Color.cyan);
                transform.rotation = Quaternion.Lerp(
                                        transform.rotation,
                    Quaternion.FromToRotation(Vector3.up, hit.normal), 0.15f);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.identity, 0.15f);
            }
        }
	}
}
