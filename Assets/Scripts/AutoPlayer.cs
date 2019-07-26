using UnityEngine;
using System.Collections;

public class AutoPlayer : MonoBehaviour {

    Regular body;

    public Vector2 leftStick;

    float MAXREACTIONTIME = 1f;

    float reactionTimer = 0;
	// Use this for initialization
	void Start () {

        body = GetComponent<Regular>();

	}
	
	// Update is called once per frame
	void Update () {

        if (reactionTimer <= 0)
        {
            leftStick += new Vector2(Random.Range(-.25f, 25f), Random.Range(-.1f, .1f));
            leftStick = Vector2.ClampMagnitude(leftStick, 1);
            reactionTimer = Random.Range(0, MAXREACTIONTIME);
            body.turningLogic(leftStick);
        }
        else
        {
            reactionTimer -= Time.deltaTime;
        }

	}
}
