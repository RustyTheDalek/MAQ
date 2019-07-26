using UnityEngine;
using System.Collections;

public class PlayerObject : MonoBehaviour 
{
    public int playerID;

    public Color playerColour
    {
        get
        {
            switch (playerID)
            {
                case 1:

                    return TwoGradients.firstGMinCol;

                case 2:

                    return TwoGradients.firstGMaxCol;

                case 3:

                    return TwoGradients.secondGMinCol;

                case 4:

                    return TwoGradients.secondGMaxCol;

                default:

                    return Color.gray;
            }
        }
    }

    public string playerName
    {
        get
        {
            switch (playerID)
            {
                case 1:

                    return "Peaches";

                case 2:

                    return "Bananarama";

                case 3:

                    return "Granny Smith";

                case 4:

                    return "Deep Blue";

                default:

                    return "Gray Storm";
            }
        }
    }

    public static string getPlayerName(int id)
    {
        switch (id)
        {
            case 1:

                return "Peaches";

            case 2:

                return "Bananarama";

            case 3:

                return "Granny Smith";

            case 4:

                return "Deep Blue";

            default:

                return "Gray Storm";
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
