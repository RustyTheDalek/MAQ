using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CursorController : PlayerObject
{

    Controller gPad;

    Ray playerRay;
    RaycastHit hit;
    //Selected part and the selected parts parent
    GameObject sPart, sPParent, closestObject;

    const float springStr = 10, springMaxDist = 0.05f;

    //public Vector3 movement;

    public Image[] objs;

    float speed = 500;

    float nearest = 100f;

    int totalParts;

    RectTransform rect;

	// Use this for initialization
	void Start () {

        gPad = new Controller(playerID-1);

        objs = GetComponentsInChildren<Image>();

        objs[0].color = playerColour/255;
        objs[1].color = playerColour/255;

        rect = GetComponent<RectTransform>();

	}

    void OnLevelWasLoaded(int level)
    {
    }
     
    // Update is called once per frame
	void Update () {

        gPad.updateStates();

        switch(CreatureCreating.createState)
        {

            case CState.creating:

                #region movement code

#if UNITY_EDITOR

            Vector3 movement = new Vector3();

            if (Input.GetKey(KeyCode.W))
            {
                movement.y++;
            }

            if (Input.GetKey(KeyCode.S))
            {
                movement.y--;
            }

            if (Input.GetKey(KeyCode.A))
            {
                movement.x--;
            }

            if (Input.GetKey(KeyCode.D))
            {
                movement.x++;
            }

            transform.position += movement * Time.deltaTime * speed;

#endif
            transform.position += new Vector3(gPad.leftStick.x, gPad.leftStick.y, 0) 
                * Time.deltaTime * speed;

            Vector3 pos = transform.position;
            transform.position = new Vector3(Mathf.Clamp(pos.x, (rect.rect.width * rect.localScale.x)/2, 
                                                                Screen.width - (rect.rect.width * rect.localScale.x)/2), 
                                             Mathf.Clamp(pos.y, (rect.rect.height * rect.localScale.y)/2,
                                                                Screen.height -(rect.rect.height * rect.localScale.y)/2),
                                             pos.z);

            //Picking up objects in world
            if(gPad.isKeyDown(XKeyCode.A) || Input.GetKeyDown(KeyCode.Space))
            {
                //If a hits an object 
                playerRay = Camera.main.ScreenPointToRay(transform.position);

                closestObject = null;
                //Are raycast
                RaycastHit[] sphereHit = Physics.SphereCastAll(Camera.main.transform.position, 1.5f, playerRay.direction);
                for (int i = 0; i < sphereHit.Length; i++)
                {
                    //Finds the nearest objectto the raycast
                    float distance = Vector3.Cross(playerRay.direction, sphereHit[i].point - playerRay.origin).magnitude;
                    if (distance< nearest && sphereHit[i].collider.tag == "Body parts")
                    {
                        closestObject = sphereHit[i].collider.gameObject;
                        hit = sphereHit[i];
                        Debug.Log(closestObject.name);
                        nearest = distance;
                    }
                }

                GameObject hitGO = closestObject;

                if (hitGO)
                {
                    //Debug.Log(hit);
                    Debug.DrawLine(playerRay.origin, hit.point, Color.green,3);

                    //GameObject hitGO = hit.collider.gameObject;
                     //If a body part or a prod
                    if(hitGO.GetComponent<LegJoint>() &&
                        (hitGO.hingeJoint || hitGO.GetComponent<ConfigurableJoint>())
                        && isPlayers(hitGO.GetComponent<BodyPart>()) && hitGO.tag == "Body parts")
                    {
                        //Set selected Objects
                        sPart = hitGO;
                        sPart.rigidbody.velocity = Vector3.zero;
                        sPart.rigidbody.angularVelocity = Vector3.zero;
                            
                        Tools.setAllChildren(sPart, (temp) => temp.layer = 2);

                        Tools.setAllChildren(Actions.setID, sPart, playerID);

                        Tools.setAllChildren(Actions.setMaterial, sPart, AssetManager.playerMaterials[playerID -1]);
                            
                        if(sPart.hingeJoint && sPart.hingeJoint.connectedBody && sPart.hingeJoint.connectedBody.gameObject.GetComponent<FollowMouse>() ||
                            sPart.GetComponent<ConfigurableJoint>() && sPart.GetComponent<ConfigurableJoint>().connectedBody.gameObject.GetComponent<FollowMouse>())
                        {
                            if(sPart.hingeJoint)
                            {
                                sPParent = sPart.hingeJoint.connectedBody.gameObject;
                            }
                            else
                            {
                                sPParent = sPart.GetComponent<ConfigurableJoint>().connectedBody.gameObject;
                            }
                        }
                        else //Body part on Current animal
                        {
                            //Set selected objects
                            if(sPart.hingeJoint)
                            {
                                sPParent = (Instantiate(CreatureCreating.pParentPrefab, sPart.transform.position, Quaternion.Euler(sPart.GetComponent<LegJoint>().restAngle)) as GameObject);
                                sPart.transform.rotation = Quaternion.identity;
                            }
                            else if(sPart.GetComponent<ConfigurableJoint>())
                            {
                                sPParent = Instantiate(CreatureCreating.pParentPrefab, 
                                                        sPart.GetComponent<ConfigurableJoint>().connectedAnchor+ sPart.transform.position, 
                                                        Quaternion.identity) as GameObject;
                            }

//                                Debug.Break();
                                
                            //Set sPart up

                            if(sPart.hingeJoint)
                            {
                                sPart.transform.parent = sPParent.transform;
                                sPart.transform.localPosition = Vector3.zero;
                                sPart.transform.parent = null;
                                sPart.hingeJoint.connectedBody = sPParent.rigidbody;
                            }
                            else if(sPart.GetComponent<ConfigurableJoint>())
                            {
                                sPart.GetComponent<ConfigurableJoint>().connectedBody = sPParent.rigidbody;
                            }
                        }
                            
                        //Set partParent up                   
                        sPParent.GetComponent<FollowMouse>().enabled = true;
                        sPParent.GetComponent<FollowMouse>().followingMouse = true;
                        sPParent.GetComponent<FollowMouse>().player = gameObject;
                        sPParent.GetComponent<FollowMouse>().playerID = playerID;
                        sPParent.rigidbody.useGravity = false;   
                        sPParent.rigidbody.mass = 100;
                            
//                            if(sPart.tag == "Body parts")
//                            {
//                                Tools.setBreakForces(sPart, true);
//                            }
                            
                        //Set sPart up
                        if(!sPart.GetComponent<SpringJoint>())
                        {
                            sPart.AddComponent<SpringJoint>();
                        }
                            
                        sPart.GetComponent<SpringJoint>().connectedBody = sPParent.rigidbody;
                        sPart.GetComponent<SpringJoint>().spring = springStr;
                        sPart.GetComponent<SpringJoint>().maxDistance = springMaxDist;
                    }
                }

                //Reset selected item for pickup
                nearest = 100f;
                hitGO = null;
            }

            //If A is continually pressed with an object selected
            //e.g. being moved by the Controller
            if((gPad.isKey(XKeyCode.A) || Input.GetKey(KeyCode.Space)) && sPart)
            {
                //Fire ray from screen to world
                playerRay = Camera.main.ScreenPointToRay(transform.position);

                //If Raycast hits a body part
                if(Physics.Raycast(playerRay, out hit, 1000) && 
                    hit.collider.tag == "Body parts")
                {
                    //Stop selected Objs parent from following the mouse
                    sPParent.GetComponent<FollowMouse>().followingMouse = false;
                        
                    //Body part will react differently to hovering over other body part
                    if(sPart.tag == "Body parts")
                    {
                        //So we can place it specially
                        sPParent.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                        //If above chest
                        Debug.Log(Vector3.SqrMagnitude(hit.transform.up - Vector3.left));
                        if(Vector3.SqrMagnitude(hit.transform.up - Vector3.left) < 1)
                        {
                            sPParent.transform.rotation = hit.transform.rotation;
                        }
                        else
                        {
                            sPParent.transform.rotation = Quaternion.Inverse(hit.transform.rotation);

                        }
                        sPParent.transform.position = hit.point + hit.normal * transform.localScale.x;
                        sPParent.GetComponent<Light>().color = playerColour;
                        sPParent.GetComponent<Light>().enabled = true;
                        //Constrain rotation
//                            sPart.transform.eulerAngles = new Vector3(sPart.transform.localEulerAngles.x, 0,0);
//                            sPart.rigidbody.constraints = RigidbodyConstraints.FreezeRotationY & RigidbodyConstraints.FreezeRotationZ;
                    }
                }
                else //doesn't hit body part
                {
					if(sPParent)
					{
	                    sPParent.GetComponent<Light>().enabled = false;
	                    //Follow mouse again
	                    sPParent.rigidbody.constraints = RigidbodyConstraints.None;
	                    sPParent.GetComponent<FollowMouse>().followingMouse = true; 
					}
                }
            }

            if((gPad.isKeyUp(XKeyCode.A) || Input.GetKeyUp(KeyCode.Space)) && sPart)
            {
                playerRay = Camera.main.ScreenPointToRay(transform.position);
                    
                //If attaching
                if(Physics.Raycast(playerRay, out hit) && 
                    hit.collider.tag == "Body parts" && 
                    sPart.tag == "Body parts" && 
                    isPlayers(hit.collider.GetComponent<BodyPart>()))
                {
                    Destroy(sPart.GetComponent<SpringJoint>());
//                        sPart.transform.rotation = sPParent.transform.rotation;
                    Tools.setRotation(sPart, sPParent);
                        
                    AddPart(sPart, hit.transform);
                    //addOnGround(sPart);
                        
                    Tools.setAllChildren(sPart, (temp) => temp.layer = 0);
                        
                    //Allow joint to Wiggle
//                        System.Action<GameObject> setJoint = 
//                            (obj) => 
//                        {
//                            if (obj.GetComponent<LegJoint>()) obj.GetComponent<LegJoint>().canWiggle = true;
//                        };
//                        
//                        Tools.setAllChildren(sPart, setJoint, true);
                        
//                        System.Action<GameObject> setSpring = 
//                            (obj) => 
//                        {
//                            if (obj.hingeJoint) obj.hingeJoint.useSpring = true;
//                        };
                        
//                        Tools.setAllChildren(sPart, setSpring, true);
                        
                    sPart = null;
                    Destroy(sPParent);
                }
                else //Letting go
                {  
                    //Reset partParent
                    sPParent.GetComponent<FollowMouse>().followingMouse = false;
                    sPParent.rigidbody.mass = 1e-07f;  
                        
                    //Reset selected object
                    sPart.layer = 0;
                    if(sPart.tag == "Body parts")
                    {
                        Destroy(sPart.GetComponent<SpringJoint>());

                        Tools.setAllChildren(Actions.setID, sPart, 5);

                        //Player5 aka noplayer
                        Tools.setAllChildren(Actions.setMaterial, sPart, AssetManager.playerMaterials[4]);

                        Tools.setAllChildren(Actions.setID, sPart, 0);
                    }
                        
                    //Remove references
                    sPart = null;
                    sPParent = null;
                        
                }
            }
#endregion

            break;
        }
	}

    public void activate()
    {
        if(Game.Current.players.Count > playerID - 1)
            gameObject.SetActive(Game.Current.players[playerID -1]);
    }

    void AddPart(GameObject part, Transform _Parent)
    {        
        Debug.Log(part.name);
        
        totalParts++;

        if(part.GetComponent<LegJoint>())
        {
            part.name = part.GetComponent<LegJoint>().name + totalParts;
        }
        
        if(part.transform.parent != _Parent)
        {
            part.transform.parent = _Parent;

            if(part.transform.localPosition.x < 0)
            {
                Debug.Log("Left side of body");
                part.transform.localEulerAngles += Vector3.up * 180;
            }
        }
        
        if(part.hingeJoint && part.hingeJoint.connectedBody != _Parent.rigidbody)
        {
            part.hingeJoint.connectedBody = _Parent.rigidbody;
        }
        else if(part.GetComponent<ConfigurableJoint>() && part.GetComponent<ConfigurableJoint>().connectedBody != _Parent.rigidbody)
        {
            part.GetComponent<ConfigurableJoint>().connectedBody = _Parent.rigidbody;

            part.GetComponent<ConfigurableJoint>().autoConfigureConnectedAnchor = false;
            part.transform.position = sPParent.transform.position + Vector3.right *2;
            part.GetComponent<ConfigurableJoint>().autoConfigureConnectedAnchor = true;

        }
        
        foreach(Transform child in part.transform)
        {
            AddPart(child.gameObject, part.transform);
        }
    }

    public void addOnGround(GameObject obj)
    {
        if(obj.transform.childCount > 0)
        {
            foreach(Transform child in obj.transform)
            {
                if(child.collider.bounds.min.y < obj.collider.bounds.min.y)
                {
                    addOnGround(child.gameObject);
                }
            }
        }
        else
        {
            obj.AddComponent<OnGround>();
            
            if(obj.transform.parent.GetComponent<OnGround>())
            {
                Destroy(obj.transform.parent.GetComponent<OnGround>());
            }
        }
    }

    public static void setMaterial(GameObject obj, Material mat)
    {
        obj.renderer.material = mat;
        
        foreach(Transform child in obj.transform)
        {
            setMaterial(child.gameObject, mat);
        }
    }  

    bool isPlayers(BodyPart bP)
    {
        if(bP.playerID == 0 || bP.playerID == playerID)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
