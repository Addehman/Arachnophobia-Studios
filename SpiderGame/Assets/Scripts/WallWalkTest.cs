using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWalkTest : MonoBehaviour
{
	public float playerSpeed;
	public float jumpStrength = 2f;
	public bool isGroundedBack;
	public bool isGroundedCenter;
	public bool hasJumped;

	private float gravityValue = -9.82f;
	private float vertical;
	private float horizontal;
	private float playerRotationY;
	private Rigidbody rb;


	void Start()
	{
		rb = GetComponent<Rigidbody>();
		RaycastDownwardsCenter(); // set a default normal to be a default normal to set the gravity to when isGrounded is false.
	}

	void Update() // Fixed?
	{
		vertical = Input.GetAxis("Vertical");
		horizontal = Input.GetAxis("Horizontal");
		
		Vector3 v = new Vector3();

		v += transform.forward * vertical;
		// v += transform.right * horizontal;
		// v += transform.up * y;

		// rb.velocity = v * Time.deltaTime;
		rb.MovePosition(transform.position + v * playerSpeed * Time.deltaTime);

		RaycastDownwardsCenter();
		RaycastDownwardsBack();
		RaycastForwards();
		PlayerRotation();
		PlayerJump();

		// if (z > 0.01f)
		// {
		// 	rb.AddForce(transform.forward * playerSpeed * Time.deltaTime * 1000);
		// }
		// else if (z < -0.01f)
		// {
		// 	rb.AddForce(-transform.forward * playerSpeed * Time.deltaTime * 1000);
		// }

		if (isGroundedBack == true || isGroundedCenter == true)
		{
			rb.AddForce(transform.up * gravityValue);
		}
		else
		{
			rb.AddForce(Vector3.up * gravityValue);
		}
	}

	private void RaycastDownwardsCenter()
	{
		Vector3 downDirCenter = transform.TransformDirection(Vector3.down);
		RaycastHit hitDownCenter;
		if (Physics.Raycast(transform.position, downDirCenter, out hitDownCenter, 1f))
		{
			isGroundedCenter = true;
		}
		else
		{
			isGroundedCenter = false;
			if (isGroundedBack == true)
			{
				transform.Rotate(45, 0, 0);
			}
		}
	}

	private void RaycastDownwardsBack()
	{
		Vector3 downDirBack = transform.TransformDirection(Vector3.down);
		Vector3 zOffset = new Vector3(0, 0, 0.45f);
		RaycastHit hitDownBack;
		if (Physics.Raycast(transform.position - zOffset, downDirBack, out hitDownBack, 1f))
		{
			Debug.DrawRay(transform.position - zOffset, downDirBack, Color.red, 0.5f);
			transform.up = hitDownBack.normal;
			isGroundedBack = true;
			hasJumped = false;
		}
		else
		{
			isGroundedBack = false;
		}
	}

	private void RaycastForwards()
	{
		Vector3 forwardDir = transform.TransformDirection(Vector3.forward);
		RaycastHit hitForward;
		if (Physics.Raycast(transform.position, forwardDir, out hitForward, 1f))
		{
			Debug.DrawRay(transform.position, forwardDir, Color.red, 0.5f);
			Vector3 rayLength = transform.position - hitForward.point;
			print ("Hit something, trying to rotate");
			transform.up = Vector3.Slerp(transform.up, hitForward.normal, 0.5f); // potentially use a coroutine to make a more smooth rotation
		}
	}

	private void PlayerRotation()
	{
		playerRotationY = transform.rotation.y + horizontal; // it is supposed to increment but only assigns it..
		transform.rotation = Quaternion.Euler(0f, playerRotationY, 0f);
	}

	private void PlayerJump()
	{
		if (Input.GetButtonDown("Jump") && isGroundedBack == true && hasJumped == false)
		{
			print ("trying to Jump!");
			rb.velocity += new Vector3(0, Mathf.Sqrt(jumpStrength * -3f * gravityValue), 0);
			hasJumped = true;
		}
	}
}
