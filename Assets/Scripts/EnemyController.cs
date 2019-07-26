using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour 
{
    enum enemyState
    {
        patrolling,
        chasing,
        returning,

    };

    enemyState thisEnemyState;

	// Use this for initialization
	void Start () 
    {
        thisEnemyState = enemyState.patrolling;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        switch(thisEnemyState)
        {
            case enemyState.patrolling:

                RaycastHit hit;
                Vector3 enemyBottom = this.transform.FindChild("EnemyBottom").transform.position;

                if(Physics.Raycast(enemyBottom, Vector3.down, out hit))
                {
                    Debug.DrawLine (enemyBottom, hit.point, Color.cyan);
                    //Debug.Log(hit.distance);
                    //rigidbody.AddForce(new Vector3(-1, 0, 0));
                }

                //rigidbody.AddForce(new Vector3(1, 0, 0));

                break;
            case enemyState.chasing:
                break;
            case enemyState.returning:
                break;
        }
	}
}
