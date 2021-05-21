using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookWeb : MonoBehaviour
{
	[HideInInspector] public bool isHookWebing = false;

	[SerializeField] private float speedMultiplier = 1f;
	[SerializeField] private bool rotateBool = false;

	public event Action DisableFPSCamera;
	public event Action<bool> LockTPCameraRotation;
	public Vector3 newTransformUp;

	private SpiderMovement spiderMovement;
	private ThirdPersonCameraController tpcController;
	private MimicCamera mimicCamera;
	private State currentState;
	private Vector3 oldPosition;
	private Vector3 hookShotPosition;
	private Vector3 previousTransformUp;
	private float lerpPercentage = 0f;

	private enum State
	{
		Normal,
		HookFlying,
	}


	void Start()
	{
		spiderMovement = GetComponent<SpiderMovement>();
		tpcController = FindObjectOfType<ThirdPersonCameraController>();
		mimicCamera = FindObjectOfType<MimicCamera>();
	}

	void Update()
	{
		if (currentState == State.Normal)
		{
			HandleHookShotStart();
		}

		if (currentState == State.HookFlying)
		{
			HandleHookShotMovement();
		}
	}

	void HandleHookShotStart()
	{
		if (Input.GetButtonDown("HookShotWeb"))
		{
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit raycastHit))
			{
				previousTransformUp = transform.up;
				newTransformUp = raycastHit.normal;

				hookShotPosition = raycastHit.point;

				rotateBool = true;

				oldPosition = transform.position;
				lerpPercentage = 0;

				currentState = State.HookFlying;

				if (DisableFPSCamera != null)
				{
					DisableFPSCamera();
				}

				if (LockTPCameraRotation != null)
				{
					LockTPCameraRotation(true);
				}

				isHookWebing = true;
			}
		}
	}

	void HandleHookShotMovement()
	{
		//   Vector3 hookShotDirection = (hookShotPosition - transform.position).normalized;
		float hookShotSpeed = Vector3.Distance(oldPosition, hookShotPosition);

		lerpPercentage += Time.deltaTime / hookShotSpeed * speedMultiplier;

		// if (lerpPercentage > 1f)
		// {
		// 	lerpPercentage = 1f;
		// }

		spiderMovement.gravityValue = 0f;

		transform.position = Vector3.Lerp(oldPosition, hookShotPosition, lerpPercentage);

		if (rotateBool)
		{
			transform.up = newTransformUp;
			rotateBool = false;
		}

		//Use below to Lerp the rotation to have a smoother transition between original rotation and 
		//transform.up = Vector3.Lerp(previousTransformUp, newTransformUp, speed);

		//Camerafix
		// tpcController.RecenterCamera();

		if (lerpPercentage >= 0.8f)
		{
			spiderMovement.UseHookWebNormal = true;
		}

		// if (lerpPercentage == 1f)
		if (Input.GetButtonDown("Jump") || lerpPercentage >= 0.99f) // this lerpPercentage check makes sure you don't travel into objects
		{
			// tpcController.RecenterCamera();
			// Invoke(nameof(SetNormalGravityAndRotation), 0.2f);
			// currentState = State.Normal;
			// isHookWebing = false;
			HookWebEnd();
		}
	}

	//Tried invoking when to turn on Raycast rotation again, but it doesn't seem to help. Look further into this.
	public void HookWebEnd()
	{
		spiderMovement.gravityValue = -9.82f;
		spiderMovement.UseHookWebNormal = false;

		if (LockTPCameraRotation != null)
		{
			LockTPCameraRotation(false);
		}

		currentState = State.Normal;
		isHookWebing = false;
	}
}