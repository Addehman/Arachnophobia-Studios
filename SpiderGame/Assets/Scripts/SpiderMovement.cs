using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
	public Rigidbody rb;
	public Transform cam;
	public float playerSpeed = 1f, turnSmoothTime = 0.1f;
	
	private Vector3 playerVelocity;
	private bool groundedPlayer;
	private float jumpHeight = 1.0f, gravityValue = -9.81f, turnSmoothVelocity;

	public Vector3 currentPosition;

    private void Update()
    {
		currentPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
	{
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

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

		Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

		if (direction.magnitude >= 0.1f)
		{
			float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
			transform.rotation = Quaternion.Euler(0f, angle, 0f);
			
			Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
		}

		// Changes the height position of the player..
		if (Input.GetButtonDown("Jump") && groundedPlayer)
		{
			playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
		}

		playerVelocity.y += gravityValue * Time.deltaTime;
	}
}