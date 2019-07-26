using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartAdvancedCreation : Action {

    public CharacterCreation charCreation;

    public Text errorMessage;

    public override void doAction()
    {
        if (GetComponentInParent<Button>().interactable && (Game.Current.planet != null || Application.isEditor))
        {
            charCreation.startAdvancedCreation();
            errorMessage.gameObject.SetActive(false);
        }
        else
        {
            errorMessage.gameObject.SetActive(true);
            errorMessage.text = "No Planet to test";
        }
    }
}
