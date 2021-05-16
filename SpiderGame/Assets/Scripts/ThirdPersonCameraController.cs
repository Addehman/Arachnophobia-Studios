using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
	[SerializeField] private float rotationSpeed = 1f;
	
	private Transform cameraParent;


	private float mouseX;
	private float mouseY;


	private void Start()
	{
		cameraParent = transform.parent;
	}
	
	void LateUpdate()
	{
		CamControl();
	}

	private void CamControl()
	{

		mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
		mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
		// Possibly Clamp mouseY

		cameraParent.localRotation = Quaternion.Euler(mouseY, mouseX, 0f);
	}
}
