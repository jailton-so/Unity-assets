using UnityEngine;
using System.Collections;

public class OrbitalCamera : MonoBehaviour
{
    [Header("debug")]
	public bool debugCollisionBounds = false;
    public bool debugOffsetPoint = false;
    private GameObject offsetPoint;
	private GameObject collisionBounds;

    [Header("position")]
	public Transform mainCamera;
	public float distance = 4.0f;
    public float distanceOffset = 0.1f;
	public float xOffset = 0f;
	public float yOffset = 0f;
	public float zOffset = 0f;

	[Header("sensitivity")]
	public float sensitivity = 3.0f;
	public bool useSenSmooth = false;
	public float sensSmooth = 5f;

	[Header("Invert Mouse")]
	public bool invertVertical = false;
	public bool invertHorizontal = false;

	[Header("Limits")]
	public bool limit = true;
	public float limitVertical = 80f;

	[Header("Camera Collision")]
	public bool useCamCollision = true;
	public float radius = 0.2f;
	public bool useCamSmooth = true;
	public float camSmooth = 10f;

	private float currentDistance;

	private float rawRotX;
	private float rawRotY;
	private float newRotX;
	private float newRotY;

	void Start()
	{
        mainCamera.SetParent(transform);

		rawRotX = transform.rotation.x;
		rawRotY = transform.rotation.y;
	}

	void Update()
	{
		MouseLook ();
		CameraPosition ();

		DebugCollisionBounds ();
        DebugOffsetPoint();
	}

	void MouseLook()
	{
		float mouseX = Input.GetAxisRaw ("Mouse X");
		float mouseY = Input.GetAxisRaw ("Mouse Y");

		//apply inverted values if enabled
		if (invertVertical == false)
		{
			rawRotX -= mouseY * sensitivity;
		}
		else
		{
			rawRotX += mouseY * sensitivity;
		}

		if (invertHorizontal == false)
		{
			rawRotY += mouseX * sensitivity;
		}
		else
		{
			rawRotY -= mouseX * sensitivity;
		}

		//limit angle
		if(limit == true)
			rawRotX = Mathf.Clamp(rawRotX, -limitVertical, limitVertical);

		//sensitivitySmooth on/off
		if (useSenSmooth == true)
		{
			newRotX = Mathf.Lerp(newRotX, rawRotX, Time.deltaTime * sensSmooth);
			newRotY = Mathf.Lerp(newRotY, rawRotY, Time.deltaTime * sensSmooth);
		}
		else
		{
			newRotX = rawRotX;
			newRotY = rawRotY;
		}

		//apply rotation
		transform.localRotation = Quaternion.Euler(new Vector3(newRotX, newRotY, 0));
	}

	void CameraPosition()
	{
		//apply positions
		mainCamera.localPosition = new Vector3(0, 0, -currentDistance);

		if (transform.parent != null)
			transform.localPosition = new Vector3 (xOffset, yOffset, zOffset);

		//camera collsion detection
		if (useCamCollision == false)
		{
			currentDistance = distance;
		}
		
		else
		{
			RaycastHit hit;
			Vector3 direction = transform.position - mainCamera.position;

			if(Physics.CapsuleCast(transform.position, transform.position, radius, -direction, out hit, distance))
			{
				if (useCamSmooth == false)
				{
					currentDistance = Vector3.Distance (transform.position, hit.point) - distanceOffset;
				}
				else
				{
					float tempDistance = Vector3.Distance (transform.position, hit.point) - distanceOffset;
					currentDistance = Mathf.MoveTowards (currentDistance, tempDistance, Time.deltaTime*camSmooth);
				}
			}
			else
			{
				if (useCamSmooth == false)
				{
					currentDistance = distance;
				}
				else
				{
					currentDistance = Mathf.MoveTowards (currentDistance, distance, Time.deltaTime*camSmooth);
				}
			}

		}
	}

    void DebugOffsetPoint()
    {
        if (debugOffsetPoint == true)
        {
            if (offsetPoint == null)
            {
                offsetPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				offsetPoint.name = "DEBUG-OffsetPoint";
				Destroy (offsetPoint.GetComponent<SphereCollider>());
				offsetPoint.transform.SetParent (this.transform);

                offsetPoint.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                Material material = new Material(Shader.Find("Unlit/Color"));
				material.color = Color.blue;
                offsetPoint.GetComponent<Renderer>().material = material;
            }
            else
                offsetPoint.transform.position = transform.position;

        }
        else
        {
            if (offsetPoint != null)
                Destroy(offsetPoint);
        }
    }

	void DebugCollisionBounds()
	{
		if (debugCollisionBounds == true)
		{
			if (collisionBounds == null)
			{
				collisionBounds = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
				collisionBounds.name = "DEBUG-CollisionBounds";
				Destroy (collisionBounds.GetComponent<CapsuleCollider>());
				collisionBounds.transform.SetParent (this.transform);

				collisionBounds.transform.localRotation = Quaternion.Euler(-90, 0, 0);

				Material material = new Material(Shader.Find("Unlit/Color"));
				material.color = Color.green;
				collisionBounds.GetComponent<Renderer>().material = material;
			}
			else
				collisionBounds.transform.localScale = new Vector3(radius*2, -currentDistance/2, radius*2);
				collisionBounds.transform.localPosition = new Vector3(0, 0, -currentDistance/2);

		}
		else
		{
			if (collisionBounds != null)
				Destroy(collisionBounds);
		}
	}
}