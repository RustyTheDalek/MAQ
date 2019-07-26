using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FlashingText : MonoBehaviour {

    //String component to flash
    Text text;

    //Color of text;
    Color col;

    //Whether the Text is going from Alpha 0 to 1 or the other way around
    bool up = true;

    //Speed at which text blinks
    [Range(0, 1f) ]
    public float speed = 0.5f;


	// Use this for initialization
	void Start () {

        text = GetComponent<Text>();
	
        col = text.color;
	}
	
	// Update is called once per frame
	void Update () {
	
        if(up)
        {
            if(text.color.a >= 0.999f)
            {
                up = false;
            }
            else
            {
                text.color = Color.Lerp(text.color, new Color(col.r, col.g, col.b, 1), speed);
            }
        }
        else
        {
            if(text.color.a <= 0.001f)
            {
                up = true;
            }
            else
            {
                text.color = Color.Lerp(text.color, new Color(col.r, col.g, col.b, 0), speed);
            }
        }
	}
}
