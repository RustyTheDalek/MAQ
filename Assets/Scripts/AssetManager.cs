using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public static class AssetManager{

    static Object[] objs;

    static List<Material>_TerrainMaterials;

    public static List<Material> terrainMaterials
    {
        get
        {
            if (_TerrainMaterials == null)
            {
                _TerrainMaterials = new List<Material>();

                objs = Resources.LoadAll("Materials/terrains");

                foreach (object obj in objs)
                {
                    _TerrainMaterials.Add((Material)obj);
                }
            }

            return _TerrainMaterials;
        }
    }

    static List<Material> _ObjectMaterials;

    public static List<Material> ObjectMaterials
    {
        get
        {
            if (_ObjectMaterials == null)
            {
                _ObjectMaterials = new List<Material>();

                objs = Resources.LoadAll("Materials/Objects");

                foreach (object obj in objs)
                {
                    _ObjectMaterials.Add((Material)obj);
                }
            }

            return _ObjectMaterials;
        }
    }

    static List<Material> _PlayerMaterials;

    //Players materials for colouring Models
    public static List<Material> playerMaterials
    {
        get
        {
            if (_PlayerMaterials == null)
            {
                _PlayerMaterials = new List<Material>();

                objs = Resources.LoadAll("Materials/Players");

                foreach (object obj in objs)
                {
                    _PlayerMaterials.Add((Material)obj);
                }
            }

            return _PlayerMaterials;
        }
    }

    //lists for the parts that make up animals
    public static List<GameObject> _AnimBodies, _AnimLimbs;

    public static List<GameObject> animBodies
    {
        get
        {
            if (_AnimBodies == null)
            {
                _AnimBodies = new List<GameObject>();

                objs = Resources.LoadAll("BodyParts/CoreBodyTypes");

                foreach (object obj in objs)
                {
                    _AnimBodies.Add((GameObject)obj);
                    _AnimBodies[animBodies.Count - 1].CreatePool(20);
                }
            }

            return _AnimBodies;
        }
    }

    public static List<GameObject> animLimbs
    {
        get
        {
            if (_AnimLimbs == null)
            {
                _AnimLimbs = new List<GameObject>();

                objs = Resources.LoadAll("BodyParts/Limbs");

                foreach (object obj in objs)
                {
                    _AnimLimbs.Add((GameObject)obj);
                    _AnimLimbs[animLimbs.Count - 1].CreatePool(100);
                }
            }

            return _AnimLimbs;
        }
    }
}
