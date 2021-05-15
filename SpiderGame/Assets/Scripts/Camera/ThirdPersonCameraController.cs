using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
	[SerializeField] private float rotationSpeed = 1f;
	[SerializeField] private Transform cameraTarget;
	[SerializeField] private Transform targetToRotate;
	[SerializeField] private Transform getUpFrom;

	public bool allowInput = true;

	private float mouseX;
	private float mouseY;


	void LateUpdate()
	{
		CamControl();
	}

	private void CamControl()
	{
		if (allowInput == true)
		{
			mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
			mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
		}

		// float mouseXDelta = Input.GetAxis("Mouse X") * rotationSpeed;
		// float mouseYDelta = -Input.GetAxis("Mouse Y") * rotationSpeed;
		// mouseY = Mathf.Clamp(mouseY, -35, 60);

		// transform.LookAt(cameraTarget.position, cameraTarget.parent.up); // might be needed, leave in case of fire!

		// print($"cameraTarget: {cameraTarget.up} \ngetUpFrom: {getUpFrom.up}");

		// cameraTarget.localRotation = Quaternion.LookRotation(cameraTarget.forward, getUpFrom.up);
		// cameraTarget.up = getUpFrom.up;
		// cameraTarget.Rotate(mouseYDelta, mouseXDelta, 0f);
		cameraTarget.localRotation = Quaternion.Euler(mouseY, mouseX, 0f);
		targetToRotate.localRotation = Quaternion.Euler(0f, mouseX, 0f);
	}
}
