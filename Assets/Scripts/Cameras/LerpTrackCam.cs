using UnityEngine;
using System.Collections;

public class LerpTrackCam : TrackCam
{
    private float xMin, yMin, width, height;

    [Range (0, 1)]
    public float speed =1f;

	public float originalSpeed;

    public Rect targetRect;

    Vector3 targetPosition, resetPosition;

    public SimpleCreationGUI playerCanvas;

    float originalDistance;

    public Quaternion originalRotation;

    Ray toTarget
    {
        get
        {
            if (target)
            {
                Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
                return new Ray(transform.position, dirToTarget);
            }

            Debug.LogWarning("No target");
            return new Ray(transform.position, Vector3.zero);
        }
    }

    RaycastHit hit;

    public enum CameraState
    {
        lerping,
        reverting,
        none,
    };

    public CameraState camState = CameraState.none;

    void Awake()
    {
        playerCanvas = GetComponentsInChildren<SimpleCreationGUI>(true)[0];

        playerCanvas.gameObject.SetActive(false);

        if (this.camera == Camera.main)
            enabled = false;
        else
            gameObject.SetActive(false);

        originalDistance = distance;
        originalRotation = transform.rotation;

		gPad = new Controller(playerID-1);

		originalSpeed = speed;
    }

	// Use this for initialization
	void Start () {


	}

    public override void setUp(GameObject _Target, Rect viewRect)
    {
        resetPosition = transform.position;

        camState = CameraState.lerping;

        target = _Target;

        targetRect = viewRect;
    }

	// Update is called once per frame
	void Update () {

        switch(camState)
        {
            case CameraState.lerping:

                if (target)
                {
                    if (Physics.Raycast(toTarget, out hit))
                    {
                        if (hit.collider.tag == target.tag || (hit.transform.root && hit.transform.root.tag == target.tag))
                        {
                            if (Physics.Raycast(transform.position, -toTarget.direction, out hit))
                            {
                                if (hit.distance > .5f && distance > originalDistance)
                                {
                                    distance -= .2f;
                                }
                            }
                            else
                            {
                                distance -= .2f;
                            }
                        }
                        else
                        {
                            distance += .2f;
                        }
                    }
                    else
                    {
                        distance += .2f;
                    }

					if(canRotate)
					{
	                    gPad.updateStates();

	                    playerOffset += new Vector3(1 * gPad.rightStick.y, 1 * gPad.rightStick.x, 0) * Time.deltaTime * turnSpeed;

	                    playerOffset = new Vector3(Mathf.Clamp(playerOffset.x, -10, 20), playerOffset.y, 0);

	                    if (gPad.isKeyDown(XKeyCode.RightStick))
	                    {
	                        playerOffset = Vector3.zero;
	                    }
					}

                    distance = Mathf.Clamp(distance, originalDistance, -target.transform.localScale.x * 2.5f);
                    targetPosition = target.transform.position + targetOffset + Quaternion.Euler(angle + playerOffset) * (Vector3.back * distance);

                    transform.position = Vector3.Slerp(transform.position, targetPosition, speed);

                    Debug.DrawLine(transform.position, targetPosition, Color.green);

                    transform.LookAt(target.transform.position + targetOffset);
                }

                xMin = Mathf.Lerp(camera.rect.xMin, targetRect.xMin, speed);
                yMin = Mathf.Lerp(camera.rect.yMin, targetRect.yMin, speed);
                width = Mathf.Lerp(camera.rect.width, targetRect.width, speed);
                height = Mathf.Lerp(camera.rect.height, targetRect.height, speed);

                camera.rect = new Rect(xMin, yMin, width, height);

                break;

            case CameraState.reverting:

                transform.position = Vector3.Slerp(transform.position, targetPosition, speed);

                xMin = Mathf.Lerp(camera.rect.xMin, targetRect.xMin, speed);
                yMin = Mathf.Lerp(camera.rect.yMin, targetRect.yMin, speed);
                width = Mathf.Lerp(camera.rect.width, targetRect.width, speed);
                height = Mathf.Lerp(camera.rect.height, targetRect.height, speed);

                camera.rect = new Rect(xMin, yMin, width, height);

                if ((Mathf.Abs(transform.position.sqrMagnitude - targetPosition.sqrMagnitude) < 10))
                {
                    transform.position = targetPosition;
                    camera.rect = targetRect;
                    camState = CameraState.none;
                    speed = .1f;

                    if (camera == Camera.main)
                    {
                        enabled = false;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                }

                break;
        }
	}

    internal void reset()
    {
        camState = CameraState.reverting;

        speed = 1f;
        targetPosition = resetPosition;
        transform.rotation = originalRotation;

        targetRect = new Rect(0, 0, 1, 1);
    }
}
