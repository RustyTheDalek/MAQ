using UnityEngine;
using System.Collections;

public class Taze : MonoBehaviour {

    float timer = 0, MAXTazeLength = 0.1f;

    int force = 0, minForce = 20000;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {      
            force = Random.Range(minForce, minForce*2);

            if(Random.value > 0.5f)
            {
                force *= -1;
            }

            GetComponent<LegJoint>().setWiggleDir();

            if(GetComponent<Rotating>())
            {
                Debug.Log(GetComponent<LegJoint>().wiggleDir);
            }

            rigidbody.AddTorque(GetComponent<LegJoint>().wiggleDir * force * Time.deltaTime, ForceMode.Impulse);

            timer = Random.Range(0f, MAXTazeLength);
        }
	}
}
