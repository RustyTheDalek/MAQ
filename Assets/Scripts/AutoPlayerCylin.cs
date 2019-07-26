using UnityEngine;
using System.Collections;

public class AutoPlayerCylin : MonoBehaviour
{

    playerPlaying playerCylinder;

    const float ANIMCHANGETIME = 30;

    float animChangeTimer = 0;


	// Use this for initialization
	void Start () {

        playerCylinder = GetComponent<playerPlaying>();
	}
	
	// Update is called once per frame
	void Update () {

        if (animChangeTimer > 0)
        {
            animChangeTimer -= Time.deltaTime;
        }
        else
        {
             playerCylinder.addPlayer(AssetManager.animLimbs[Random.Range(0, AssetManager.animLimbs.Count)], true);
             playerCylinder.player.AddComponent<AutoPlayer>();
             playerCylinder.setTesting(true);
             Tools.setAllChildren(Actions.setWiggle, playerCylinder.player, true);
             Tools.setAllChildren(Actions.canRace, playerCylinder.player, false);
             animChangeTimer = ANIMCHANGETIME;
        }
	}
}
