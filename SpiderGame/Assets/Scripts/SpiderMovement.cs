using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
	public Rigidbody rb;
	public Transform cam;
	public float playerSpeed = 1f, turnSmoothTime = 0.1f;
	public float raycastRange = 1f;
	public Vector3 playerVelocity;
	
	private bool groundedPlayer;
	private float jumpHeight = 1.0f, gravityValue = -9.81f, turnSmoothVelocity;
	private float horizontal;
	private float vertical;

	private enum GravityStates {Floor, NorthWall, EastWall, SouthWall, WestWall, Ceiling}
	private GravityStates gravityState;


	private void FixedUpdate()
	{
		PlayerMovement();

		PlayerRotation();

		CheckForObsticleToClimb();

		// Changes the height position of the player..
		if (Input.GetButtonDown("Jump"))
		{
			if (groundedPlayer)
			{
				playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
			}

			transform.rotation = Quaternion.Euler(0, 0, 0);
			gravityState = GravityStates.Floor;
		}

		switch (gravityState)
		{
			case GravityStates.Floor:
				print (gravityState);
				playerVelocity.y += gravityValue * Time.deltaTime;
				transform.rotation = Quaternion.Euler(0, 0, 0);
				break;
			case GravityStates.NorthWall:
				print (gravityState);
				playerVelocity.z -= gravityValue * Time.deltaTime;
				transform.rotation = Quaternion.Euler(-90, 0, 0);
				break;
			case GravityStates.EastWall:
				print (gravityState);
				playerVelocity.x -= gravityValue * Time.deltaTime;
				transform.rotation = Quaternion.Euler(-90, 0, 0);
				break;
			case GravityStates.SouthWall:
				print (gravityState);
				playerVelocity.z += gravityValue * Time.deltaTime;
				transform.rotation = Quaternion.Euler(-90, 0, 0);
				break;
			case GravityStates.WestWall:
				print (gravityState);
				playerVelocity.x += gravityValue * Time.deltaTime;
				transform.rotation = Quaternion.Euler(-90, 0, 0);
				break;
			case GravityStates.Ceiling:
				print (gravityState);
				playerVelocity.y -= gravityValue * Time.deltaTime;
				transform.rotation = Quaternion.Euler(-90, 0, 0);
				break;
		}
		
		rb.velocity = playerVelocity;
	}

	private void Update()
	{
		// Locking the rotation for x and z, but letting the y axis be as is.
		// rb.rotation = Quaternion.Euler(0, rb.rotation.eulerAngles.y, 0);
	}

	private void PlayerMovement()
	{
		vertical = Input.GetAxis("Vertical");
		horizontal = Input.GetAxis("Horizontal");

		playerVelocity = rb.velocity;

		float multiplier = 1f;
		if (Input.GetKey(KeyCode.LeftShift))
		{
			multiplier = 2f;
		}

		if (rb.velocity.magnitude < playerSpeed * multiplier)
		{
			if (vertical > 0.01f)
			{
				rb.AddForce(transform.forward * vertical * Time.fixedDeltaTime * 1000f);
			}
			if (vertical < -0.01f)
			{
				rb.AddForce(-transform.forward * vertical * Time.fixedDeltaTime * 1000f);
			}


			if (horizontal > 0.01f)
			{
				rb.AddForce(transform.forward * horizontal * Time.fixedDeltaTime * 1000f);
			}
			if (horizontal < -0.01f)
			{
				rb.AddForce(-transform.forward * horizontal * Time.fixedDeltaTime * 1000f);
			}
		}
		
		if (vertical == 0 || horizontal == 0)
		{
			rb.velocity = Vector3.zero;
		}
	}

	private void PlayerRotation()
	{
		Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

		if (direction.magnitude >= 0.1f)
		{
			float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
			transform.rotation = Quaternion.Euler(0f, angle, 0f);
			
			Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
		}
	}

	private void CheckForObsticleToClimb()
	{
		RaycastHit hitDown;
		RaycastHit hitForward;
		Ray rayDown = new Ray(transform.localPosition, -transform.up);
		Ray rayForward = new Ray(transform.localPosition, transform.forward);

		if (Physics.Raycast(rayDown, out hitDown, raycastRange))
		{
			groundedPlayer = true;

			if (hitDown.collider.gameObject.CompareTag("Floor"))
			{
				gravityState = GravityStates.Floor;
			}
			else if (hitDown.collider.gameObject.CompareTag("NorthWall"))
			{
				gravityState = GravityStates.NorthWall;
			}
			else if (hitDown.collider.gameObject.CompareTag("EastWall"))
			{
				gravityState = GravityStates.EastWall;
			}
			else if (hitDown.collider.gameObject.CompareTag("SouthWall"))
			{
				gravityState = GravityStates.SouthWall;
			}
			else if (hitDown.collider.gameObject.CompareTag("WestWall"))
			{
				gravityState = GravityStates.WestWall;
			}
			else if (hitDown.collider.gameObject.CompareTag("Ceiling"))
			{
				gravityState = GravityStates.Ceiling;
			}
		}
		else
		{
			groundedPlayer = false;
		}

		if (Physics.Raycast(rayForward, out hitForward, raycastRange))
		{
			// print("Found obsticle");
			Debug.DrawRay(transform.position, transform.forward, Color.red);
			// Vector3 rotateTo = new Vector3(transform.rotation.x - 1f, 0, 0);
			// print(rotateTo);
			// transform.Rotate(transform.rotation.x - 1f, 0, 0);
			if (hitForward.collider.gameObject.CompareTag("Floor"))
			{
				transform.Rotate(Vector3.Lerp(transform.rotation.eulerAngles, Vector3.zero, 1f));
			}
			else if (hitForward.collider.gameObject.CompareTag("NorthWall"))
			{
				Quaternion playerRotation = transform.rotation;
				Quaternion northwallRotation =  new Quaternion(-90, 0, 0, 0);
				Quaternion.Lerp(playerRotation, northwallRotation, 1f);
			}
			// else if (hitForward.collider.gameObject.CompareTag(""))
		}
		// else
		// {
		// 	doClimbWall = false;
		// 	// print("Found nothing...");
		// 	Debug.DrawRay(transform.position, transform.forward, Color.red);
		// }
	}
}