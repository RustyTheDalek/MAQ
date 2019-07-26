using UnityEngine;
using System.Collections;

public class Animal : Organism
{
    //Default values for Animal
    static float defaultNutritionMass = 50; 
    static float defaultMoveForce = 800;
    static float defaultMaxVelocity = 8;
    static float defaultJumpStrength = 40;
    static float defaultHOCLevel = 0;
    static float defaultMaxHealthAndHunger = 100;
    static float defaultAttackStrength = 30;

    //reference to model
    //public TEST_bone model;

    //Variable for dyanmic HOC behaviour
    public float HOCLevel;

    //Variables for animal movement
    public float moveForce;
    public float maxXVelocity;
    public float jumpStrength;

    public float attackStrength;

    #region Health & Hunger Variables

    public float maxHealth;
    public float maxHunger;

    //Animal Stats
    public float hunger;

    //Rate at which health regenerates
    protected float healthRegen = 0.25f;
    //Rate at which an animal becomes more hungry;
    protected float hungerDecay = 0.5f;
    //Rate at which animal loses health while starving
    protected float healthDecay = 1.5f;

    //value hunger must be greater than in order to start regenerating health
    protected int healthRegenThreshold = 75;
    //value hunger must be less than in order to start decreasing health
    protected int starvingThreshold = 25;

    float healthPercen;
    float hungerPercent;

    #endregion

    RaycastHit hit;

    protected OnGround legs; 
    protected float attackTimer = 2;

    protected Animator animator;

    public enum direction
    {
        left,
        right,
    };

    public direction facing;
	// Use this for initialization
	protected virtual void Start () 
    {
        //model = GetComponent<TEST_bone>();

        HOCLevelColouring();

        legs = GetComponentInChildren<OnGround>();

        facing = direction.right;

        animator = GetComponent<Animator>();

        GetComponentInChildren<ParticleSystem>().transform.position = transform.Find("pasted__Root").transform.position;

	}

    //Setting up animal with default variables
    public void Initialise(int count)
    {
        base.Initialise(count, defaultNutritionMass);
        
        name = "animal" + count;

        moveForce = defaultMoveForce;
        maxXVelocity = defaultMaxVelocity;
        jumpStrength = defaultJumpStrength;
        HOCLevel = defaultHOCLevel;
        maxHealth = defaultMaxHealthAndHunger;
        maxHunger = defaultMaxHealthAndHunger;
        health = defaultMaxHealthAndHunger;
        hunger = defaultMaxHealthAndHunger;
        attackStrength = defaultAttackStrength;
        
        nutType = nutritionType.meat;

        Debug.Log("Note: Spawning animals with default variables");
    }

    //Setting up Animal with set variables
    public void Initialise(int count, AnimalVariables sAV)
    {
        base.Initialise(count, defaultV(sAV.nutritionMass, defaultNutritionMass));

        name = "animal" + count;
        
        moveForce = defaultV(sAV.moveForce, defaultMoveForce);
        maxXVelocity = defaultV(sAV.maxVelocity, defaultMaxVelocity);
        jumpStrength = defaultV(sAV.jumpStrength, defaultJumpStrength);
        HOCLevel = defaultV(sAV.HOCLevel, defaultHOCLevel);
        maxHealth = defaultV(sAV.maxHealth, defaultMaxHealthAndHunger);
        maxHunger = defaultV(sAV.maxHunger, defaultMaxHealthAndHunger);
        health = defaultV(sAV.health, defaultMaxHealthAndHunger);
        hunger = defaultV(sAV.hunger, defaultMaxHealthAndHunger);
        attackStrength = defaultV(sAV.attackStrength, defaultAttackStrength);
        
        nutType = nutritionType.meat;
    }
	
