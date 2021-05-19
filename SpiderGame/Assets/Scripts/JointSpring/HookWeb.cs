using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookWeb : MonoBehaviour
{
	[SerializeField] Transform debugHitPointTransform;
	public GameObject webStartPosition;
	SpiderMovement spiderMovement;
	ThirdPersonCameraController tpcController;
	MimicCamera mimicCamera;
	public event Action DisableFPSCamera;
	public event Action<bool> LockTPCameraRotation;
	State currentState;

	private float lerpPercentage = 0f;
	public float speedMultiplier = 1f;
	public bool rotateBool = false;
	public Vector3 newTransformUp;

	Vector3 oldPosition;
	Vector3 hookShotPosition;
	Vector3 previousTransformUp;

	enum State
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

				debugHitPointTransform.position = raycastHit.point;
				hookShotPosition = raycastHit.point;

				// spiderMovement.UseHookWebNormal = true;
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
			}
		}
	}

	void HandleHookShotMovement()
	{
		//   Vector3 hookShotDirection = (hookShotPosition - transform.position).normalized;
		float hookShotSpeed = Vector3.Distance(oldPosition, hookShotPosition);

		lerpPercentage += Time.deltaTime / hookShotSpeed * speedMultiplier;

		if (lerpPercentage > 1f)
		{
			lerpPercentage = 1f;
		}

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

		if (lerpPercentage == 1f)
		{
			// tpcController.RecenterCamera();
			Invoke(nameof(SetNormalGravityAndRotation), 0.2f);
			currentState = State.Normal;
		}
	}

	//Tried invoking when to turn on Raycast rotation again, but it doesn't seem to help. Look further into this.
	public void SetNormalGravityAndRotation()
	{
		spiderMovement.gravityValue = -9.82f;
		spiderMovement.UseHookWebNormal = false;

		if (LockTPCameraRotation != null)
		{
			LockTPCameraRotation(false);
		}
	}
}