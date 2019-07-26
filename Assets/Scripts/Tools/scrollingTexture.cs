using UnityEngine;
using System.Collections;

public class scrollingTexture : MonoBehaviour {

    float scrollSpeed = 1f;

    float offset;
    public Vector2 dir;
 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        offset+= (Time.deltaTime*scrollSpeed)/10.0f;
        renderer.material.SetTextureOffset ("_MainTex", dir * offset);
	
	}
}
