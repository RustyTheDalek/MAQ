using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

    public float delayTime = 3f;
    public bool done = false;

    private float timer;

	// Use this for initialization
	void Start () {
        timer = delayTime;

        StartCoroutine("load");
	}
	
	// Update is called once per frame
	void Update () {
	
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        if (done && Input.anyKey)
        {
            Application.LoadLevel("Main Menu");
        }

	}

    IEnumerator load()
    {
        yield return new WaitForSeconds (0.01f);
        done = true;
    }

}
