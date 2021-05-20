using System;
using UnityEngine;

public class ClimbWeb : MonoBehaviour
{
	[HideInInspector] public Vector3 newTransformUp;

	[SerializeField] private float speedMultiplier = 0.005f;

	public event Action DisableFPSCamera;
	public event Action<bool> ActivationClimbRotation;
	public event Action CameraStartRotation;
	public event Action CameraEndRotation;

	private SpiderMovement spiderMovement;
	private ThirdPersonCameraController tpcController;
	private MimicCamera mimicCamera;
	private State currentState;
	private LineRenderer lineRenderer;
	private Vector3 oldPosition;
	private Vector3 hookShotPosition;
	private Vector3 previousTransformUp;
	private float lerpPercentage = 0f;
	private bool rotateBool = false;
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
		if (Input.GetButtonDown("HookShotWeb") && spiderMovement.debugSettings.isGrounded == true)
		{
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit raycastHit))
			{
				previousTransformUp = transform.up;
				newTransformUp = raycastHit.normal;

				hookShotPosition = raycastHit.point;

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

				if (CameraStartRotation != null)
				{
					CameraStartRotation();
				}
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
			lerpPercentage += speedMultiplier / hookShotSpeed;
		}
		else if (vertical < 0f)
		{
			lerpPercentage -= speedMultiplier / hookShotSpeed;
		}
		lerpPercentage = Mathf.Clamp(lerpPercentage, 0f, 1f);
		print (lerpPercentage);
		transform.position = Vector3.Lerp(oldPosition, hookShotPosition, lerpPercentage);
		
		if (rotateBool)
		{
			EnableClimbRotation();
			rotateBool = false;
		}

		if (lerpPercentage >= 0.8f)
		{
			DisableClimbRotation();
			spiderMovement.UseClimbWebNormal = true;
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
		spiderMovement.gravityValue = -9.82f;
		spiderMovement.UseClimbWebNormal = false;
		lineRenderer.enabled = false;
		doDrawLine = false;
		DisableClimbRotation();
		if (CameraEndRotation != null)
		{
			CameraEndRotation();
		}
	}

	private void EnableClimbRotation()
	{
		if (ActivationClimbRotation != null)
			ActivationClimbRotation(false); // bool value = do raycasts?
	}

	private void DisableClimbRotation()
	{
		if (ActivationClimbRotation != null)
			ActivationClimbRotation(true); // bool value = do raycasts?

	}

	private void RotateToFaceTarget()
	{
		transform.forward = (hookShotPosition - transform.position).normalized;
	}
}