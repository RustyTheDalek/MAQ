using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DistanceBar : MonoBehaviour {

	public Race race;

    public List<Image> raceImages;
	public List<Slider> distSliders;

	float startLinePos, endLinePos;

	// Use this for initialization
	void Start () {

        distSliders = new List<Slider>();

        Slider[] sliders = GetComponentsInChildren<Slider>(true);

        foreach (Slider slider in sliders)
        {
            distSliders.Add(slider);
        }

		startLinePos = race.lines [0].transform.position.x;
		endLinePos = race.lines [1].transform.position.x;

	}

    public void activate()
    {
        if (Game.Current.players.Count > 1)
        {
            for (int i = 0; i < Game.Current.players.Count; i++)
            {
                distSliders[i].gameObject.SetActive(true);
            }

            raceImages[0].gameObject.SetActive(true);
            raceImages[1].gameObject.SetActive(true);
            raceImages[2].gameObject.SetActive(true);

        }
        else
        {
            gameObject.SetActive(false);

            raceImages[0].gameObject.SetActive(false);
            raceImages[1].gameObject.SetActive(false);
            raceImages[2].gameObject.SetActive(false);

        }
    }
	
	// Update is called once per frame
	void Update () {

        if (Race.rState == Race.raceState.racing || Race.rState == Race.raceState.finishing)
        {
            for (int i = 0; i < Game.Current.players.Count; i++)
            {
                distSliders[i].value = Tools.RangeConvert(Game.Current.players[i].transform.position.x, startLinePos, endLinePos, 10, 110);

                distSliders[i].GetComponent<Canvas>().sortingOrder = (int)distSliders[i].value;
            }
        }
	}
}
