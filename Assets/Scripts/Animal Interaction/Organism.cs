 //Organism Class
// Base clas for all living creatures that allow plants and Animals to be combined in a list

using UnityEngine;
using System.Collections;

public class Organism : MonoBehaviour 
{
    public enum nutritionType
    {
        plant,
        meat,
    };

    //Whether or not the object should be destroyed
    public bool hideable = false;
    public nutritionType nutType;
    //Value that denotes how much of the Organism is food, when this value is nothing the Organism will disappear
    public float nutritionalMass;

    //To allow Animals to be killed health is stored here
    public float health = 0;

	// Use this for initialization
	void Start () 
    {
	
	}

    public void Initialise(int count, float _NutritionVal)
    {
        name = "organism" + count;
        nutritionalMass = _NutritionVal;
        nutType = nutritionType.plant;
    }
	
	// Update is called once per frame
	protected virtual void  Update () 
    {
        if(!hideable)
        {
            if(nutritionalMass <= 0)
            {
                //Debug.Log(renderer.material.color.a);
                deadLogic();
            }

            if(renderer.material.color.a <= 0.009)
            {
                this.hideable = true;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
	}

    protected virtual void deadLogic()
    {
        Color tempColor = renderer.material.color;
        tempColor.a = Mathf.Lerp(tempColor.a, 0, Time.deltaTime);
        renderer.material.color = tempColor;
    }
}