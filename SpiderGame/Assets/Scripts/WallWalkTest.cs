using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWalkTest : MonoBehaviour
{
	public float playerSpeed;
	public bool isGroundedBack;
	public bool isGroundedCenter;


	private Rigidbody rb;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		RaycastDownwardsCenter(); // set a default normal to be a default normal to set the gravity to when isGrounded is false.
	}

	// Update is called once per frame
	void Update()
	{
		float x = Input.GetAxis("Vertical");
		float z = Input.GetAxis("Horizontal");
		float y = -9.82f;

		Vector3 v = new Vector3();

		v += transform.forward * x;
		v += transform.right * z;
		// v += transform.up * y;

		// rb.velocity = v * Time.deltaTime;
		rb.MovePosition(transform.position + v * playerSpeed * Time.deltaTime);

		

		RaycastDownwardsCenter();
		RaycastForwards();

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
			rb.AddForce(transform.up * y);
		}
		else
		{
			rb.AddForce(Vector3.up * y);
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
			transform.up = hitDownBack.normal;
			isGroundedBack = true;
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
}
