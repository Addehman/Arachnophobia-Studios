using UnityEngine;
using System;

public class ToggleCameras : MonoBehaviour
{
	public event Action<bool> ActivationFPSCam;

	public GameObject crosshair;
	public bool boosted = false;
	public int PriorityBoostAmount = 10;

	private Cinemachine.CinemachineVirtualCamera aimCamera;

	void Start()
	{
		aimCamera = GameObject.Find("cmAimCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
	}

	private void Update()
	{
		if (aimCamera != null)
		{
			if (Input.GetButtonDown("Aim"))
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
		aimCamera.Priority += PriorityBoostAmount;
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
		aimCamera.Priority -= PriorityBoostAmount;
		boosted = false;
		if (ActivationFPSCam != null)
		{
			ActivationFPSCam(false);
		}
	}
}
