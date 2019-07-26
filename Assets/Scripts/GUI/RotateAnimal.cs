using UnityEngine;
using System.Collections;

public class RotateAnimal : MonoBehaviour {

    public Vector3 direction;

	// Use this for initialization
	void Start () {
        rotateAnimal();
	}

    void rotateAnimal()
    {
        this.transform.Rotate(direction);
    }
	
	// Update is called once per frame
	void Update () {
        rotateAnimal();
	}
}
