using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveDrone : MonoBehaviour {

    Controller gPad;

    //How far the drone is from the scene
    protected float depth = 4f;

    public static int activeMenu = 1;

    public LineRenderer lineRenderer;
    public Transform drone;
    public Transform handler;

    public Color col;

    public Vector3 offset;

    private CharacterController charController;

    public LevelSelect lvlSel;

    public Transform laserPos;
	// Use this for initialization
	void Start () {

        gPad = new Controller(0);

        charController = GetComponent<CharacterController>();

        col = TwoGradients.firstGMinCol/255 ;
                       
        lineRenderer.SetColors(col, col);

    	lineRenderer.SetWidth(.02f, 0.0001f);
	}
	
	// Update is called once per frame
	void Update () 
    {
        gPad.updateStates();
        
        if (gPad.isKeyDown(XKeyCode.A) || Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;

            if(Physics.Raycast(handler.position, Vector3.Normalize(handler.transform.position - Camera.main.transform.position), out hit))
            {
                Debug.DrawLine(handler.position, hit.point, Color.green, 10);

                if(hit.collider.tag == "Button")
                {
                    if(hit.collider.GetComponent<Action>())
                    {
                        hit.collider.GetComponent<Action>().doAction();
                    }
                }

                if (hit.collider.tag == "FindButton")
                {
                    //Debug.Log("Next Level");
                    lvlSel.NextLevel();
                }

                if (hit.collider.tag == "PlayButton")
                {
                    lvlSel.playPlanet();
                }

            }

        }

        if (activeMenu != 1)
        {
#if UNITY_EDITOR

            Vector3 movement = new Vector3();

            if (Input.GetKey(KeyCode.W))
            {
                movement.y++;
            }

            if (Input.GetKey(KeyCode.S))
            {
                movement.y--;
            }

            if (Input.GetKey(KeyCode.A))
            {
                movement.x++;
            }

            if (Input.GetKey(KeyCode.D))
            {
                movement.x--;
            }

            transform.position += movement * Time.deltaTime * 3;
#endif
            transform.position += new Vector3((-gPad.leftStick.x * Time.deltaTime * 3), (gPad.leftStick.y * Time.deltaTime * 3), 0);

            //GetComponentInChildren<Light>().transform.LookAt(transform.position);
        }

	}

    void LateUpdate ()
    {
        if (activeMenu != 1)
        {
            lineRenderer.SetPosition(0, laserPos.position);
            lineRenderer.SetPosition(1, handler.position);
        }
    }
}
