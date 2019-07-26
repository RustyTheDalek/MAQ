using UnityEngine;
using System.Collections;

public class TEST_bone : MonoBehaviour
{
	public bool display;

    //Public models
    public GameObject head;
    public GameObject neck;
    public GameObject body;
    public GameObject leg;
    public GameObject shin;
    public GameObject foot;
    public Color modelCol = Color.green;
    //Bones model
    GameObject model;

    //Instanciated copies
    GameObject head0;
    GameObject neck0;
    public GameObject body0;
    //Numbered 1-4 starting front left clickwise
    GameObject leg1,leg2,leg3,leg4;
    GameObject shin1,shin2,shin3,shin4;
    GameObject foot1,foot2,foot3,foot4;

    public GameObject ground;
    public BoxCollider collision;

    bool drawingBones = true;

    void Start()
    {
        //List of test bone names examples for refrence
        //pasted__RFL_knee (knee)
        //pasted__RFL_r (leg root)
        //pasted__LFL_shin (shin)
        //pasted__N_bottom (base of neck)
        //pasted__N_middle (neck)
        //pasted__N_top (neck head base)
        //pasted__H_front (head)
        //pasted__H_back (head) cvdfsvdfbgnhv'dln.,cs.,c.x //pasted__RFL_knee (knee)
        //pasted__LBL_foot(foot)
        //pasted__B_1 (body)
        //pasted__Root(body)
        
        // Instantiate(brick, new Vector3(x, y, 0), Quaternion.identity);

        head0 = (GameObject) Instantiate(head, transform.position, transform.rotation);
        neck0 = (GameObject) Instantiate(neck, transform.position, transform.rotation);
        body0 =(GameObject) Instantiate(body, transform.position, transform.rotation);
        
        leg1 = (GameObject) Instantiate(leg, transform.position, transform.rotation);
        leg2 = (GameObject) Instantiate(leg, transform.position, transform.rotation);
        leg3 = (GameObject) Instantiate(leg, transform.position, transform.rotation);
        leg4 = (GameObject) Instantiate(leg, transform.position, transform.rotation);
        
        shin1 = (GameObject) Instantiate(shin, transform.position, transform.rotation);
        shin2 = (GameObject) Instantiate(shin, transform.position, transform.rotation);
        shin3 = (GameObject) Instantiate(shin, transform.position, transform.rotation);
        shin4 = (GameObject) Instantiate(shin, transform.position, transform.rotation);

        foot1 = (GameObject) Instantiate(foot, transform.position, transform.rotation);
        foot2 = (GameObject) Instantiate(foot, transform.position, transform.rotation);
        foot3 = (GameObject) Instantiate(foot, transform.position, transform.rotation);
        foot4 = (GameObject) Instantiate(foot, transform.position, transform.rotation);

        //Parent each object to the character
        //Find bone

        model = transform.Find("/" + this.name + "/pasted__Root/pasted__N_bottom/pasted__LFL_r").gameObject;
        //Sets parent
        leg1.transform.parent = model.transform;
        leg1.transform.localPosition = Vector3.zero;
        //leg1.transform.localRotation = Quaternion.identity;
        leg1.transform.localScale = Vector3.one;
        
        model = GameObject.Find("/" + this.name + "/pasted__Root/pasted__N_bottom/pasted__RFL_r");
        //Sets parent
        leg2.transform.parent = model.transform;
        leg2.transform.localPosition = Vector3.zero;
        //leg2.transform.localRotation = Quaternion.identity;
        leg2.transform.localScale = Vector3.one;
        
        model = GameObject.Find("/" + this.name + "/pasted__Root/pasted__B_1/pasted__B_2/pasted__RBL_r");
        //Sets parent
        leg3.transform.parent = model.transform;
        leg3.transform.localPosition = Vector3.zero;
        //leg3.transform.localRotation = Quaternion.identity;
        leg3.transform.localScale = Vector3.one;
        
        model = GameObject.Find("/" + this.name + "/pasted__Root/pasted__B_1/pasted__B_2/pasted__LBL_r");
        //Sets parent
        leg4.transform.parent = model.transform;
        leg4.transform.localPosition = Vector3.zero;
        //leg4.transform.localRotation = Quaternion.identity;
        leg4.transform.localScale = Vector3.one;
        
        //Find bone
        model = GameObject.Find("/" + this.name + "/pasted__Root/pasted__N_bottom/pasted__N_middle/pasted__N_top/pasted__H_back");
        //Sets parent
        head0.transform.parent = model.transform;
        head0.transform.localPosition = Vector3.zero;
        //head0.transform.localRotation = Quaternion.identity;
        head0.transform.localScale = Vector3.one;
        
        //Find bone
        model = GameObject.Find("/" + this.name + "/pasted__Root");
        //Sets parent
        body0.transform.parent = model.transform;
        body0.transform.localPosition = Vector3.zero;
        //body0.transform.localRotation = Quaternion.identity;
        body0.transform.localScale = Vector3.one;
        
        //Find bone
        model = GameObject.Find("/" + this.name + "/pasted__Root/pasted__N_bottom/pasted__LFL_r/pasted__LFL_knee/pasted__LFL_shin");
        //Sets parent
        shin1.transform.parent = model.transform;
        shin1.transform.localPosition = Vector3.zero;
        //shin1.transform.localRotation = Quaternion.identity;
        shin1.transform.localScale = Vector3.one;
        
        //Find bone
        model = GameObject.Find("/" + this.name + "/pasted__Root/pasted__N_bottom/pasted__RFL_r/pasted__RFL_knee/pasted__RFL_shin");
        //Sets parent
        shin2.transform.parent = model.transform;
        shin2.transform.localPosition = Vector3.zero;
        //shin2.transform.localRotation = Quaternion.identity;
        shin2.transform.localScale = Vector3.one;
        
        //Find bone
        model = GameObject.Find("/" + this.name + "/pasted__Root/pasted__B_1/pasted__B_2/pasted__RBL_r/pasted__RBL_knee/pasted__RBL_shin");
        //Sets parent
        shin3.transform.parent = model.transform;
        shin3.transform.localPosition = Vector3.zero;
        //shin3.transform.localRotation = Quaternion.identity;
        shin3.transform.localScale = Vector3.one;
        
        //Find bone
        model = GameObject.Find("/" + this.name + "/pasted__Root/pasted__B_1/pasted__B_2/pasted__LBL_r/pasted__LBL_knee/pasted__LBL_shin");
        //Sets parent
        shin4.transform.parent = model.transform;
        shin4.transform.localPosition = Vector3.zero;
        //shin4.transform.localRotation = Quaternion.identity;
        shin4.transform.localScale = Vector3.one;

        //Find bone
        model = GameObject.Find("/" + this.name + "/pasted__Root/pasted__N_bottom/pasted__LFL_r/pasted__LFL_knee/pasted__LFL_shin/pasted__LFL_foot");
        //Sets parent
        foot1.transform.parent = model.transform;
        foot1.transform.localPosition = Vector3.zero;
        //foot1.transform.localRotation = Quaternion.identity;
        foot1.transform.localScale = Vector3.one;

        //Find bone
        model = GameObject.Find("/" + this.name + "/pasted__Root/pasted__N_bottom/pasted__RFL_r/pasted__RFL_knee/pasted__RFL_shin/pasted__RFL_foot");
        //Sets parent
        foot2.transform.parent = model.transform;
        foot2.transform.localPosition = Vector3.zero;
        //foot2.transform.localRotation = Quaternion.identity;
        foot2.transform.localScale = Vector3.one;

        //Find bone
        model = GameObject.Find("/" + this.name + "/pasted__Root/pasted__B_1/pasted__B_2/pasted__RBL_r/pasted__RBL_knee/pasted__RBL_shin/pasted__RBL_foot");
        //Sets parent
        foot3.transform.parent = model.transform;
        foot3.transform.localPosition = Vector3.zero;
        //foot3.transform.localRotation = Quaternion.identity;
        foot3.transform.localScale = Vector3.one;

        //Find bone
        model = GameObject.Find("/" + this.name + "/pasted__Root/pasted__B_1/pasted__B_2/pasted__LBL_r/pasted__LBL_knee/pasted__LBL_shin/pasted__LBL_foot");
        //Sets parent
        foot4.transform.parent = model.transform;
        foot4.transform.localPosition = Vector3.zero;
        //foot4.transform.localRotation = Quaternion.identity;
        foot4.transform.localScale = Vector3.one;

        //Find bone
        model = GameObject.Find("/" + this.name + "/pasted__Root/pasted__N_bottom/pasted__N_middle");
        //Sets parent
        neck0.transform.parent = model.transform;
        neck0.transform.localPosition = Vector3.zero;
        //neck0.transform.localRotation = Quaternion.identity;
        neck0.transform.localScale = Vector3.one;


		if(!display)
		{
	        //Getting collision box to encompass whole Animal
	        collision = GetComponent<BoxCollider>() as BoxCollider;
//
            float sizeX = Mathf.Abs(foot1.renderer.bounds.min.x - foot2.renderer.bounds.max.x) * 1/this.transform.localScale.x;
            float sizeY = Mathf.Abs(foot4.renderer.bounds.min.y - neck0.renderer.bounds.max.y) * 1/this.transform.localScale.x;
            float sizeZ = Mathf.Abs(foot4.renderer.bounds.min.z - neck0.renderer.bounds.max.z) * 1/this.transform.localScale.x;

            Debug.DrawLine(foot4.renderer.bounds.min,body0.renderer.bounds.max, Color.red);
	        
	        collision.size = new Vector3(sizeX, sizeY, sizeZ);
//
            collision.center = new Vector3(0, 0,0);

	        //Setting Ground to collision box to be at the Animals Feet
	        ground = transform.FindChild("ground").gameObject;
	        ground.transform.position = Vector3.Lerp(foot1.transform.position, foot3.transform.position, 0.5f);

	//        Debug.Log(ground.transform.localScale);

	        ground.transform.localScale = new Vector3(collision.size.x, 2, collision.size.z*0.9f);

//	        Debug.Log(ground.transform.localScale);
		}

    }

//    void Start ()
//    {
//    
//       
//    }
    // Update is called once per frame
    void Update ()
    {        
        colorModel();

        if(Input.GetKeyDown(KeyCode.Q))
        {
            switch(drawingBones)
            {
                case true:

                    drawingBones = false;
                    break;

                case false:

                    drawingBones = true;
                    break;
            }
        }

        if(drawingBones && head0 != null)
        {
            drawBones();
        }
    }

