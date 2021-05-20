using UnityEngine;
using System;

public class ToggleCameras : MonoBehaviour
{	
	public event Action<bool> ActivationFPSCam;

	public GameObject crosshair;
	public bool boosted = false;
	public int PriorityBoostAmount = 20;

	private Cinemachine.CinemachineVirtualCamera aimCamera;
	private HookWeb hookWeb;

	void Start()
	{
		aimCamera = GameObject.Find("cmAimCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
		hookWeb = FindObjectOfType<HookWeb>();
		hookWeb.DisableFPSCamera += DisableFPSCamera;
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
		aimCamera.Priority = PriorityBoostAmount;
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
		aimCamera.Priority = PriorityBoostAmount - PriorityBoostAmount;
		boosted = false;
		if (ActivationFPSCam != null)
		{
			ActivationFPSCam(false);
		}
	}
}
