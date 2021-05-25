using UnityEngine;
using System;

public class ToggleCameras : MonoBehaviour
{	
	[SerializeField] private Cinemachine.CinemachineVirtualCamera swingCamera;

	public event Action<bool> ActivationFPSCam;

	public GameObject crosshair;
	public bool boosted = false;
	public int PriorityBoostAmount = 20;

	private Cinemachine.CinemachineVirtualCamera aimCamera;
	private Cinemachine.CinemachineVirtualCamera hardLockCam;
	private HookWeb hookWeb;
	private ClimbWeb climbWeb;
	private SpringJointWeb springJointWeb;


	private void Awake()
	{
		aimCamera = GameObject.Find("cmAimCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
		hardLockCam = FindObjectOfType<ThirdPersonCameraController>().GetComponent<Cinemachine.CinemachineVirtualCamera>();
		hookWeb = FindObjectOfType<HookWeb>();
		hookWeb.DisableFPSCamera += DisableFPCamera;
		// hookWeb.SwitchToHardLockCam += ActivationHardLockCam;
		climbWeb = FindObjectOfType<ClimbWeb>();
		climbWeb.DisableFPSCamera += DisableFPCamera;
		springJointWeb = FindObjectOfType<SpringJointWeb>();
		springJointWeb.ExitFPCamera += DisableFPCamera;
		springJointWeb.SwitchToSwingCamera += SwitchToSwingCamera;
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
		{
			crosshair.SetActive(boosted);
		}
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

	private void SwitchToSwingCamera(bool isActive)
	{
		// Increase or Decrease the hard-lock-camera's priority.
		if (isActive == true)
		{
			swingCamera.Priority = PriorityBoostAmount;
		}
		else
		{
			swingCamera.Priority = PriorityBoostAmount - PriorityBoostAmount;
		}
	}

	private void OnDestroy()
	{
		hookWeb.DisableFPSCamera -= DisableFPCamera;
		climbWeb.DisableFPSCamera -= DisableFPCamera;
		springJointWeb.ExitFPCamera -= DisableFPCamera;
	}
}
