using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Animate : MonoBehaviour {

    public Animator animate;

	// Use this for initialization
	void Start () {

	}

    public void Gorunning () {
        animate.SetBool("Run", true);
    }

    public void GorunningBack () {
        animate.SetBool("Run", false);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
