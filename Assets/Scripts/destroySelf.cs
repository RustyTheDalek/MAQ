using UnityEngine;
using System.Collections;

public class destroySelf : MonoBehaviour {
    
    // Use this for initialization
    void Start ()
    {
    }
    
    // Update is called once per frame
    void Update () {
        if (this.gameObject.transform.position.y <= -5)
        {
            Destroy(this.gameObject);
        }
    }
}