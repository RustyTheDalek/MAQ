using UnityEngine;
using System.Collections;

public class PlaceOnGround : MonoBehaviour {

	// Use this for initialization
	void Start () {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            this.gameObject.transform.position = hit.point+Vector3.up*10;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
