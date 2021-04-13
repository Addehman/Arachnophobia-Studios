using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
	[SerializeField] private CharacterController controller;
	[SerializeField] private Transform cam;
	[SerializeField] private float playerSpeed = 2.0f, turnSmoothTime = 0.1f;
	
	private Vector3 playerVelocity;
	private bool groundedPlayer;
	private float jumpHeight = 1.0f, gravityValue = -9.81f, turnSmoothVelocity;


	private void Update()
	{
		groundedPlayer = controller.isGrounded;
		if (groundedPlayer && playerVelocity.y < 0)
		{
			playerVelocity.y = 0f;
		}

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

		if (direction.magnitude >= 0.1f)
		{
			float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
			transform.rotation = Quaternion.Euler(0f, angle, 0f);
			
			Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
			controller.Move(moveDir.normalized * playerSpeed * Time.deltaTime);
		}

		// Changes the height position of the player..
		if (Input.GetButtonDown("Jump") && groundedPlayer)
		{
			playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
		}

		playerVelocity.y += gravityValue * Time.deltaTime;
		controller.Move(playerVelocity * Time.deltaTime);
	}
}