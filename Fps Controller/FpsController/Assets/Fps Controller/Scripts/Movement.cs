using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	private CharacterController controller;
	private Keyboard keyboard;

	//configurable
	public bool airMovement = true;

	[Space(10)]
	public float groundAcceleration = 8f;
	public float groundDeceleration = 2f;
	public float groundinverseAcc = 4f;
	[Space(5)]
	public float airAcceleration = 0.5f;
	public float airDeceleration = 0f;
	public float airinverseAcc = 4f;

	[Space(10)]
	public float speed = 4.0f;

	[Space(10)]
	public float jumpSpeed = 8.0f;
	public float gravity = 25.0f;
	public float maxFallSpeed = 50;


	//support variables
	public float fwdValue;
	public float sideValue;
	private float prevFwdValue;
	private float prevSideValue;

	private float acceleration;
	private float deceleration;
	private float inverseAcc;

	public Vector3 moveDirection;
	public Vector3 prevMoveDirection;
	public Vector3 smoothedMoveDirection;
	public float smooth;

	public float gravityVel;

	public bool prevGrounded;

	void Start()
	{
		controller = GetComponent<CharacterController>();
		keyboard = GetComponent<Keyboard>();
	}

	void Update()
	{
		//FORWARD INPUT VALUES--------------------------------------------------------------
		if(Input.GetKey(keyboard.mForward) && !Input.GetKey(keyboard.mBackward))
		{
			if (fwdValue < 0)
				fwdValue = Mathf.MoveTowards(fwdValue, 0f, Time.deltaTime * acceleration * inverseAcc);
			else
				fwdValue = Mathf.MoveTowards(fwdValue, 1f, Time.deltaTime * acceleration);
		}
		if(Input.GetKey(keyboard.mBackward) && !Input.GetKey(keyboard.mForward))
		{
			if (fwdValue > 0)
				fwdValue = Mathf.MoveTowards(fwdValue, 0f, Time.deltaTime * acceleration * inverseAcc);
			else
				fwdValue = Mathf.MoveTowards(fwdValue, -1f, Time.deltaTime * acceleration);
		}
		if(!Input.GetKey(keyboard.mForward) && !Input.GetKey(keyboard.mBackward))
		{
			fwdValue =  Mathf.MoveTowards(fwdValue, 0f, Time.deltaTime * deceleration);
		}
		if(Input.GetKey(keyboard.mForward) && Input.GetKey(keyboard.mBackward))
		{
			fwdValue = Mathf.MoveTowards(fwdValue, 0f, Time.deltaTime * deceleration);
		}
		//--------------------------------------------------------------

		//SIDES INPUT VALUES--------------------------------------------------------------
		if(Input.GetKey(keyboard.mRight) && !Input.GetKey(keyboard.mLeft))
		{
			if (sideValue < 0)
				sideValue = Mathf.MoveTowards(sideValue, 0, Time.deltaTime * acceleration * inverseAcc);
			else
				sideValue = Mathf.MoveTowards(sideValue, 1f, Time.deltaTime * acceleration);
		}
		if(Input.GetKey(keyboard.mLeft) && !Input.GetKey(keyboard.mRight))
		{
			if (sideValue > 0)
				sideValue = Mathf.MoveTowards(sideValue, 0, Time.deltaTime * acceleration * inverseAcc);
			else
				sideValue = Mathf.MoveTowards(sideValue, -1f, Time.deltaTime * acceleration);
		}

		if(!Input.GetKey(keyboard.mRight) && !Input.GetKey(keyboard.mLeft))
		{
			sideValue =  Mathf.MoveTowards(sideValue, 0f, Time.deltaTime * deceleration);
		}
		if(Input.GetKey(keyboard.mRight) && Input.GetKey(keyboard.mLeft))
		{
			sideValue = Mathf.MoveTowards(sideValue, 0, Time.deltaTime * deceleration);
		}
		//--------------------------------------------------------------

		prevGrounded = !controller.isGrounded;


		//apply vector of movement
		moveDirection = new Vector3(sideValue, 0, fwdValue);
		moveDirection = transform.TransformDirection(moveDirection);
		//normalize diagonal velocity
		if (moveDirection.magnitude >= 1)
		{
			moveDirection.Normalize();
		}

		moveDirection *= speed;

		if (controller.isGrounded)
		{
			gravityVel = -1;

			acceleration = groundAcceleration;
			deceleration = groundDeceleration;
			inverseAcc = groundinverseAcc;

			smoothedMoveDirection = prevMoveDirection;

			//jump
			if (Input.GetKeyDown(keyboard.jump))
			{
				gravityVel = jumpSpeed;
			}

			Move();
		}
		else
		{
			gravityVel = Mathf.MoveTowards(gravityVel, -maxFallSpeed, Time.deltaTime * gravity);

			acceleration = airAcceleration;
			deceleration = airDeceleration;
			inverseAcc = airinverseAcc;

			//smoothedMoveDirection = Vector3.MoveTowards(smoothedMoveDirection, moveDirection, Time.deltaTime * acceleration);


			if (airMovement == true)
			{
				MoveOnAir();
			}
		}

		if (controller.isGrounded == true)
		{
			if (Input.GetKey(keyboard.mRight) || Input.GetKey(keyboard.mLeft) || Input.GetKey(keyboard.mForward) || Input.GetKey(keyboard.mBackward))
			{
				//prevMoveDirection = Vector3.MoveTowards(prevMoveDirection, moveDirection, Time.deltaTime*acceleration);
				prevMoveDirection = moveDirection;
			}

			if (!Input.GetKey(keyboard.mRight) && !Input.GetKey(keyboard.mLeft) && !Input.GetKey(keyboard.mForward) && !Input.GetKey(keyboard.mBackward))
			{
				//prevMoveDirection = Vector3.MoveTowards(prevMoveDirection, Vector3.zero, Time.deltaTime * deceleration);
				prevMoveDirection = moveDirection;
			}
		}
		if (controller.isGrounded == false)
		{
			prevMoveDirection = smoothedMoveDirection;
			if (Input.GetKey(keyboard.mRight) || Input.GetKey(keyboard.mLeft) || Input.GetKey(keyboard.mForward) || Input.GetKey(keyboard.mBackward))
			{
				smoothedMoveDirection = Vector3.MoveTowards(smoothedMoveDirection, moveDirection, Time.deltaTime * smooth);
			}

			if (!Input.GetKey(keyboard.mRight) && !Input.GetKey(keyboard.mLeft) && !Input.GetKey(keyboard.mForward) && !Input.GetKey(keyboard.mBackward))
			{
				smoothedMoveDirection = Vector3.MoveTowards(smoothedMoveDirection, Vector3.zero, Time.deltaTime * deceleration);
			}
		}
	}

	void Move()
	{
		//move player
		controller.Move(new Vector3(prevMoveDirection.x, gravityVel, prevMoveDirection.z) * Time.deltaTime);
	}
	void MoveOnAir()
	{
		//move player
		//controller.Move(new Vector3(smoothedMoveDirection.x, gravityVel, smoothedMoveDirection.z) * Time.deltaTime);
		controller.Move(new Vector3(prevMoveDirection.x, gravityVel, prevMoveDirection.z) * Time.deltaTime);
		//prevMoveDirection = smoothedMoveDirection;
	}

	void LateUpdate()
	{
		//fix flying if is blocked on jump

		//fix stop on air if blocked while jumping
		if(controller.collisionFlags == CollisionFlags.Above)
		{
			if(gravityVel != 0)
			{
				gravityVel = -3;
			}
		}

		//fix "walk stopped" if blocked by a wall
		if(controller.velocity.z == 0)
		{
			//moveDirection.z = controller.velocity.z;
			//fwdValue = 0;
		}

		//fix "walk stopped" (stop the character if it is blocked by a wall)
		if(controller.velocity.z == 0)
		{
			//moveDirection.z = controller.velocity.z;
		}

	}

}