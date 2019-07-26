using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using System;

public class AnimalLoader : MonoBehaviour {

    public CameraManager camManager;

    public Sun sun;

    Vector3 resetPos;

	// Use this for initialization
	void Start () {

        resetPos = transform.position;
        sun.posRef = gameObject;
	}

    void OnLevelWasLoaded(int level)
    {

        loadAnimals();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void reset()
    {
        transform.position = resetPos;

        loadAnimals();
    }

    void loadAnimals()
    {
        for (int i = 0; i < Game.Current.players.Count; i++)
        {
            Game.Current.players[i].SetActive(true);

            GameObject animal = Game.Current.players[i];

            if (animal)
            {
                animal.transform.position = transform.position - Vector3.right * animal.collider.bounds.size.z * 1 / animal.transform.localScale.x;
                transform.position += Vector3.forward * 6.25f;
                animal.transform.rotation = transform.rotation;
                animal.rigidbody.useGravity = false;
                Tools.setAllChildren(Actions.setWiggle, animal, false);
                Tools.setAllChildren(Actions.canRace, animal, false);
                Tools.setAllChildren(Actions.setKinematic, animal, true);
                DontDestroyOnLoad(animal);
            }

            //loadAnimal( Game.Current.players[i]);
        }

        camManager.camSetup();
    }

    void loadAnimal(GameObject animal)
    {
        if(animal)
        {
            transform.position += Vector3.forward * 5 * animal.transform.localScale.x; //6.25f
            animal.transform.position = transform.position - Vector3.right * animal.collider.bounds.size.z * 1 / animal.transform.localScale.x;
            animal.transform.rotation = transform.rotation;
            animal.rigidbody.useGravity = false;
            animal.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            Tools.setAllChildren(Actions.setWiggle, animal, false);
            DontDestroyOnLoad(animal);
        }
    }
}
