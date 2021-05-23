using System;
using UnityEngine;

public class ClimbWeb : MonoBehaviour
{
	[HideInInspector] public Vector3 newTransformUp;
	[HideInInspector] public bool isClimbWebing = false;

	[SerializeField] private float speedMultiplier = 0.005f;

	public event Action DisableFPSCamera;
	public event Action<bool> ActivationClimbRotation;
	public event Action CameraStartRotation;
	public event Action CameraEndRotation;

	private Transform parentObject;
	private SpiderMovement spiderMovement;
	private ThirdPersonCameraController tpcController;
	private MimicCamera mimicCamera;
	private ToggleCameras toggleCameras;
	private WebSelector webSelector;
	private LineRenderer lineRenderer;
	private State currentState;
	private Vector3 oldPosition;
	private Vector3 climbShotPosition;
	private Vector3 previousTransformUp;
	private float lerpPercentage = 0f;
	private bool rotateBool = false;
	private bool doDrawLine = false;
	private bool hasPressed = false;

	private enum State {Normal, Climbing,}


	private void Awake()
	{
		lineRenderer = FindObjectOfType<LineRenderer>();
	}

	private void Start()
	{
		parentObject = transform.parent;
		spiderMovement = GetComponent<SpiderMovement>();
		tpcController = FindObjectOfType<ThirdPersonCameraController>();
		mimicCamera = FindObjectOfType<MimicCamera>();
		toggleCameras = Camera.main.GetComponent<ToggleCameras>();
		webSelector = FindObjectOfType<WebSelector>();
	}

	private void Update()
	{
		if (webSelector.webState == WebAbilityState.Climb)
		{
			if (toggleCameras.boosted == true)
			{
				if (currentState == State.Normal)
				{
					InitiateClimbWeb();
				}
			}

			if (currentState == State.Climbing)
			{
				ClimbWebMovement();
			}
		}
	}

	private void LateUpdate()
	{
		if (doDrawLine == true)
		{
			DrawLine();
		}
	}

	private void OnDisable()
	{
		ClimbWebEnd();
	}

	private void InitiateClimbWeb()
	{
		if ((Input.GetButtonDown("UseWeb") || Input.GetAxis("UseWeb") > 0f && hasPressed == false) && spiderMovement.debugSettings.isGrounded == true)
		{
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit raycastHit))
			{
				previousTransformUp = transform.up;
				newTransformUp = raycastHit.normal;

				climbShotPosition = raycastHit.point;

				rotateBool = true;

				oldPosition = parentObject.transform.position;
				lerpPercentage = 0.02f;

				spiderMovement.gravityValue = 0f;

				spiderMovement.debugSettings.isGrounded = false;

				currentState = State.Climbing;

				if (DisableFPSCamera != null)
				{
					DisableFPSCamera();
				}

				doDrawLine = true;

				if (CameraStartRotation != null)
				{
					CameraStartRotation();
				}

				isClimbWebing = true;
			}
			hasPressed = true;
		}
		
		if ((Input.GetButtonUp("UseWeb") || Input.GetAxis("UseWeb") <= 0f) && hasPressed == true)
		{
			hasPressed = false;
		}
	}

	private void ClimbWebMovement()
	{
		if (Input.GetButtonDown("Jump") || lerpPercentage > 0.995f || lerpPercentage < 0.005f)
		{
			ClimbWebEnd();
			return;
		}

		float climbSpeed = Vector3.Distance(oldPosition, climbShotPosition);

		// Climb Controls
		float vertical = Input.GetAxis("Vertical");
		if (vertical > 0f)
		{
			lerpPercentage += speedMultiplier / climbSpeed;
		}
		else if (vertical < 0f)
		{
			lerpPercentage -= speedMultiplier / climbSpeed;
		}
		lerpPercentage = Mathf.Clamp(lerpPercentage, 0f, 1f);
		print(lerpPercentage);
		parentObject.transform.position = Vector3.Lerp(oldPosition, climbShotPosition, lerpPercentage);

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
		lineRenderer.SetPosition(1, climbShotPosition);
		lineRenderer.enabled = true;
	}

	private void ClimbWebEnd()
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
		isClimbWebing = false;
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
		transform.forward = (climbShotPosition - parentObject.transform.position).normalized;
	}
}