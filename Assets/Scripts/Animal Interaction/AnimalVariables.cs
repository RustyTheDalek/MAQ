using UnityEngine;
using System.Collections;

public struct AnimalVariables{

    public float nutritionMass; 
    public float moveForce;
    public float maxVelocity;
    public float jumpStrength;
    public float HOCLevel;
    public float maxHealth;
    public float health;
    public float maxHunger;
    public float hunger;
    public float attackStrength;

    public void reset()
    {
        nutritionMass = -1; 
        moveForce = -1;
        maxVelocity = -1;
        jumpStrength = -1;
        HOCLevel = -1;
        maxHealth = -1;
        health = -1;
        maxHunger = -1;
        hunger = -1;
        attackStrength = -1;
    }

}