    public void explodeParts()
    {
        head0.transform.parent = null;
        neck0.transform.parent = null;
        leg1.transform.parent = null;
        leg2.transform.parent = null;
        leg3.transform.parent = null;
        leg4.transform.parent = null;
        shin1.transform.parent = null;
        shin2.transform.parent = null;
        shin3.transform.parent = null;
        shin4.transform.parent = null;
        foot1.transform.parent = null;
        foot2.transform.parent = null;
        foot3.transform.parent = null;
        foot4.transform.parent = null;

        AddColAndRig(head0);
        AddColAndRig(neck0);
        AddColAndRig(leg1);
        AddColAndRig(leg2);
        AddColAndRig(leg3);
        AddColAndRig(leg4);
        AddColAndRig(shin1);
        AddColAndRig(shin2);
        AddColAndRig(shin3);
        AddColAndRig(shin4);
        AddColAndRig(foot1);
        AddColAndRig(foot2);
        AddColAndRig(foot3);
        AddColAndRig(foot4);
    }

    void AddColAndRig(GameObject gO)
    {
        gO.AddComponent<Rigidbody>();
        gO.AddComponent<CapsuleCollider>();

        Vector3 explodeForce = new Vector3(Random.value * 2 - 1, 
                                           Random.value * 2 - 1, 
                                           Random.value * 2 - 1);
        float randomForce = Random.Range(0,20);

        gO.rigidbody.AddForce(explodeForce * randomForce, ForceMode.Impulse);

        gO.AddComponent<destroyScript>();
    }

