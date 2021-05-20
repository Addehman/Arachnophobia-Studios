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
	private ClimbWeb climbWeb;
	private SpringJointWeb springJointWeb;

	void Start()
	{
		aimCamera = GameObject.Find("cmAimCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
		hookWeb = FindObjectOfType<HookWeb>();
		hookWeb.DisableFPSCamera += DisableFPCamera;
		climbWeb = FindObjectOfType<ClimbWeb>();
		climbWeb.DisableFPSCamera += DisableFPCamera;
		springJointWeb = FindObjectOfType<SpringJointWeb>();
		springJointWeb.ExitFPCamera += DisableFPCamera;
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
					DisableFPCamera();
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

	public void DisableFPCamera()
	{
		aimCamera.Priority = PriorityBoostAmount - PriorityBoostAmount;
		boosted = false;
		if (ActivationFPSCam != null)
		{
			ActivationFPSCam(false);
		}
	}
}
