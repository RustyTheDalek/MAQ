using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour {

    public GameObject target;

    public float distance = 4, speed = 20;

	public float height = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.LookAt(target.transform.position);

        //Rotate using angle
        transform.localRotation = Quaternion.Euler(0,Time.time * speed ,0);

        //Mathf.Sin(Time.time) * Time.deltaTime * speed

        transform.position = transform.rotation * new Vector3(0, height, -distance)
            + target.transform.position;
	}
}
