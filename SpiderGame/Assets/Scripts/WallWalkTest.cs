using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWalkTest : MonoBehaviour
{
	public float playerSpeed;

	private Rigidbody rb;

	public bool isGrounded;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		RaycastDownwards(); // set a default normal to be a default normal to set the gravity to when isGrounded is false.
	}

	// Update is called once per frame
	void Update()
	{
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");
		float y = -9.82f;

		Vector3 v = new Vector3();

		v += transform.forward * x;
		v += transform.right * z;
		v += transform.up * y;

		rb.velocity = v * Time.deltaTime;

		RaycastDownwards();
		RaycastForwards();

		if (z > 0.01f)
		{
			rb.AddForce(transform.forward * playerSpeed * Time.deltaTime * 1000);
		}
		else if (z < -0.01f)
		{
			rb.AddForce(-transform.forward * playerSpeed * Time.deltaTime * 1000);
		}

		if (isGrounded == false)
		{
			// transform.up = 
		}
	}

	private void RaycastDownwards()
	{
		Vector3 downDir = transform.TransformDirection(Vector3.down);
		RaycastHit hitDown;
		if (Physics.Raycast(transform.position, downDir, out hitDown, 1f))
		{
			transform.up = hitDown.normal;
			isGrounded = true;
		}
		else
		{
			isGrounded = false;
		}
	}

	private void RaycastForwards()
	{
		Vector3 forwardDir = transform.TransformDirection(Vector3.forward);
		RaycastHit hitForward;
		if (Physics.Raycast(transform.position, forwardDir, out hitForward, 5f))
		{
			Debug.DrawRay(transform.position, forwardDir, Color.red, 0.5f);
			Vector3 rayLength = transform.position - hitForward.point;
			print ("Hit something, trying to rotate");
			transform.eulerAngles = new Vector3(transform.eulerAngles.x * rayLength.magnitude, 0, 0); // think this isn't giving the correct values? does not rotate anyway
		}
	}
}
