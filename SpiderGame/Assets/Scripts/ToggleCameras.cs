using UnityEngine;
using System;

public class ToggleCameras : MonoBehaviour
{
	public event Action<bool> ActivationFPSCam;

	// public KeyCode ActivationKey = KeyCode.LeftControl;
	public int PriorityBoostAmount = 10;
	public GameObject crosshair;
	public Cinemachine.CinemachineVirtualCamera fpsCamera;

	public bool boosted = false;

	// void Start()
	// {
	// 	fpsCamera = GetComponent<Cinemachine.CinemachineVirtualCameraBase>();
	// }

	private void Update()
	{
		if (fpsCamera != null)
		{
			if (Input.GetMouseButtonDown(1))
			{
				if (!boosted)
				{
					EnableFPSCamera();
				}
				else if (boosted)
				{
					DisableFPSCamera();
				}
			}
		}
		if (crosshair != null)
			crosshair.SetActive(boosted);
	}

	public void EnableFPSCamera()
	{
		fpsCamera.Priority += PriorityBoostAmount;
		boosted = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		if (ActivationFPSCam != null)
		{
			ActivationFPSCam(true);
		}
	}

	public void DisableFPSCamera()
	{
		fpsCamera.Priority -= PriorityBoostAmount;
		boosted = false;
		if (ActivationFPSCam != null)
		{
			ActivationFPSCam(false);
		}
	}
}