    void colorModel()
    {
        //Although we have multiple models they all share the same colour 
        //(for now)
        if(head0 != null && head0.renderer.material.color != modelCol)
        {
            head0.renderer.material.color = modelCol;

            neck0.renderer.material.color = modelCol;

            body0.renderer.material.color = modelCol;

            leg1.renderer.material.color = modelCol;
            leg2.renderer.material.color = modelCol;
            leg3.renderer.material.color = modelCol;
            leg4.renderer.material.color = modelCol;

            shin1.renderer.material.color = modelCol;
            shin2.renderer.material.color = modelCol;
            shin3.renderer.material.color = modelCol;
            shin4.renderer.material.color = modelCol;

            foot1.renderer.material.color = modelCol;
            foot2.renderer.material.color = modelCol;
            foot3.renderer.material.color = modelCol;
            foot4.renderer.material.color = modelCol;
        }
    }

    void drawBones()
    {
        Debug.DrawLine(head0.transform.position, neck0.transform.position, Color.blue);
        Debug.DrawLine(neck0.transform.position, body0.transform.position, Color.blue);

        Debug.DrawLine(body0.transform.position, leg1.transform.position, Color.blue);
        Debug.DrawLine(body0.transform.position, leg2.transform.position, Color.blue);
        Debug.DrawLine(body0.transform.position, leg3.transform.position, Color.blue);
        Debug.DrawLine(body0.transform.position, leg4.transform.position, Color.blue);

        Debug.DrawLine(leg1.transform.position, shin1.transform.position, Color.blue);
        Debug.DrawLine(leg2.transform.position, shin2.transform.position, Color.blue);
        Debug.DrawLine(leg3.transform.position, shin3.transform.position, Color.blue);
        Debug.DrawLine(leg4.transform.position, shin4.transform.position, Color.blue);

        Debug.DrawLine(shin1.transform.position, foot1.transform.position, Color.blue);
        Debug.DrawLine(shin2.transform.position, foot2.transform.position, Color.blue);
        Debug.DrawLine(shin3.transform.position, foot3.transform.position, Color.blue);
        Debug.DrawLine(shin4.transform.position, foot4.transform.position, Color.blue);

    }
}