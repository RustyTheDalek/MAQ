using UnityEngine;
using System.Collections;

public class PlayerAnimal : Animal 
{
    public float distance = 0f;
    public Vector3 lastPosition;

    bool leftDown, rightDown, jumpDown;

    bool attacking;

    float attackWindow = 1;
	// Use this for initialization
	protected override void Start () 
    {
        lastPosition = transform.position;
        base.Start();
	}

    void OnLevelWasLoaded(int level)
    {
//        if(Game.current!= null)
//        {
//            nutritionalMass = Game.current.animal.nutritionMass;
//            moveForce = Game.current.animal.moveForce;
//            maxXVelocity = Game.current.animal.maxVelocity;
//            jumpStrength = Game.current.animal.jumpStrength;
//            HOCLevel = Game.current.animal.HOCLevel;
//            maxHealth = Game.current.animal.maxHealth;
//            health = Game.current.animal.health;
//            maxHunger = Game.current.animal.hunger;
//            hunger = Game.current.animal.hunger;
//            attackStrength =Game.current.animal.attackStrength;
//        }
    }
	
	// Update is called once per frame
	protected override void Update () 
    {
        distance += Vector3.Distance(transform.position, lastPosition); 
        lastPosition = transform.position;
        //Debug.Log(distance);

        if(Input.GetKeyDown(KeyCode.B))
        {
            deadLogic();
        }

        if (Input.GetKey(KeyCode.A)&& legs.onGround)
        {
            leftDown = true;
        }
        
        if (Input.GetKey(KeyCode.D)&& legs.onGround)
        {
            rightDown = true;
        }
        
        if(Input.GetKey(KeyCode.W) && legs.onGround)
        {
            jumpDown = true;
        }     

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            HOCLevel = 0;
            HOCLevelColouring();
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            HOCLevel = 50;
            HOCLevelColouring();
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            HOCLevel = 100;
            HOCLevelColouring();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed");

            if(attackTimer >= 1)
            {
                Debug.Log("Player attacking");
                attacking = true;
                attackTimer = 0;
            }
        }

        if(attackTimer <1)
        {
            attackTimer += Time.deltaTime;
        }

        if(attacking)
        {
            if(attackWindow > 0)
            {
                attackWindow -= Time.deltaTime;
            }
            else
            {
                attackWindow = 1;
                attacking = false;
            }
        }

        base.Update();
	}

    protected override void FixedUpdate()
    {
        if(leftDown && rigidbody.velocity.z > - maxXVelocity)
        {
            rigidbody.AddForce(Vector3.back * moveForce * Time.deltaTime);
            leftDown = false;
        }
        
        if(rightDown && rigidbody.velocity.z < maxXVelocity)
        {
            rigidbody.AddForce(Vector3.forward * moveForce * Time.deltaTime);
            rightDown = false;
        }
        
        if(jumpDown)
        {
            rigidbody.AddForce(Vector2.up * jumpStrength * Time.deltaTime, 
                                 ForceMode.Impulse);
            jumpDown = false;
        }

        base.FixedUpdate();
    }

//    void OnGUI()
//    {       
//        //Health Display
//        GUI.Label(new Rect (0,0,100,20), "Health");
//        GUI.Label(new Rect (50, 0, 100, 50), health.ToString("F1")+ "%");
//        
//        //Hunger Display
//        GUI.Label(new Rect (100,0,300,50), "Hunger");
//        GUI.Label(new Rect (150, 0, 100, 50), hunger.ToString("F1")+ "%");
//        
//        //Distance Display
//        GUI.Label(new Rect (400, 0, 100, 50), "Distance Traveled");
//        GUI.Label(new Rect (470, 0, 100, 50), distance.ToString("F2"));
//        
//    }

    void OnCollisionEnter(Collision collision)
    {
        playerAttackLogic(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        playerAttackLogic(collision);
    }

    void OnTriggerEnter(Collider collision)
    {
        playerAttackLogic(collision);
    }
    
    void OnTriggerStay(Collider collision)
    {
        playerAttackLogic(collision);
    }

    protected override void deadLogic()
    {
        StartCoroutine(BackToShip());
        //base function for intherited classes to use
        base.deadLogic();
    }

    IEnumerator BackToShip()
    {
        yield return new WaitForSeconds (5.5f);
//        CharacterCreation.newAnimal = true;
        Application.LoadLevel("Main Menu");
    }

    void playerAttackLogic(Collision collision)
    {
        if(attacking)
        {
            if(collision.gameObject.tag == "Organism")
            {
                switch(facing)
                {
                    case direction.left:
                        
                        Debug.Log("Facing left");
                        //if colliding object is on the left
                        if((collision.transform.position.z - 
                        transform.position.z) < 0)
                        {
                            Debug.Log("attacking");
                            attackOrEat(
                                collision.gameObject.GetComponent<Organism>());
                            attackWindow = 1;
                            attacking = false;
                        }
                        
                        break;
                        
                    case direction.right:
                        
                        Debug.Log("Facing right");
                        //If colliding object is to the right
                        if((collision.transform.position.z - 
                        transform.position.z) > 0)
                        {
                            Debug.Log("attacking");
                            attackOrEat(
                                collision.gameObject.GetComponent<Organism>());
                            attackWindow = 1;
                            attacking = false;
                        }
                        
                        break;
                }
            }
            
        }
    }

    void playerAttackLogic(Collider collision)
    {
        if(attacking)
        {
            if(collision.gameObject.tag == "Organism")
            {
                switch(facing)
                {
                    case direction.left:
                        
                        Debug.Log("Facing left");
                        //if colliding object is on the left
                        if((collision.transform.position.x - 
                        transform.position.x) < 0)
                        {
                            Debug.Log("attacking");
                            attackOrEat(
                                collision.gameObject.GetComponent<Organism>());
                            attackWindow = 1;
                            attacking = false;
                        }
                        
                        break;
                        
                    case direction.right:
                        
                        Debug.Log("Facing right");
                        //If colliding object is to the right
                        if((collision.transform.position.x - 
                        transform.position.x) > 0)
                        {
                            Debug.Log("attacking");
                            attackOrEat(
                                collision.gameObject.GetComponent<Organism>());
                            attackWindow = 1;
                            attacking = false;
                        }
                        
                        break;
                }
            }
            
        }
    }
}
