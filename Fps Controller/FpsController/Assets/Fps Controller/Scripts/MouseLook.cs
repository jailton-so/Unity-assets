using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour
{
	[Header("Camera")]
	public Transform playerCamera;

	[Header("Smooth")]
	public bool useSmooth = false;
	public bool mouseStartMove;
	public bool mouseStopMove;
	public float smooth = 10f;

	[Header("Invert Mouse")]
	public bool invertVertical = false;
	public bool invertHorizontal = false;

	[Header("sensitivity")]
	public float Sensitivity = 3.0f;

	[Header("Limits")]
	public float limitVertical = 80f;

	private float rawRotX;
	private float rawRotY;
	private float newRotX;
	private float newRotY;

	private float prevNewRotX;
	private float prevNewRotY;

	void Start()
	{
		rawRotX = transform.rotation.x;
		rawRotY = transform.rotation.y;
	}

	void Update()
	{
		float mouseX = Input.GetAxisRaw("Mouse X");
		float mouseY = Input.GetAxisRaw("Mouse Y");

		//apply inverted values if enabled
		if (invertVertical == false)
		{
			rawRotX -= mouseY * Sensitivity;
		}
		else
		{
			rawRotX += mouseY * Sensitivity;
		}

		if (invertHorizontal == false)
		{
			rawRotY += mouseX * Sensitivity;
		}
		else
		{
			rawRotY -= mouseX * Sensitivity;;
		}

		//limit angle
		rawRotX = Mathf.Clamp(rawRotX, -limitVertical, limitVertical);

		//use or not smooth
		if (useSmooth == true)
		{
			if(mouseStartMove == true && mouseStopMove == false)
			{
				if (Input.GetAxis("Mouse Y") != 0)
				{
					newRotX = Mathf.Lerp(newRotX, rawRotX, Time.deltaTime * smooth);
					prevNewRotX = newRotX;
				}
				else
				{
					rawRotX = prevNewRotX;
					newRotX = Mathf.Lerp(newRotX, rawRotX, Time.deltaTime * smooth*20);
				}
				if (Input.GetAxis("Mouse X") != 0)
				{
					newRotY = Mathf.Lerp(newRotY, rawRotY, Time.deltaTime * smooth);
					prevNewRotY = newRotY;
				}
				else
				{
					rawRotY = prevNewRotY;
					newRotY = Mathf.Lerp(newRotY, rawRotY, Time.deltaTime * smooth*20);
				}
			}

			if(mouseStopMove == true && mouseStartMove == false)
			{
				if (Input.GetAxis("Mouse Y") == 0)
				{
					newRotX = Mathf.Lerp(newRotX, rawRotX, Time.deltaTime * smooth);
				}
				else
				{
					newRotX = rawRotX;
				}
				if (Input.GetAxis("Mouse X") == 0)
				{
					newRotY = Mathf.Lerp(newRotY, rawRotY, Time.deltaTime * smooth);
				}
				else
				{
					newRotY = rawRotY;
				}
			}
			if(mouseStartMove == true && mouseStopMove == true)
			{
				newRotX = Mathf.Lerp(newRotX, rawRotX, Time.deltaTime * smooth);
				newRotY = Mathf.Lerp(newRotY, rawRotY, Time.deltaTime * smooth);
			}
		}
		else
		{
			newRotX = rawRotX;
			newRotY = rawRotY;
		}

		//apply rotation
		playerCamera.localRotation = Quaternion.Euler(new Vector3(newRotX, 0, 0));
		transform.localRotation = Quaternion.Euler(new Vector3(0, newRotY, 0));
	}
}
