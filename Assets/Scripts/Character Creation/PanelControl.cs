using UnityEngine;
using System.Collections;

public class PanelControl : MonoBehaviour {

    public CanvasGroup bodyPanel, legPanel, otherPanel;

	// Use this for initialization
	void Start () {
	
        bodyPanel.alpha = (0f);
        bodyPanel.interactable = false;
        bodyPanel.blocksRaycasts = false;
        
        legPanel.alpha = (0f);
        legPanel.interactable = false;
        legPanel.blocksRaycasts = false;
        
        otherPanel.alpha = (0f);
        otherPanel.interactable = false;
        otherPanel.blocksRaycasts = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickBodyPanel()
    {
        bodyPanel.alpha = (1f);
        bodyPanel.interactable = true;
        bodyPanel.blocksRaycasts = true;

        legPanel.alpha = (0f);
        legPanel.interactable = false;
        legPanel.blocksRaycasts = false;
        
        otherPanel.alpha = (0f);
        otherPanel.interactable = false;
        otherPanel.blocksRaycasts = false;
    }

    public void OnClickLegPanel()
    {
        legPanel.alpha = (1f);
        legPanel.interactable = true;
        legPanel.blocksRaycasts = true;

        bodyPanel.alpha = (0f);
        bodyPanel.interactable = false;
        bodyPanel.blocksRaycasts = false;

        otherPanel.alpha = (0f);
        otherPanel.interactable = false;
        otherPanel.blocksRaycasts = false;
    }

    public void OnClickOtherPanel()
    {
        otherPanel.alpha = (1f);
        otherPanel.interactable = true;
        otherPanel.blocksRaycasts = true;

        bodyPanel.alpha = (0f);
        bodyPanel.interactable = false;
        bodyPanel.blocksRaycasts = false;

        legPanel.alpha = (0f);
        legPanel.interactable = false;
        legPanel.blocksRaycasts = false;
    }

}
