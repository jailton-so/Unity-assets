using UnityEngine;
using System.Collections;

public class Crouch : MonoBehaviour {

	private Transform tr;
	private float dist; // distance to ground

	private CharacterController ch;

	// Use this for initialization
	void Start()
	{

		tr = transform;
		ch = GetComponent<CharacterController>();
		dist = ch.height / 2; // calculate distance to ground
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		float vScale = 1.0f;

		if (Input.GetKey("c"))
		{ // press C to crouch
			vScale = 0.7f;
		}

		float ultScale = tr.localScale.y; // crouch/stand up smoothly 

		Vector3 tmpScale = tr.localScale;
		Vector3 tmpPosition = tr.position;



		tmpScale.y = Mathf.MoveTowards(tr.localScale.y, vScale, 3 * Time.deltaTime);
		tr.localScale = tmpScale;

		tmpPosition.y += dist * (tr.localScale.y - ultScale); // fix vertical position        
		tr.position = tmpPosition;
	}
}
