using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
	public BoxCollider frontTrigger;
	public Rigidbody rb;
	public Transform cam;
	public float playerSpeed = 1f, turnSmoothTime = 0.1f;
	public float raycastRange = 1f;
	public Vector3 playerVelocity;
	
	private bool groundedPlayer;
	private bool doClimbWall = false;
	private float jumpHeight = 1.0f, gravityValue = -9.81f, turnSmoothVelocity;
	private float horizontal;
	private float vertical;


	private void FixedUpdate()
	{
		PlayerMovement();

		PlayerRotation();

		CheckForObsticleToClimb();

		// Changes the height position of the player..
		if (Input.GetButtonDown("Jump") && groundedPlayer)
		{
			playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
		}

		if (doClimbWall == true)
		{
			playerVelocity.x += gravityValue * Time.deltaTime;
		}
		else
		{
			playerVelocity.y += gravityValue * Time.deltaTime;
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
			transform.rotation = Quaternion.Euler(transform.rotation.x, angle, 0f);
			
			Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
		}
	}

	private void CheckForObsticleToClimb()
	{
		RaycastHit hit;
		Ray ray = new Ray(transform.localPosition, transform.forward);

		if (Physics.Raycast(ray, out hit, raycastRange) && vertical > 0)
		{
			doClimbWall = true;
			// print("Found obsticle");
			Debug.DrawRay(transform.position, transform.forward, Color.red);
			// Vector3 rotateTo = new Vector3(transform.rotation.x - 1f, 0, 0);
			// print(rotateTo);
			// transform.Rotate(transform.rotation.x - 1f, 0, 0);
			transform.Rotate(-90, 0, 0);
		}
		// else
		// {
		// 	doClimbWall = false;
		// 	// print("Found nothing...");
		// 	Debug.DrawRay(transform.position, transform.forward, Color.red);
		// }
	}
}