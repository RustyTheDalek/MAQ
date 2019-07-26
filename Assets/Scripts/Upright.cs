using UnityEngine;
using System.Collections;

public class Upright : MonoBehaviour {

    float uprightThreshold = 3f, upsideDownThreshold = .1f;

    public bool upright
    {
        get
        {
            return Vector3.SqrMagnitude(transform.up - Vector3.down) > uprightThreshold;
        }
    }

    public bool upsideDown
    {
        get
        {
            return Vector3.SqrMagnitude(transform.up - Vector3.down) < upsideDownThreshold;
        }
    }

    public bool sideways
    {
        get
        {
            return (Vector3.SqrMagnitude(transform.up - Vector3.down) > upsideDownThreshold) && (Vector3.SqrMagnitude(transform.up - Vector3.down) < uprightThreshold);
        }
    }
    //Integer relating to whether object is upside down or not
    public int uprightF
    {
        get
        {
            if (upright)
            {
                return 1;
            }
            else if (sideways)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }

    public float DownRatio
    {
        get
        {
            return Vector3.SqrMagnitude(transform.up - Vector3.down);
        }
    }
}