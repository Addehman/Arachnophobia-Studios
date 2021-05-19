using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookWeb : MonoBehaviour
{
	[SerializeField] private float speedMultiplier = 0.002f;
	[SerializeField] private bool rotateBool = false;

	public event Action DisableFPSCamera;
	public event Action<bool> ActivationClimbRotation;
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
				lerpPercentage = 0.02f;

				spiderMovement.gravityValue = 0f;

				spiderMovement.debugSettings.isGrounded = false;

				currentState = State.HookFlying;

				if (DisableFPSCamera != null)
				{
					DisableFPSCamera();
				}

				doDrawLine = true;
			}
		}
	}

	private void HandleHookShotMovement()
	{
		if(Input.GetButtonDown("Jump") || lerpPercentage > 0.995f || lerpPercentage < 0.005f)
		{
			HookShotEnd();
			return;
		}

		//   Vector3 hookShotDirection = (hookShotPosition - transform.position).normalized;
		float hookShotSpeed = Vector3.Distance(oldPosition, hookShotPosition);

		// Climb Controls
		float vertical = Input.GetAxis("Vertical");
		if (vertical > 0f)
		{
			lerpPercentage += speedMultiplier;
		}
		else if (vertical < 0f)
		{
			lerpPercentage -= speedMultiplier;
		}
		lerpPercentage = Mathf.Clamp(lerpPercentage, 0f, 1f);
		print (lerpPercentage);
		transform.position = Vector3.Lerp(oldPosition, hookShotPosition, lerpPercentage);
		
		if (rotateBool)
		{
			// transform.up = newTransformUp;
			EnableClimbRotation();
			rotateBool = false;
		}

		//Use below to Lerp the rotation to have a smoother transition between original rotation and 
		//transform.up = Vector3.Lerp(previousTransformUp, newTransformUp, speed);

		//Camerafix
		// tpcController.RecenterCamera();

		if (lerpPercentage >= 0.8f)
		{
			DisableClimbRotation();
			spiderMovement.UseHookWebNormal = true;
		}
		else 
		{
			RotateToFaceTarget();
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
		// DisableClimbRotation();
	}

	//Tried invoking when to turn on Raycast rotation again, but it doesn't seem to help. Look further into this.
	public void SetNormalGravityAndRotation()
	{
		spiderMovement.gravityValue = -9.82f;
		spiderMovement.UseHookWebNormal = false;
	}

	private void EnableClimbRotation()
	{
		if (ActivationClimbRotation != null)
		{
			ActivationClimbRotation(false); // bool value = do raycasts?
		}
	}

	private void DisableClimbRotation()
	{
		if (ActivationClimbRotation != null)
		{
			ActivationClimbRotation(true); // bool value = do raycasts?
		}
	}

	private void RotateToFaceTarget()
	{
		transform.forward = (hookShotPosition - transform.position).normalized;
	}
}