using UnityEngine;
using System.Collections;
using System;

public static class Actions
{
    //Set Collision layer
    public static Action<GameObject, int> setColLayer = 
        (obj, val) => 
    {
        obj.layer = val;
    };

    //Set Kinematic
    public static Action<GameObject, bool> setKinematic =
    (obj, val) =>
    {
        if (obj.GetComponent<Rigidbody>())
        {
            obj.rigidbody.isKinematic = val;
        }
    };

    //Disable anchors
    public static Action<GameObject, bool> setAutoConfigAnch =
    (obj, val) =>
    {
        if (obj.GetComponent<HingeJoint>())
        {
            obj.hingeJoint.autoConfigureConnectedAnchor = val;
        }
        else if (obj.GetComponent<ConfigurableJoint>())
        {
            obj.GetComponent<ConfigurableJoint>().
                autoConfigureConnectedAnchor = val;
        }
    };

    //Set Gravity
    public static Action<GameObject, bool> setGravity =
    (obj, val) =>
    {
        if (obj.rigidbody)
        {
            obj.rigidbody.useGravity = val;
        }
    };

    //Set onGround
    public static Action<GameObject, bool> setOnGround =
    (obj, val) =>
    {
        if (obj.GetComponent<OnGround>())
        {
            obj.GetComponent<OnGround>().onGround = val;
        }
    };

    //Set Angular drag
    public static Action<GameObject, int> setAngDrag =
    (obj, val) =>
    {
        if (obj.rigidbody)
        {
            obj.rigidbody.angularDrag = val;
        }
    };

    //Set Wiggle
    public static Action<GameObject, bool> setWiggle =
    (obj, val) =>
    {
        if (obj.hingeJoint && obj.GetComponent<LegJoint>())
        {
            obj.GetComponent<LegJoint>().canWiggle = val;
        }
    };

    //Set Player ID
    public static Action<GameObject, int> setID =
    (obj, val) =>
    {
        //Debug.Log(obj.name);
        if (obj.GetComponent<BodyPart>())
        {
            obj.GetComponent<BodyPart>().playerID = val;
        }
    };

    //Set canRace
    public static Action<GameObject, bool> canRace =
    (obj, val) =>
    {
        if (obj.GetComponent<BodyPart>())
        {
            obj.GetComponent<BodyPart>().canRace = val;
        }
    };

    //Add on ground
    public static Action<GameObject, BodyPart> findOnGround =
    (obj, parent) =>
    {
        if (obj.GetComponent<OnGround>())
        {
            parent.parts.Add(obj.GetComponent<OnGround>());
        }
    };

    //Set correct playerMaterials
    public static Action<GameObject, Material> setMaterial =
    (obj, mat) =>
    {
        if (obj.renderer)
        {
            //Debug.Log(obj.renderer.material.shader.name);

            if (obj.renderer.material.shader.name == "Custom/TGShader-Local")
            {
                if (obj.GetComponent<BodyPart>())
                {
                    int playerID = obj.GetComponent<BodyPart>().playerID;

                    if (obj.GetComponent<Rotating>())
                    {
                        obj.renderer.material = Resources.Load(
                            "Materials/Limb colours/Player" + playerID + "/red")
                            as Material;
                    }
                    else if (obj.GetComponent<Spring>())
                    {
                        obj.renderer.material = Resources.Load(
                            "Materials/Limb colours/Player" + playerID + "/green")
                            as Material;
                    }
                    else if (obj.GetComponent<Knee>())
                    {
                        obj.renderer.material = Resources.Load(
                            "Materials/Limb colours/Player" + playerID + "/blue")
                            as Material;
                    }
                    else
                    {
                        obj.renderer.material = mat;
                    }
                }
                else
                {
                    obj.renderer.material = mat;
                }
            }
        }
    };

    //Reycle
    public static Action<GameObject> recycle =
    (obj) =>
    {
        obj.Recycle();
    };
}
