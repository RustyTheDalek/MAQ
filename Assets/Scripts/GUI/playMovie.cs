﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playMovie : MonoBehaviour {

	// Use this for initialization
	void Start () {

		MovieTexture movie = GetComponent<RawImage>().texture as MovieTexture;
		movie.loop = true;
		movie.Play ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
