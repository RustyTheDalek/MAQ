using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour {

    public List<GameObject> cameras;

	public CanvasGroup twoPlayers, threePlayers, fourPlayers;
    int totalPlayers = 0;

    //Template camera to use for each Game.current.players cam
    public GameObject camTemplate;

    public bool start = false;

    public List<GameObject> testAnims;

	// Use this for initialization
	void Start () {

        //Load camera
        camTemplate = Resources.Load("Prefabs/PlayerCam") as GameObject;

        if (start)
        {
            Game.Current.players.AddRange(testAnims);

            camSetup();
        }
	}

    void OnLevelWasLoaded(int level)
    {
        start = false;
    }

	// Update is called once per frame
	void Update () {
	
	}

    public void camSetup()
    {
        cameras = new List<GameObject>();

        if (!camTemplate)
        {
            camTemplate = Resources.Load("Prefabs/PlayerCam") as GameObject;
        }

        //Debug.Log(Game.Current.players.Count);

        switch (Game.Current.players.Count)
        {
            case 1:

                //1 = fullscreen
                cameras.Add(Instantiate(camTemplate) as GameObject);
                cameras[cameras.Count-1].GetComponent<TrackCam>().setUp(Game.Current.players[0],
                    new Rect(0, 0, 1, 1));

                break;

            case 2:

                //2 = top and bottom
                cameras.Add(Instantiate(camTemplate) as GameObject);
                cameras[cameras.Count - 1].GetComponent<TrackCam>().setUp(Game.Current.players[0],
                    new Rect(0, 0.5f, 1, 0.5f));

                cameras.Add(Instantiate(camTemplate) as GameObject);
                cameras[cameras.Count - 1].GetComponent<TrackCam>().setUp(Game.Current.players[1],
                    new Rect(0, 0, 1, 0.5f));

				twoPlayers.alpha = (1f);

                break;


            case 3:

                //3 = TopL, TopR and Bottom Middle
                cameras.Add(Instantiate(camTemplate) as GameObject);
                cameras[cameras.Count - 1].GetComponent<TrackCam>().setUp(Game.Current.players[0],
                    new Rect(0, 0.5f, 0.5f, 0.5f));

                cameras.Add(Instantiate(camTemplate) as GameObject);
                cameras[cameras.Count - 1].GetComponent<TrackCam>().setUp(Game.Current.players[1],
                    new Rect(0.5f, 0.5f, 0.5f, 0.5f));

                cameras.Add(Instantiate(camTemplate) as GameObject);
                cameras[cameras.Count - 1].GetComponent<TrackCam>().setUp(Game.Current.players[2],
                    new Rect(0, 0, 0.5f, 0.5f));

				threePlayers.alpha = (1f);

                break;

            case 4:

                //4 = TopL, TopR, BottomL and BottomR
                cameras.Add(Instantiate(camTemplate) as GameObject);
                cameras[cameras.Count - 1].GetComponent<TrackCam>().setUp(Game.Current.players[0],
                    new Rect(0, 0.5f, 0.5f, 0.5f));

                cameras.Add(Instantiate(camTemplate) as GameObject);
                cameras[cameras.Count - 1].GetComponent<TrackCam>().setUp(Game.Current.players[1],
                    new Rect(0.5f, 0.5f, 0.5f, 0.5f));

                cameras.Add(Instantiate(camTemplate) as GameObject);
                cameras[cameras.Count - 1].GetComponent<TrackCam>().setUp(Game.Current.players[2],
                    new Rect(0, 0, 0.5f, 0.5f));

                cameras.Add(Instantiate(camTemplate) as GameObject);
                cameras[cameras.Count - 1].GetComponent<TrackCam>().setUp(Game.Current.players[3],
                    new Rect(0.5f, 0, 0.5f, 0.5f));

				fourPlayers.alpha = (1f);

                break;
        }
    }
    
}
