using UnityEngine;
using System.Collections;

public static class ExtensionMethods
{
    public static CreatureController creatureController(this GameObject gO)
    {
        if (gO.GetComponent<CreatureController>())
        {
            return gO.GetComponent<CreatureController>();
        }
        else
        {
            Debug.LogWarning("No Creature controller attached");

            return null;
        }
    }
    
    public static Color setColAlpha(this Material material, float a)
    {
        return new Color(material.color.r, material.color.g, material.color.b, a);
    }

    public static Color setColAlpha(this Color col, float a)
    {
        return new Color(col.r, col.g, col.b, a);
    }
}
