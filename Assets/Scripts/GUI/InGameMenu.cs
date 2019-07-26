using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameMenu : MonoBehaviour {

    protected int options = 0;

    public Rect windowRect = new Rect(20, 20, 120, 50);
    public GUISkin SciFi_Skin;
    public GUIContent keyboardLayout;
    public CanvasGroup pause_Menu;

	// Use this for initialization
	void Start () {
	
        pause_Menu.alpha = (0f);
        pause_Menu.interactable = false;

	}
	
	// Update is called once per frame
	void Update () {


        //DEBUGGING pressing R reloads level, incase of problems and shit
        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel("Terrain2");
        }

        //if (Input.GetKeyDown(KeyCode.Escape) && options == 0)
        //{
        //    pause_Menu.alpha = (1f);
        //    pause_Menu.interactable = true;
            
        //    Screen.showCursor = true;

        //    options = 1;
        //    Time.timeScale = 0;
        //} 
        //else if (Input.GetKeyDown(KeyCode.Escape) && options == 1)
        //{
        //    pause_Menu.alpha = (0f);
        //    pause_Menu.interactable = false;
            
        //    Screen.showCursor = false;
        //    options = 0;
        //    Time.timeScale = 1;
        //}

    }

    public void OnClickBTS()
    {
        Application.LoadLevel("Main Menu");
    }

    public void OnClickBack()
    {
        pause_Menu.alpha = (0f);
        pause_Menu.interactable = false;
        
        options = 0;
        
        Screen.showCursor = false;

        Time.timeScale = 1;
    }

    void OnGUI(){
        
        GUI.skin = SciFi_Skin;

//        if (options == 1)
//        {
//            
//            GUI.Box(new Rect(Screen.width / 2, Screen.height / 2 - 140, 200, 300), "Paused");
//            GUI.Button(new Rect(Screen.width / 2 + 50, Screen.height / 2 - 100, 100, 20), "Audio");
//            GUI.Button(new Rect(Screen.width / 2 + 50, Screen.height / 2 - 60, 100, 20), "GamePlay");
//            if (GUI.Button(new Rect(Screen.width / 2 + 50, Screen.height / 2 - 20, 100, 20), "Controls"))
//            {
//                GUI.Window(0, windowRect, myWindow, "Controls");
//            }
//            
//            if (GUI.Button(new Rect(Screen.width / 2 + 50, Screen.height / 2 + 20, 100, 20), "Back"))
//            {
//                options = 0;
//                Time.timeScale = 1;
//            }
//            
//            Screen.showCursor = true;
//        }
        
    }
    
    private void myWindow(int id)
    {
        GUI.Label(new Rect(Screen.width / 6 + 50, Screen.height / 2 - 50, 200, 300), keyboardLayout);
    }

}
