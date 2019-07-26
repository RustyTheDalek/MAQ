using UnityEngine;
using System.Collections;

public class Straighten : MonoBehaviour {

    public float strength, damper;
	// Use this for initialization
	void Start () {

        JointSpring temp = new JointSpring();

        temp.spring = strength;
        temp.damper = damper;
        hingeJoint.spring = temp;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
