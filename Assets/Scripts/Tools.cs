using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

public static class Tools 
{
    /*his conversion is used when 0 and 1 represent bool in a string instead of
        true/false*/
    public static bool stringToBool(string boolString)
    {
        if(boolString == "1")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static float lowCheck(float val, float lowVal)
    {
        if(val < lowVal && val != 0)
        {
            return val;
        }
        else
        {
            return lowVal;
        }
    }

    public static float highCheck(float val, float highVal)
    {
        if(val > highVal && val != 0)
        {
            return val;
        }
        else
        {
            return highVal;
        }

    }

    public static float RangeConvert(float oldValue, float oldMin, float oldMax, float newMin,  float newMax)
    {
        float oldRange = (oldMax - oldMin);
        float newRange = (newMax - newMin);
        
        return ((((oldValue - oldMin) * newRange) / oldRange) + newMin);
    }

    public static float handleExpData(float expVal, float lowVal, float highVal, 
                                      float average, float newLowVal, float newHighVal, int index)
    {
        if(SaveLoad.localAvgAvailable())
        {
            if(expVal!=0)
            {
                return Tools.RangeConvert(expVal, lowVal, highVal, newLowVal, newHighVal);
            }
            else
            {
                float tempVal = Tools.CreateVal(average, highVal, lowVal, index, 
                                                SaveLoad.LocalExoData.Count);
                
                return Tools.RangeConvert(tempVal, lowVal, highVal, newLowVal, newHighVal);
            }
        }
        else
        {
            Debug.LogError("Local averages not available");
            return -1;
        }
    }

    //Creates values using an average, high and low value and it's index in the array.
    //TODO:Improve creation of valu
    public static float CreateVal(float average, float high, float low, int index, int size)
    {
        //Debug.Log(index);

        float percentage = (float)((float)index/(float)size);

        percentage = Tools.RangeConvert(percentage, 0, 1, low, high);

        return percentage;
    }

    public static Vector3 setVector3(Vector3 original, string component, float val)
    {
        component = component.ToLower();
        switch(component)
        {
            case "x":
                return new Vector3(val, original.y, original.z);

            case "y":
                return new Vector3(original.x, val, original.z); 

            case "z":
                return new Vector3(original.x, original.y, val);

            default:
                Debug.LogWarning(original + " is incorrect, use x, y or z instead");
                return original;
        }
    }

    public static float getSmallestDiff(float val, float curenntLowVal, float newLowVal)
    {
        if(Mathf.Abs(val - curenntLowVal) <= Mathf.Abs(val - newLowVal) && curenntLowVal != 0)
        {
            return curenntLowVal;
        }
        else
        {
            return newLowVal;
        }
    }

    public static bool nearLimits(HingeJoint hingeJoint, float angle)
    {
        if(Mathf.Abs(hingeJoint.angle - hingeJoint.limits.max) < angle || 
           Mathf.Abs(hingeJoint.angle - hingeJoint.limits.min) < angle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool withinRng(float val, float low, float high)
    {
        if(val > low && val < high)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static IEnumerator lerpTextAlpha(UnityEngine.UI.Text text, float alphaTarget, float speed)
    {
        while(Mathf.Abs(text.color.a - alphaTarget) > 0.01)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(text.color.a, alphaTarget, speed));
            yield return null;
        }

        text.color = new Color(text.color.r, text.color.g, text.color.b, alphaTarget);
    }

    public static Vector3 push(RaycastHit hit, GameObject obj)
    {
//        string largestAxis = largestAxis(Vector3);

        switch(largestAxis(hit.normal))
        {
            case "x":

                if(hit.normal.x > 0)
                {
                    return hit.normal* Mathf.Abs(
                        obj.GetComponent<MeshFilter>().sharedMesh.bounds.min.x);
                }
                else
                {
                    return hit.normal* Mathf.Abs(
                        obj.GetComponent<MeshFilter>().sharedMesh.bounds.max.x);
                }
            case "y":

                if(hit.normal.y > 0)
                {
                    return hit.normal* Mathf.Abs(
                        obj.GetComponent<MeshFilter>().sharedMesh.bounds.min.y);
                }
                else
                {
                    return hit.normal* Mathf.Abs(
                        obj.GetComponent<MeshFilter>().sharedMesh.bounds.max.y);
                }

            case "z":

                if(hit.normal.z > 0)
                {
                    return hit.normal* Mathf.Abs(
                        obj.GetComponent<MeshFilter>().sharedMesh.bounds.min.z);
                }
                else
                {
                    return hit.normal* Mathf.Abs(
                        obj.GetComponent<MeshFilter>().sharedMesh.bounds.max.z);
                }
        }

        Debug.LogWarning("No larger normal found");
        
        return Vector3.zero;
    }
    
    public static string largestAxis(Vector3 v)
    {
        if(Mathf.Abs(v.x) > Mathf.Abs(v.y) && Mathf.Abs(v.x) > Mathf.Abs(v.z))
        {
//            Debug.Log("X largest");
            return "x";
        }

        if(Mathf.Abs(v.y) > Mathf.Abs(v.x) && Mathf.Abs(v.y) > Mathf.Abs(v.z))
        {
//            Debug.Log("Y largest");
            return "y";
        }

        if(Mathf.Abs(v.z) > Mathf.Abs(v.y) && Mathf.Abs(v.z) > Mathf.Abs(v.x))
        {
//            Debug.Log("Z largest");
            return "z";
        }

//        Debug.LogWarning("No larger axis found");
        
        return "error";
    }

    public static Vector3 largestNormal(Vector3 v)
    {
        if(Mathf.Abs(v.x) > Mathf.Abs(v.y) && Mathf.Abs(v.x) > Mathf.Abs(v.z))
        {
//            Debug.Log("X largest");
            return Vector3.right;
        }
        
        if(Mathf.Abs(v.y) > Mathf.Abs(v.x) && Mathf.Abs(v.y) > Mathf.Abs(v.z))
        {
//            Debug.Log("Y largest");
            return Vector3.up;
        }
        
        if(Mathf.Abs(v.z) > Mathf.Abs(v.y) && Mathf.Abs(v.z) > Mathf.Abs(v.x))
        {
//            Debug.Log("Z largest");
            return Vector3.forward;
        }
        
//        Debug.LogWarning("No larger axis found");
        
        return Vector3.zero;
    }

    public static void setAlpha(GameObject part, float alpha)
    {
        part.renderer.material.color = new Color(part.renderer.material.color.r, 
                                                 part.renderer.material.color.g, 
                                                 part.renderer.material.color.b, 
                                                 alpha);
        foreach(Transform child in part.transform)
        {
            setAlpha(child.gameObject, alpha);
        }
    }

    public static void setAllChildren(GameObject obj, Action<GameObject> prop)
    {
        prop(obj);
        
        foreach(Transform child in obj.transform)
        {
            setAllChildren(child.gameObject, prop);
        }
    }

    public static void setAllChildren<T>(Action<GameObject, T> prop, GameObject obj, T val)
    {
        prop(obj, val);

        foreach(Transform child in obj.transform)
        {
            setAllChildren(prop, child.gameObject, val);
        }
    }

    public static void setAllChildren<T>(Action<GameObject, T> prop, GameObject obj, T val, string tagMask)
    {
        if (obj.tag != tagMask)
        {
            //Debug.Log(obj.name + " " + obj.tag);
            prop(obj, val);
        }

        foreach (Transform child in obj.transform)
        {
            setAllChildren(prop, child.gameObject, val, tagMask);
        }
    }

    public static void setBreakForces(GameObject obj, bool toInf)
    {
        if(obj.GetComponent<LegJoint>())
        {
            if(toInf)
            {
                obj.GetComponent<LegJoint>().breakForce = obj.hingeJoint.breakForce;
                obj.GetComponent<LegJoint>().breakTorque = obj.hingeJoint.breakTorque;
                
                obj.hingeJoint.breakForce = Mathf.Infinity;
                obj.hingeJoint.breakTorque = Mathf.Infinity;
    //            obj.rigidbody.drag = 3;
    //            obj.rigidbody.angularDrag = 3;
            }
            else
            {
                obj.hingeJoint.breakForce = obj.GetComponent<LegJoint>().breakForce;
                obj.hingeJoint.breakTorque = obj.GetComponent<LegJoint>().breakTorque;
                
                obj.rigidbody.drag = 0.1f;
                obj.rigidbody.angularDrag = 0.1f;
           }
        }

        foreach(Transform child in obj.transform)
        {
            setBreakForces(child.gameObject, toInf);
        }
    }

//    public static void setRotation( GameObject obj, Vector3 val)
//    {
//        if(obj.GetComponent<LegJoint>())
//        {
//            obj.transform.localEulerAngles = val;
//        }
//
//        foreach(Transform child in obj.transform)
//        {
//            setRotation( child.gameObject, val);
//        }
//    }

    public static void setRotation(GameObject obj, GameObject parent)
    {
        if(obj.GetComponent<LegJoint>())
        {
//            Debug.Log(obj.name);
            obj.transform.localEulerAngles = parent.transform.eulerAngles + obj.GetComponent<LegJoint>().restAngle;
        }
        
        foreach(Transform child in obj.transform)
        {
            setRotation(child.gameObject, obj);
        }
    }

    public static void removeHinges( GameObject obj)
    {
        if(obj.hingeJoint)
        {
            //Debug.Log(obj.name);

            if (obj.GetComponent<LegJoint>())
            {
                obj.GetComponent<LegJoint>().spring = obj.hingeJoint.spring;
                obj.GetComponent<LegJoint>().limits = obj.hingeJoint.limits;
                obj.GetComponent<LegJoint>().breakForce = obj.hingeJoint.breakForce;
                obj.GetComponent<LegJoint>().breakTorque = obj.hingeJoint.breakTorque;
            }

            if(obj.GetComponent<Rotating>())
            {
                obj.GetComponent<Rotating>().axis = obj.hingeJoint.axis;
            }

            GameObject.Destroy(obj.hingeJoint);
        }
        else if(obj.GetComponent<ConfigurableJoint>())
        {
            GameObject.Destroy(obj.GetComponent<ConfigurableJoint>());
        }


        foreach(Transform child in obj.transform)
        {
            removeHinges(child.gameObject);
        }
    }

    public static void addHinges(GameObject obj)
    {
        if(obj.GetComponent<LegJoint>())
        {
            obj.AddComponent<HingeJoint>();

            obj.hingeJoint.spring = obj.GetComponent<LegJoint>().spring;
            obj.hingeJoint.limits = obj.GetComponent<LegJoint>().limits;

            if(obj.GetComponent<Rotating>())
            {
                //Set hingeJoints axis up correctly
                obj.hingeJoint.axis = obj.GetComponent<Rotating>().axis;
            }
            else
            {
                obj.hingeJoint.useLimits = true;
            }
//            obj.hingeJoint.breakForce = 50;
//            obj.hingeJoint.breakTorque = 25;
            obj.hingeJoint.connectedBody = obj.transform.parent.rigidbody;
        }
        else if(obj.name == "Pupil")
        {
            obj.AddComponent<ConfigurableJoint>();

            ConfigurableJoint cJoint = obj.GetComponent<ConfigurableJoint>();

            cJoint.xMotion = ConfigurableJointMotion.Limited;
            cJoint.yMotion = ConfigurableJointMotion.Locked;
            cJoint.zMotion = ConfigurableJointMotion.Limited;

            cJoint.angularXMotion = ConfigurableJointMotion.Limited;
            cJoint.angularYMotion = ConfigurableJointMotion.Limited;
            cJoint.angularZMotion = ConfigurableJointMotion.Limited;

            SoftJointLimit limit = new SoftJointLimit();

            limit.limit = Eyes.eyeLimit(obj.transform.root.localScale.x);
            limit.bounciness = 0.5f;

            cJoint.linearLimit = limit;

            limit.limit = -30;
            limit.bounciness = 0.1f;
            limit.damper = 5;

            cJoint.lowAngularXLimit = limit;

            limit.limit *= -1;

            cJoint.highAngularXLimit = limit;

            cJoint.angularYLimit = limit;

            cJoint.angularZLimit = limit;

            JointDrive drive = new JointDrive();

            drive.mode = JointDriveMode.Position;
            drive.positionSpring = 15;
            drive.maximumForce = 3.402823e+38f;

            cJoint.angularXDrive = drive;
            cJoint.angularYZDrive = drive;

            cJoint.enableCollision = true;

            obj.GetComponent<ConfigurableJoint>().connectedBody = obj.transform.parent.rigidbody;
        }
        else if (obj.name == "Left" || obj.name == "Right")
        {
            obj.AddComponent<HingeJoint>();
            obj.hingeJoint.connectedBody = obj.transform.root.rigidbody;
            obj.hingeJoint.useLimits = true;
        }
        
        foreach(Transform child in obj.transform)
        {
            addHinges(child.gameObject);
        }
    }

    public static IEnumerator reAddHinges(GameObject part, RigidbodyConstraints rConst,
        bool gravityNow)
    {
        yield return new WaitForFixedUpdate();
        
        //Re-add Hinges
        Tools.addHinges(part);
        
//        Tools.setRotation(part);
        //Auto config Anchors

        //Unfreeze Rigidbodies - Disabled as re-configuring Hinges Broken 
        //Tools.setAllChildren(Actions.setKinematic, part, false);

        if (gravityNow)
        {
            Tools.setAllChildren(Actions.setGravity, part, true);
        }

		Tools.setAllChildren(Actions.setKinematic, part, false);

        System.Action<GameObject> reAddMotor =
            (obj) =>
        {
            if(obj.GetComponent<Rotating>())
            {
                JointMotor mot = new JointMotor();

                mot.targetVelocity = 750;
                mot.force = 100;
                obj.hingeJoint.motor = mot;
                obj.hingeJoint.useMotor = false;
            }
        };

        Tools.setAllChildren(part, reAddMotor);

        Tools.setAllChildren(Actions.setAutoConfigAnch, part, true);

        if (part.GetComponentInChildren<Eyes>())
        {
            part.GetComponentInChildren<Eyes>().scaleEyes();
        }

        part.rigidbody.AddForce(Vector3.down);

        part.rigidbody.constraints = rConst;
    }

    public static void setHinge( GameObject obj, HingeJoint hinge)
    {
        JointSpring temp = new JointSpring();
        temp = hinge.spring;

        obj.GetComponent<HingeJoint>().connectedBody = hinge.connectedBody;

        obj.hingeJoint.spring = temp;
        obj.hingeJoint.useSpring = true;

        obj.hingeJoint.limits = hinge.limits;
        obj.hingeJoint.useLimits = hinge.useLimits;
    }

    public static GameObject findbyName(List<GameObject> list, string name)
    {
        foreach(GameObject part in list)
        {
            if(part.name == name)
            {
                return part;
            }
        }
        
        Debug.LogWarning("No Part found by " + name);
        return new GameObject();
    }

    public static void findTotal(GameObject obj, ref int total)
    {
        total++;

        foreach(Transform child in obj.transform)
        {
            findTotal(child.gameObject, ref total);
        }
    }

    public static Vector3 allAsOne(float val)
    {
        return new Vector3(val, val, val);
    }

    public static bool objectValid(GameObject obj)
    {
        if(obj && obj.activeSelf)
            return true;
        else
            return false;
    }

    /*Splits text up by adding spaces each time a capital letter is found 
    (Exlucding first character)*/
    public static string textSplit(string text)
    {
        //Debug.Log(text.Length);

        for (int i = 1; i < text.Length; i++)
        {
            if (Char.IsUpper(text[i]))
            {
                text = text.Insert(i, " ");
                i++;
            }
        }

        return text;
    }

    internal static bool intersectsChildren(GameObject parent, GameObject limb, out Vector3 position)
    {
        foreach (Transform child in parent.transform)
        {
            if(child.tag == "Body parts" && 
                child.GetComponent<Collider>() && 
                limb.collider.bounds.Intersects(child.collider.bounds))
            {
                position = child.transform.localPosition;
                return true;
            }
        }

        position = Vector3.zero;

        return false;
    }
}
