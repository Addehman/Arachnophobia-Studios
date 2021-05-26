using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCameraControll : MonoBehaviour
{
	[SerializeField] private Transform targetToRotate;
	[SerializeField] private float mouseRotationSpeed = 2f;
	[SerializeField] private float gamepadRotationSpeed = 7f;

	private SpiderMovement spiderMovement;
	private float cameraInputX;


	private void Start()
	{
		spiderMovement = transform.parent.GetComponent<SpiderMovement>();
	}
	
	void Update()
	{
		if (spiderMovement.debugSettings.isFpsEnabled == false)
		{
			return;
		}

		Vector3 mouseInput = new Vector3(Input.GetAxis("Mouse X"), 0f, Input.GetAxis("Mouse Y"));
		if (mouseInput.sqrMagnitude == 0f)
		{
			cameraInputX += Input.GetAxis("CameraInputX") * gamepadRotationSpeed;
		}
		else
		{
			cameraInputX -= Input.GetAxis("CameraInputX") * mouseRotationSpeed;
		}

		// transform.parent.localRotation = Quaternion.Euler(0f, cameraInputX, 0f);
		targetToRotate.localRotation = Quaternion.Euler(0f, cameraInputX, 0f);
	}
}
