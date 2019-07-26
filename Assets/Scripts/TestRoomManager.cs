using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestRoomManager : MonoBehaviour {

    public CameraManager camManager;

    public List<Transform> positions;

    public List<PlayerText> players;

    const float TESTENDTIME = 3;

    float timer = 0;

    int playersActive, totPlayersReady;

    Canvas playerReadyCanvas;

    bool playersReady
    {
        get
        {
            playersActive = 0;
            totPlayersReady = 0;

            if (Game.Current.players != null)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].gameObject.activeSelf)
                    {
                        playersActive++;

                        if (players[i].ready)
                        {
                            totPlayersReady++;
                        }
                    }
                }

                if (playersActive == totPlayersReady && playersActive != 0)
                {
                    //Debug.Log(playersActive + " " + totPlayersReady);
                    return true;
                }
                else
                {
                    //Debug.Log("Players active" + playersActive);
                    //Debug.Log("Total players ready" + totPlayersReady);
                }
            }
            return false;
        }
    }

	// Use this for initialization
	void Start () {

        for (int i = 0; i < players.Count; i++)
        {
            if (Game.Current.players != null && i < Game.Current.players.Count)
                players[i].gameObject.SetActive(true);
        }

        playerReadyCanvas = Resources.Load<Canvas>("Prefabs/GUI/PlayerReadyCanvas");
	}

    void OnLevelWasLoaded(int level)
    {
        playerReadyCanvas = Resources.Load<Canvas>("Prefabs/GUI/PlayerReadyCanvas");

        BodyPart.testing = true;

        loadAnimals();
    }
	
	// Update is called once per frame
	void Update () {

        if (playersReady)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                BodyPart.testing = false;

                foreach (GameObject player in Game.Current.players)
                {
                    if (player)
                        reset(player);
                }

                Application.LoadLevel("Terrain2");
            }
        }
        else
        {
            timer = TESTENDTIME;
        }
	}

    void loadAnimals()
    {

        for (int i = 0; i < Game.Current.players.Count; i++)
        {
            Game.Current.players[i].SetActive(true);
            loadAnimal(Game.Current.players[i], positions[i]);
        }

        camManager.camSetup();

        Canvas pReadyCanvas;

        if (camManager.cameras[0])
        {
            camManager.cameras[0].GetComponent<TrackCam>().angle = new Vector3(20, 210, 0);
            pReadyCanvas = Instantiate(playerReadyCanvas) as Canvas;
            pReadyCanvas.worldCamera = camManager.cameras[0].GetComponent<Camera>();
            pReadyCanvas.GetComponentInChildren<PlayerText>().playerID = 1;
            players.Add(pReadyCanvas.GetComponentInChildren<PlayerText>());
        }

        if (camManager.cameras.Count > 1 && camManager.cameras[1])
        {
            camManager.cameras[1].GetComponent<TrackCam>().angle = new Vector3(20, -30, 0);
            pReadyCanvas = Instantiate(playerReadyCanvas) as Canvas;
            pReadyCanvas.worldCamera = camManager.cameras[1].GetComponent<Camera>();
            pReadyCanvas.GetComponentInChildren<PlayerText>().playerID = 2;
            players.Add(pReadyCanvas.GetComponentInChildren<PlayerText>());
        }

        if (camManager.cameras.Count > 2 && camManager.cameras[2])
        {
            camManager.cameras[2].GetComponent<TrackCam>().angle = new Vector3(20, 150, 0);
            pReadyCanvas = Instantiate(playerReadyCanvas) as Canvas;
            pReadyCanvas.worldCamera = camManager.cameras[2].GetComponent<Camera>();
            pReadyCanvas.GetComponentInChildren<PlayerText>().playerID = 3;
            players.Add(pReadyCanvas.GetComponentInChildren<PlayerText>());
        }

        if (camManager.cameras.Count > 3 && camManager.cameras[3])
        {
            camManager.cameras[3].GetComponent<TrackCam>().angle = new Vector3(20, 30, 0);
            pReadyCanvas = Instantiate(playerReadyCanvas) as Canvas;
            pReadyCanvas.worldCamera = camManager.cameras[3].GetComponent<Camera>();
            pReadyCanvas.GetComponentInChildren<PlayerText>().playerID = 4;
            players.Add(pReadyCanvas.GetComponentInChildren<PlayerText>());
        }

        BodyPart.testing = true;
    }

    void loadAnimal(GameObject animal, Transform spawn)
    {
        if (animal)
        {
            //Debug.Log("here");
            animal.transform.position = spawn.position;
            animal.transform.rotation = spawn.rotation;

            //Unfreeze, enable gravity
            Tools.setAllChildren(Actions.setKinematic, animal, false);
            Tools.setAllChildren(Actions.setGravity, animal, true);
            DontDestroyOnLoad(animal);
        }
    }

    void reset(GameObject obj)
    {
        Tools.setAllChildren(Actions.setGravity, obj, false);
        Tools.setAllChildren(Actions.setKinematic, obj, true);
    }
}
