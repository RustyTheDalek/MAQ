using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatBars : MonoBehaviour {

    public Image healthBar;
    public Image hungerBar;
    public CanvasGroup statbars;

    public PlayerAnimal pAnimal;

    void start() {

        statbars.blocksRaycasts = false;
        statbars.interactable = false;
           
    }
	// Update is called once per frame
	void Update () {
	
        healthBar.fillAmount = pAnimal.health / 100;
        hungerBar.fillAmount = pAnimal.hunger / 100;

	}
}
