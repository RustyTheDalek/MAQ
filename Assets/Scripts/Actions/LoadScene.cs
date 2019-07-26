using UnityEngine;
using System.Collections;

public class LoadScene : Action {

    public string level;

    public override void doAction()
    {
        Application.LoadLevel(level);
    }
}
