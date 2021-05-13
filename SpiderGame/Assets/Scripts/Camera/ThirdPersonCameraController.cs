using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
	[SerializeField] private float rotationSpeed = 1f;
	[SerializeField] private Transform cameraTarget;
	[SerializeField] private Transform lookAtTarget;

	private float mouseX;
	private float mouseY;


	void LateUpdate()
	{
		CamControl();
	}

	private void CamControl()
	{
		mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
		mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
		// mouseY = Mathf.Clamp(mouseY, -35, 60);

		// transform.LookAt(cameraTarget.position, cameraTarget.parent.up); // might be needed, leave in case of fire!

		cameraTarget.localRotation = Quaternion.Euler(mouseY, mouseX, 0f);
		lookAtTarget.localRotation = Quaternion.Euler(0f, mouseX, 0f);
	}
}
