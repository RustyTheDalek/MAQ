using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalAI : Animal 
{

    //Variables for finding direction and distance to other object in world
    Vector3 directionToOtherOrg = Vector3.zero;
    float distanceToOtherOrg = 0;

    public static int sight = 25;

    public Animal biggestDanger;
    public Organism bestFood;

    //Weighings used to scaled distance and HOC differences appropriatley
    static int distanceWeighting = 4;
    static int HOCWeighting = 25;

    float HOCDifference;
    static float defaultHOCDifference = 50;

    //Wander variables
    static float wanderTimer = 0;
    static float waitTimer = 0;
    static int maxWanderTime = 3;
    static int maxWaitTime = 5;
    Vector3 wanderDirection;

    Vector3 foodReactionForce;
    Vector3 dangerReactionForce;
    Vector3 wanderForce;

	// Use this for initialization
	protected override void Start () 
    {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () 
    {
        if(this.alive() && !hideable)
        {
            if(biggestDanger != null)
            {
                dangerReactionForce.x = reactToDanger(biggestDanger);
                addClampedForce(dangerReactionForce);
            }
            else if(bestFood != null && isHungry())
            {
                foodReactionForce.x = reactToFood(bestFood) * 2;
                addClampedForce(foodReactionForce);

                //If the food is close enough
                if(this.collider.bounds.Intersects(bestFood.collider.bounds))
                {
                    if(attackTimer >= 1)
                    {
                        attackOrEat(bestFood);
                        attackTimer = 0;
                    }
                    else
                    {
                        
                        attackTimer += Time.deltaTime;
                    }
                }
            }
            else
            {
                wanderForce.z = wander();
                addClampedForce(wanderForce);
            }

        }       

        base.Update();

	}

    public float wander()
    {
        if(wanderTimer <= 0)
        {
            if(waitTimer <= 0)
            {
                //Give a random time again for wandering
                wanderTimer = Random.Range(1, maxWanderTime);
                
                if(Random.value > 0.5f) //50/50 chance of going left or right
                {
                    wanderDirection = Vector3.back;
                }
                else
                {
                    wanderDirection = Vector3.forward;
                }
                
                waitTimer = Random.Range(1, maxWaitTime);
            }
            else
            {
                waitTimer -=Time.deltaTime;
                //Debug.Log("Waiting");
            }

            return 0;
        }
        else
        {
            wanderTimer -= Time.deltaTime;
            //Debug.Log("Wandering");
            
            return wanderDirection.z * (moveForce * 0.75f) * Time.deltaTime;
            
            //Debug.Log("WanderForce: " + wanderForce);
            //clampVelocity(wanderForce);
        }
    }

    //React using HOCLevel
    //TODO: Add more advanced behaviour so that these reaction are also based on hunger and health
    public float reactToDanger(Animal otherAnimal)
    {
        directionAndDistance(otherAnimal.transform.position); 

        if(otherAnimal.HOCLevel > this.HOCLevel)
        {
            HOCDifference = otherAnimal.HOCLevel - HOCLevel;      
        }
        else //Apply default differnce
        {
            HOCDifference = defaultHOCDifference;
        }            

        //The standard force away the animal scaled by the move force
        float dangerReactionForce = directionToOtherOrg.x * -1 * moveForce * Time.deltaTime; 

        //Scale the reaction force to the distance between the animals
        dangerReactionForce *=  (distanceWeighting/distanceToOtherOrg);


        dangerReactionForce *= (HOCDifference/HOCWeighting);

        return dangerReactionForce;
    }

    public float reactToFood(Organism organism)
    {

        directionAndDistance(organism.transform.position);

        //Hungrier the animal the stronger the force
        float hungerWeighting = (100 - hunger)/50;

        float hungerReactionForce = directionToOtherOrg.x * moveForce * Time.deltaTime;

        hungerReactionForce *= hungerWeighting;
                    
        return hungerReactionForce;

    }

    void directionAndDistance(Vector3 OtherPosition)
    {
        directionToOtherOrg = Vector3.zero;
        
        distanceToOtherOrg = 0;

        //Finds direction towards other animals
        directionToOtherOrg = OtherPosition - transform.position;

        /*Find magnituede of vector to be used as relative distance 
        could use distance but more costly and only used to weight
        reactions so stronger reactions to closer animals*/
        distanceToOtherOrg = Vector3.Magnitude(directionToOtherOrg);

        //Distance below 2 cause to strong a force which causes "Bouncing when moving"
        distanceToOtherOrg = Mathf.Clamp(distanceToOtherOrg, 2, 20);

        //Normalise direction so direction can be scaled by particular force
        directionToOtherOrg = Vector3.Normalize(directionToOtherOrg);

    }
    
    public bool canSee(Vector3 organismPos)
    {
        distanceToOtherOrg = Vector3.Distance(organismPos, this.transform.position);
        
        if(distanceToOtherOrg < sight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //This function can be added to in various ways to decide whether the animal is dangerous
    //TODO: IF otherAnimal is attacking you then it is dangerous
    public bool isInDanger(Animal OtherAnimal)
    {
        if(this.HOCLevel < OtherAnimal.HOCLevel)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //TODO:Add to BiggestDanger function to account for HOCLevel when reacting
    public bool moreDangerous(Animal otherAnimal)
    {
        if(biggestDanger != null)
        {
            if(Vector3.SqrMagnitude(otherAnimal.transform.position - transform.position) < Vector3.SqrMagnitude(biggestDanger.transform.position - transform.position))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(otherAnimal != null)
        {
            return true;
        }
        else
        {
            Debug.LogError("Both Animals are null");
            return false;
        }

    }

    //TODO:Apply nutritional mass, logic for Omnivores
    public bool betterFood(Organism organism)
    {
        if(bestFood != null)
        {
            if(Vector3.SqrMagnitude(organism.transform.position - transform.position) < Vector3.SqrMagnitude(bestFood.transform.position - transform.position))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(organism != null)
        {
            return true;
        }
        else
        {
            Debug.LogError("Both Animals are null");
            return false;
        }
        
    }

    //Same as isInDanger function this can be added to to allow for more complex logic
    public bool canEat(Organism otherOrg)
    {
        switch(otherOrg.nutType)
        {
            case nutritionType.meat:

                if(HOCLevel > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case nutritionType.plant:

                if(HOCLevel < 100)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }

        return false;
    }

    public bool isHungry()
    {
        if(hunger < 50)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void addClampedForce(Vector3 forceToAdd)
    {
        //TODO:Change to sqrMagnitude
        if(forceToAdd.z > 0 && rigidbody.velocity.z < maxXVelocity)
        {
            rigidbody.AddForce(forceToAdd);
        }
        else if(forceToAdd.z < 0 && rigidbody.velocity.z > - maxXVelocity)
        {
            rigidbody.AddForce(forceToAdd);
        }
    }
}
