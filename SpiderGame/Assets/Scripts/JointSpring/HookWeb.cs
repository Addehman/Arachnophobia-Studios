using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookWeb : MonoBehaviour
{
	[SerializeField] private float speedMultiplier = 0.002f;
	[SerializeField] private bool rotateBool = false;

	public event Action DisableFPSCamera;
	public event Action<bool> LockTPCameraRotation;
	public Vector3 newTransformUp;

	private SpiderMovement spiderMovement;
	private ThirdPersonCameraController tpcController;
	private MimicCamera mimicCamera;
	private State currentState;
	private LineRenderer lineRenderer;
	private Vector3 oldPosition;
	private Vector3 hookShotPosition;
	private Vector3 previousTransformUp;
	private float lerpPercentage = 0f;
	private bool doDrawLine = false;

	private enum State
	{
		Normal,
		HookFlying,
	}


	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}
	
	private void Start()
	{
		spiderMovement = GetComponent<SpiderMovement>();
		tpcController = FindObjectOfType<ThirdPersonCameraController>();
		mimicCamera = FindObjectOfType<MimicCamera>();
	}

	private void Update()
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

	private void LateUpdate()
	{
		if (doDrawLine == true)
		{
			DrawLine();
		}
	}

	private void HandleHookShotStart()
	{
		if (Input.GetButtonDown("HookShotWeb"))
		{
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit raycastHit))
			{
				previousTransformUp = transform.up;
				newTransformUp = raycastHit.normal;

				hookShotPosition = raycastHit.point;

				// spiderMovement.UseHookWebNormal = true;
				rotateBool = true;

				oldPosition = transform.position;
				// lerpPercentage = 0.02f;

				spiderMovement.gravityValue = 0f;

				// spiderMovement.debugSettings.isGrounded = false;

				currentState = State.HookFlying;

				if (DisableFPSCamera != null)
				{
					DisableFPSCamera();
				}

				// doDrawLine = true;

				if (LockTPCameraRotation != null)
				{
					LockTPCameraRotation(true);
				}
			}
		}
	}

	private void HandleHookShotMovement()
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
			Invoke(nameof(SetNormalGravityAndRotation), 0.2f);
			currentState = State.Normal;
		}
	}

	private void DrawLine()
	{
		lineRenderer.SetPosition(0, oldPosition);
		lineRenderer.SetPosition(1, hookShotPosition);
		lineRenderer.enabled = true;
	}

	private void HookShotEnd()
	{
		currentState = State.Normal;
		SetNormalGravityAndRotation(); // This could be unpacked here later on.
		lineRenderer.enabled = false;
		doDrawLine = false;
		DisableClimbRotation();
		// if (CameraEndRotation != null)
		// {
		// 	CameraEndRotation();
		// }
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

	private void EnableClimbRotation()
	{
		if (LockTPCameraRotation != null)
		{
			LockTPCameraRotation(false); // bool value = do raycasts?
		}
	}

	private void DisableClimbRotation()
	{
		if (LockTPCameraRotation != null)
		{
			LockTPCameraRotation(true); // bool value = do raycasts?
		}
	}

	private void RotateToFaceTarget()
	{
		transform.forward = (hookShotPosition - transform.position).normalized;
	}
}