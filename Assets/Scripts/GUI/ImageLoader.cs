using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
        StartCoroutine(loadImage());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator loadImage()
    {
        string fileName = "file://C:/" + Application.persistentDataPath + "/Thumbnails/" + name + ".png"; 
        
        WWW www = new WWW(fileName);
        
        yield return www;
        
        GetComponentInChildren<Image>().sprite = Sprite.Create(www.texture, new Rect(0,0, www.texture.width, www.texture.height), Vector2.zero);
    }
}
