using UnityEngine;
using System.Collections;

public class destroyScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(DestroyMe());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(5.0f);

        Destroy(this.gameObject);
    }
}
