using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SaveToggle : MonoBehaviour {

    public GameObject saveAnimalGUI;

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(onClick);
    }
    
    // Update is called once per frame
    void Update () {
    }
    
    void onClick()
    {
        saveAnimalGUI.SetActive(!saveAnimalGUI.activeSelf);
    }
}
