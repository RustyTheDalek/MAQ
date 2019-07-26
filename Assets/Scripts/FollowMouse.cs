//Simple script that has object follow mousem, can be toggled on and off and 
//limits set
using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {

    public bool followingMouse;

    public int playerID;

    public float minX, maxX, minY, maxY, minZ, maxZ;

    public GameObject player;

    static float distance = 20f;

//  float speed = 20;

    public Vector3 wantedPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(followingMouse)
        {
            
//            transform.position += new Vector3(-player.transform.position.x, player.transform.position.y, 0) * Time.deltaTime * speed;

            wantedPos = Camera.main.ScreenToWorldPoint(new Vector3((player.transform.position.x), (player.transform.position.y), distance));

            transform.position = new Vector3(
                                          Mathf.Clamp(wantedPos.x, minX, maxX),
                                          Mathf.Clamp(wantedPos.y, minY, maxY),
                                          Mathf.Clamp(wantedPos.z, minZ, maxZ));
        }
    }
}
    