	// Update is called once per frame
	protected override void Update () 
    {
        if(this.alive() && !hideable)
        {
            hungerPercent = (hunger/maxHunger) * 100;

            hungerPercent = Mathf.Clamp(hungerPercent, 0 , 100);

            /*If hunger is above 75% (The animal is 75% full up
              then regenerate health*/
            if(hungerPercent > healthRegenThreshold)
            {
                health += healthRegen * Time.deltaTime;

            }
            else if(hungerPercent < starvingThreshold)
            {
                health -= healthDecay * Time.deltaTime;
            }
            
            //Hunger will slowly decay over time
            hunger -= hungerDecay * Time.deltaTime;
            
            hunger = Mathf.Clamp(hunger, 0, maxHunger);
            
            health = Mathf.Clamp(health, 0, maxHealth);
            
            HOCLevel = Mathf.Clamp(HOCLevel, 0, 100);

            switch(facing)
            {
                case direction.left:
                    
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.x, -transform.localScale.x);
                    //transform.rotation = ;
                    break;
                    
                case direction.right:
                    
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.x, transform.localScale.x);
                    break;
            }

//            if(Physics.Raycast(model.collision.transform.position, Vector3.down, out hit))
//            {
//                Debug.DrawLine (model.collision.transform.position, hit.point, Color.cyan);
//
//                rotatetoGound *= Quaternion.FromToRotation(Vector3.up, hit.normal);
//
//                Debug.DrawRay(hit.point, hit.normal, Color.red);
//            }

//            if(rigidbody.velocity.z > 0.5)
//            {
//                animator.SetBool("Moving", true);
//            }
//            else
//            {
//                animator.SetBool("Moving", false);
//            }
        }
        else
        {
            deadLogic();
        }

        if(!hideable)
        {
            if(nutritionalMass <= 0)
            {
                
                //Color tempColor = model.modelCol;
                //tempColor.a = Mathf.Lerp(tempColor.a, 0, Time.deltaTime);
                //model.modelCol = tempColor;
            }
            
//            if(model.modelCol.a <= 0.009)
//            {
//                this.hideable = true;
//            }
        }
        else
        {
            gameObject.SetActive(false);
        }

	}

    protected virtual void FixedUpdate()
    {
        if(this.rigidbody.velocity.z < -1)
        {
//            Debug.Log("Facing left");
            facing = direction.left;
        }
        else
        {
//            Debug.Log("Facing right");
            facing = direction.right;
        }
    }

    protected bool alive()
    {
        if(health > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual void attackOrEat(Organism otherOrg)
    {
        if(otherOrg.health < this.attackStrength)
        {
            eat(otherOrg);
        }
        else
        {
            attack(otherOrg);
        }
    }

    protected void attack(Organism otherOrg)
    {
        Debug.Log(this.name + " is attacking " + otherOrg.name);
        otherOrg.health -= this.attackStrength;
        Debug.Log("Jump force: " + Vector3.up * defaultJumpStrength * Time.deltaTime);
        otherOrg.rigidbody.AddForce(Vector3.up * defaultJumpStrength * 2 * Time.deltaTime, ForceMode.Impulse);
    }

    protected void eat(Organism otherOrg)
    {
        Debug.Log(this.name + "is eating " + otherOrg.name);
        if(otherOrg.nutritionalMass > this.attackStrength)
        {
            otherOrg.nutritionalMass -= attackStrength;
            otherOrg.health -= this.attackStrength;
            this.hunger += attackStrength * eatingEffectiveness(otherOrg.nutType);
        }
        else
        {
            this.hunger += (attackStrength-otherOrg.nutritionalMass) * eatingEffectiveness(otherOrg.nutType);
            otherOrg.health -= attackStrength - otherOrg.nutritionalMass;
            otherOrg.nutritionalMass = 0;
        }
    }

    protected float eatingEffectiveness(nutritionType otherNutType)
    {
        switch(otherNutType)
        {
            case nutritionType.meat:

                return (this.HOCLevel/100);

            case nutritionType.plant:

                return (100 - this.HOCLevel)/100;
        }

        Debug.LogError("Error: Nutritional type not set");
        return 0;
    }

    protected override void deadLogic()
    {
        //base function for intherited classes to use
        explode();
    }

    public void explode()
    {
        //First remove auto colliders
//        Destroy(GetComponent<BoxCollider>());
        Destroy(this.transform.FindChild("ground").gameObject);
        Destroy(GetComponent<RotateAround>());

        //GetComponent<TEST_bone>().explodeParts();

        //GameObject chest = GetComponent<TEST_bone>().body0;
        GetComponent<BoxCollider>().size = new Vector3(1,1,1);

        GetComponentInChildren<ParticleSystem>().Play();

    }

    public void HOCLevelColouring()
    {
        float GreenStrength = (100 - HOCLevel)/100;
        float BlueStrength =  (HOCLevel)/100;

        GreenStrength = Mathf.Clamp(GreenStrength, 0.0f, 1.0f);
        BlueStrength = Mathf.Clamp(BlueStrength, 0.0f, 1.0f);

        //model.modelCol = new Color(0, GreenStrength, BlueStrength);
    }

    protected static int defaultV(int val, int defaultVal)
    {
        if(val == -1)
        {
            return defaultVal;
        }
        else
        {
            return val;
        }
    }

    protected static float defaultV(float val, float defaultVal)
    {
        if(val == -1)
        {
            return defaultVal;
        }
        else
        { 
            return val;
        }
    }

}
