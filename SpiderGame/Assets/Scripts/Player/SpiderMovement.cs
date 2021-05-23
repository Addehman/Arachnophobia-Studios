using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using System;

[System.Serializable]
public class MainRaycastsAdjustment
{
	// [Header("Main Raycasts Adjustment")]
	public float rayFwdMod	= 1f;
	public float rayBwdMod	= 1f;
	public float raysBackOriginOffset = 0f;
	public float rayDownMod	= 1f;
	public float rayDownOriginOffset = 0f;
}

[System.Serializable]
public class ForwardsRaycastsAdjustment
{
	// [Header("Forwards-Raycasts Adjustment")]
	public float rayFwdMod1 	= 0.025f;
	public float rayFwdModDown1 = 0.1f;
	public float rayFwdMod2 	= 0.05f;
	public float rayFwdModDown2	= 0.1f;
	public float rayFwdMod3		= 0.075f;
	public float rayFwdModDown3	= 0.1f;
	public float rayFwdMod4		= 0.11f;
	public float rayFwdModDown4	= 0.1f;
	public float rayFwdMod5		= 0.1f;
	public float rayFwdModDown5	= 0.063f;
	public float rayFwdMod6		= 0.1f;
	public float rayFwdModDown6	= 0.038f;
}

[System.Serializable]
public class BackwardsRaycastsAdjustment
{
	// [Header("Backwards-Raycasts Adjustment")]
	public float rayBwdMod1		= 0.025f;
	public float rayBwdModDown1	= 0.1f;
	public float rayBwdMod2		= 0.05f;
	public float rayBwdModDown2	= 0.1f;
	public float rayBwdMod3		= 0.075f;
	public float rayBwdModDown3	= 0.1f;
	public float rayBwdMod4		= 0.11f;
	public float rayBwdModDown4	= 0.1f;
	public float rayBwdMod5		= 0.1f;
	public float rayBwdModDown5	= 0.063f;
	public float rayBwdMod6		= 0.1f;
	public float rayBwdModDown6	= 0.038f;
}

[System.Serializable]
public class RaycastGeneralSettings
{
	// [Header("Raycast General Settings")]
	public float raycastReach = 0.05f;
	public float increasedRaycastReach = 0.1f;
	public float defaultRaycastReach = 0.05f;
	public float raycastReachEdge = 0.1f;
	public float edgeRayOriginOffset = 0.03f;
	public float edgeRayOriginOffset1 = 0.04f;
	public int fwdRaycastWeightMultiplier = 9;
	public int backRaycastWeightMultiplier = 9;
	public int edgeRaycastWeightMultiplier = 3;
	public LayerMask layerMask = new LayerMask();
}

[System.Serializable]
public class PlayerSettings
{
	// [Header("Player Settings")]
	public float normalPlayerSpeed = 0.2f;
	public float normalSlowPlayerSpeed = 0.1f;
	public float defaultNormalPlayerSpeed = 0.2f;
	public float normalSprintMultiAmount = 0.2f;
	public float velocityPlayerSpeed = 30f;
	public float velocityNormalPlayerSpeed = 30f;
	public float velocitySprintMultiAmount = 60f;
	public float jumpUpStrength = 100f;
	public float jumpFwdStrength = 50f;
	public float playerToGroundRange = 0.3f;
	public float turnSmoothTime = 0.1f;
	public float turnSpeed = 180f;
	public float moveToSpeed = 1f;
}

[System.Serializable]
public class DebugSettings
{
	// [Header("Debug")]
	public Vector3 mainDownRayNormalDirection;
	public Vector3 averageNormalDirection;
	public List<Vector3> averageNormalDirections = new List<Vector3>();
	public Vector3 fwdRayHitNormalDebug;
	public bool isGrounded;
	public bool doDrawRayGizmos = true;
	public bool fwdRayCheckNoHit = false;
	public bool fwdRayCheckNoHit2 = false;
	public bool backRayNoHit = false;
	public bool isFwdRayHitting;
	public bool isFpsEnabled = false;
	public bool isPlayerBeingVacuumed;
	public bool doRaycasts = true;
	public bool doForwardCheckRay = true;
	public float forwardRotationSpeed = 1f;
}

public class SpiderMovement : MonoBehaviour
{
	[HideInInspector] public Rigidbody rb;
	[HideInInspector] public Vector3 currentPosition;
	[HideInInspector] public float gravityValue = -9.82f;
	[HideInInspector] public bool UseHookWebNormal = false;
	[HideInInspector] public bool UseClimbWebNormal = false;

	[SerializeField] private Transform movementParent;
	[SerializeField] private GameObject cmTPCamera;
	[SerializeField] private GameObject cameraParent;
	[SerializeField] private GameObject spiderModel;

	public event Action<bool> cameraChangeStrategy;

	public MainRaycastsAdjustment mainRaycastAdjustments;
	public ForwardsRaycastsAdjustment forwardsRaycastAdjustment;
	public BackwardsRaycastsAdjustment backwardsRaycastAdjustment;
	public RaycastGeneralSettings raycastGeneralSettings;
	public PlayerSettings playerSettings;
	public DebugSettings debugSettings;

	private enum RaycastTypes {MainForwards, MainBackwards, MainDown, Forwards, Backwards, Downwards, Any, ForwardsEdgeCheck, ForwardsEdgeCheck2}
	private GameObject parentObject;
	private GameObject[] modelChildren;
	private Animator spiderAnimator;
	private RaycastTypes raycastType;
	private VacuumBlackhole vacuumBlackhole;
	private SpringJointWeb springJointWeb;
	private GameObject targetRotationObject;
	private Transform cam;
	private Transform lookAtTarget;
	private HookWeb hookWeb;
	private ClimbWeb climbWeb;
	private RotationConstraint cameraRotationConstraint;
	private Vector3 myNormal;
	private float vertical;
	private float horizontal;
	private float turnSmoothVelocity;
	private float sprintMulti;
	private int randomIdle;
	private float randomIdleTimer = 0f;
	private float rotationSlerpSpeed = 10f;


	void Start()
	{
		parentObject = transform.parent.gameObject;
		rb = parentObject.GetComponent<Rigidbody>();
		cam = Camera.main.transform;
		cam.GetComponent<ToggleCameras>().ActivationFPSCam += activateOnKeypress_ActivationFPSCam;
		spiderAnimator = GetComponent<Animator>();
		spiderAnimator.SetTrigger("Idle");
		vacuumBlackhole = FindObjectOfType<VacuumBlackhole>();
		vacuumBlackhole.PullingPlayer += vacuumBlackhole_PullingPlayer;

		springJointWeb = parentObject.GetComponent<SpringJointWeb>();
		hookWeb = GetComponent<HookWeb>();
		climbWeb = GetComponent<ClimbWeb>();
		climbWeb.ActivationClimbRotation += ActivationOfRaycasts;

		lookAtTarget = FindObjectOfType<LookAtTargetController>().transform;
		cameraRotationConstraint = cameraParent.GetComponent<RotationConstraint>();

		// Here the reference is made for all the children of the spidermodel, used to be able to hide/show when in fpCamera-mode.
		int amountOfModelParts = 0;
		foreach (Transform item in spiderModel.transform)
		{
			amountOfModelParts ++;
		}
		modelChildren = new GameObject[amountOfModelParts - 1];
		for (int i = 0; i < modelChildren.Length; i++)
		{
			modelChildren[i] = spiderModel.transform.GetChild(i).gameObject;
		}

		raycastGeneralSettings.raycastReach = raycastGeneralSettings.defaultRaycastReach;
		playerSettings.normalPlayerSpeed = playerSettings.defaultNormalPlayerSpeed;
	}

	private void ActivationOfRaycasts(bool isActive)
	{
		debugSettings.doRaycasts = isActive;
		debugSettings.averageNormalDirections.Clear();
	}

	private void activateOnKeypress_ActivationFPSCam(bool isActive)
	{
		debugSettings.isFpsEnabled = isActive;
		bool visibilityActivation = !isActive;
		SetVisibilityOfModel(visibilityActivation);
	}

	private void vacuumBlackhole_PullingPlayer(bool isPlayerPulled)
	{
		debugSettings.isPlayerBeingVacuumed = isPlayerPulled;
	}

	// Update is called once per frame
	void Update()
	{
		debugSettings.averageNormalDirections.Clear();

		currentPosition = transform.position;

		// cam.Rotate(transform.up, Space.Self);


		vertical = Input.GetAxisRaw("Vertical");
		horizontal = Input.GetAxisRaw("Horizontal");

		RaycastsToCast();
		// SetPlayerUpDirection();
		Sprint();
		SpiderJump();
		SpiderAnimation();
	}

	private void FixedUpdate()
	{
		// apply constant weight force according to character normal:
		if (debugSettings.isGrounded == true)
		{
			rb.AddForce(gravityValue * rb.mass * transform.up);
		}
		else
		{
			rb.AddForce(gravityValue * rb.mass * Vector3.up);
		}

		if (debugSettings.isPlayerBeingVacuumed == true)
		{
			VacuumPullingMovement();
		}
		else
		{
			// CameraDirectionMovement();

			SetLookDirection();
			SetPlayerUpDirection();
			TranslateMovement();
			// DefaultMovement();
			// RigidbodyMovement();
		}
	}

	private void RaycastsToCast()
	{
		if (debugSettings.doRaycasts == false)
		{
			return;
		}

		// Consider making these into For-Loops instead!
		RaycastHelper(transform.TransformDirection(Vector3.forward) * mainRaycastAdjustments.rayFwdMod, 0f, RaycastTypes.MainForwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * mainRaycastAdjustments.rayBwdMod, 0f, RaycastTypes.MainBackwards);
		RaycastHelper(transform.TransformDirection(Vector3.down) * mainRaycastAdjustments.rayDownMod, mainRaycastAdjustments.rayDownOriginOffset, RaycastTypes.MainDown);

		RaycastHelper(transform.TransformDirection(Vector3.right) + transform.TransformDirection(Vector3.down), 0f, RaycastTypes.Any);
		RaycastHelper(transform.TransformDirection(Vector3.left) + transform.TransformDirection(Vector3.down), 0f, RaycastTypes.Any);

		RaycastHelper(transform.TransformDirection(Vector3.forward) * forwardsRaycastAdjustment.rayFwdMod1 + transform.TransformDirection(Vector3.down) * forwardsRaycastAdjustment.rayFwdModDown1, 0f, RaycastTypes.ForwardsEdgeCheck);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * forwardsRaycastAdjustment.rayFwdMod2 + transform.TransformDirection(Vector3.down) * forwardsRaycastAdjustment.rayFwdModDown2, 0f, RaycastTypes.Forwards);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * forwardsRaycastAdjustment.rayFwdMod3 + transform.TransformDirection(Vector3.down) * forwardsRaycastAdjustment.rayFwdModDown3, 0f, RaycastTypes.Forwards);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * forwardsRaycastAdjustment.rayFwdMod4 + transform.TransformDirection(Vector3.down) * forwardsRaycastAdjustment.rayFwdModDown4, 0f, RaycastTypes.Forwards);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * forwardsRaycastAdjustment.rayFwdMod5 + transform.TransformDirection(Vector3.down) * forwardsRaycastAdjustment.rayFwdModDown5, 0f, RaycastTypes.ForwardsEdgeCheck2);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * forwardsRaycastAdjustment.rayFwdMod6 + transform.TransformDirection(Vector3.down) * forwardsRaycastAdjustment.rayFwdModDown6, 0f, RaycastTypes.Forwards);

		RaycastHelper(transform.TransformDirection(Vector3.back) * backwardsRaycastAdjustment.rayBwdMod1 + transform.TransformDirection(Vector3.down) * backwardsRaycastAdjustment.rayBwdModDown1, mainRaycastAdjustments.raysBackOriginOffset, RaycastTypes.Backwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * backwardsRaycastAdjustment.rayBwdMod2 + transform.TransformDirection(Vector3.down) * backwardsRaycastAdjustment.rayBwdModDown2, mainRaycastAdjustments.raysBackOriginOffset, RaycastTypes.Backwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * backwardsRaycastAdjustment.rayBwdMod3 + transform.TransformDirection(Vector3.down) * backwardsRaycastAdjustment.rayBwdModDown3, mainRaycastAdjustments.raysBackOriginOffset, RaycastTypes.Backwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * backwardsRaycastAdjustment.rayBwdMod4 + transform.TransformDirection(Vector3.down) * backwardsRaycastAdjustment.rayBwdModDown4, mainRaycastAdjustments.raysBackOriginOffset, RaycastTypes.Backwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * backwardsRaycastAdjustment.rayBwdMod5 + transform.TransformDirection(Vector3.down) * backwardsRaycastAdjustment.rayBwdModDown5, mainRaycastAdjustments.raysBackOriginOffset, RaycastTypes.Backwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * backwardsRaycastAdjustment.rayBwdMod6 + transform.TransformDirection(Vector3.down) * backwardsRaycastAdjustment.rayBwdModDown6, mainRaycastAdjustments.raysBackOriginOffset, RaycastTypes.Backwards);
		
		// Edge Raycasts:
		Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
		if (debugSettings.fwdRayCheckNoHit == true && movementInput.sqrMagnitude > 0f)
		{
			EdgeRaycastHelper(transform.TransformDirection(Vector3.back) + transform.TransformDirection(Vector3.down), raycastGeneralSettings.edgeRayOriginOffset);
			EdgeRaycastHelper(transform.TransformDirection(Vector3.back) + transform.TransformDirection(Vector3.down), raycastGeneralSettings.edgeRayOriginOffset1);
		}

		if (debugSettings.fwdRayCheckNoHit2 == true && movementInput.sqrMagnitude > 0f)
		{
			playerSettings.normalPlayerSpeed = playerSettings.normalSlowPlayerSpeed;
			raycastGeneralSettings.raycastReach = raycastGeneralSettings.increasedRaycastReach;
		}
		else 
		{
			playerSettings.normalPlayerSpeed = playerSettings.defaultNormalPlayerSpeed;
			raycastGeneralSettings.raycastReach = raycastGeneralSettings.defaultRaycastReach;
		}

		if (debugSettings.doForwardCheckRay == true)
		{
			if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), 0.3f))
			{
				if (cameraChangeStrategy != null)
				{
					cameraChangeStrategy(true);
				}
			}
			else
			{
				if (cameraChangeStrategy != null)
				{
					cameraChangeStrategy(false);
				}
			}
		}
	}
	// Special "Hook"- or Edge-Raycasts, used to look over edges to find footing where the other rays won't reach.
	private void EdgeRaycastHelper(Vector3 direction, float originOffsetValue)
	{
		RaycastHit hit;
		Vector3 originOffset = transform.TransformDirection(Vector3.back) * originOffsetValue;
		if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastGeneralSettings.raycastReachEdge, raycastGeneralSettings.layerMask))
		{
			if (debugSettings.doDrawRayGizmos == true)
			{
				Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastGeneralSettings.raycastReachEdge);
			}
			RaycastWeightMulti(debugSettings.averageNormalDirections, raycastGeneralSettings.edgeRaycastWeightMultiplier, hit.normal);
		}
	}
	// Main Raycasts function, that casts the rays that balances the player's rotation according to all the normals that these rays find.
	private void RaycastHelper(Vector3 direction, float originOffsetValue, RaycastTypes inRaycastType)
	{
		Vector3 originOffset = transform.TransformDirection(Vector3.back) * originOffsetValue;
		RaycastHit hit;
		
		switch (inRaycastType)
		{
			case RaycastTypes.MainForwards:
				Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
				if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastGeneralSettings.raycastReach, raycastGeneralSettings.layerMask) && movementInput.sqrMagnitude > 0f)
				{
					if (debugSettings.doDrawRayGizmos == true)
					{
						Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastGeneralSettings.raycastReach);
					}

					// it seems that when adding more of the same on the same raycast will give it more weight, thus we might be able to remove some of the raycasts! 
					//Specifically those forward might be possible to cut away.
					RaycastWeightMulti(debugSettings.averageNormalDirections, raycastGeneralSettings.fwdRaycastWeightMultiplier, hit.normal);
				
					debugSettings.fwdRayHitNormalDebug = hit.normal;
				}
				break;
			case RaycastTypes.MainDown:
				if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastGeneralSettings.raycastReach, raycastGeneralSettings.layerMask))
				{
					if (debugSettings.doDrawRayGizmos == true)
					{
						Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastGeneralSettings.raycastReach);
					}
					debugSettings.averageNormalDirections.Add(hit.normal);
					// debugSettings.mainDownRayNormalDirection = hit.normal;

					float rbVelocity = rb.velocity.y;
					if (hit.distance < playerSettings.playerToGroundRange && rbVelocity < 0f)
					{
						debugSettings.isGrounded = true;
						spiderAnimator.SetBool("Jump", false);
					}
				}
				else
				{
					debugSettings.isGrounded = false;
					// debugSettings.mainDownRayNormalDirection = Vector3.zero;
				}
				break;
			case RaycastTypes.MainBackwards:
				if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastGeneralSettings.raycastReach, raycastGeneralSettings.layerMask))
				{
					if (debugSettings.doDrawRayGizmos == true)
					{
						Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastGeneralSettings.raycastReach);
					}
					RaycastWeightMulti(debugSettings.averageNormalDirections, raycastGeneralSettings.backRaycastWeightMultiplier, hit.normal);
				}
				break;
			case RaycastTypes.ForwardsEdgeCheck:
				if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastGeneralSettings.raycastReach, raycastGeneralSettings.layerMask))
				{
					if (debugSettings.doDrawRayGizmos == true)
					{
						Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastGeneralSettings.raycastReach);
					}
					debugSettings.averageNormalDirections.Add(hit.normal);
					debugSettings.fwdRayCheckNoHit = false;
				}
				else
				{
					debugSettings.fwdRayCheckNoHit = true;
				}
				break;
			case RaycastTypes.ForwardsEdgeCheck2:
				if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastGeneralSettings.raycastReach, raycastGeneralSettings.layerMask))
				{
					if (debugSettings.doDrawRayGizmos == true)
					{
						Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastGeneralSettings.raycastReach);
					}
					debugSettings.averageNormalDirections.Add(hit.normal);
					debugSettings.fwdRayCheckNoHit2 = false;
				}
				else
				{
					debugSettings.fwdRayCheckNoHit2 = true;
				}
				break;
			case RaycastTypes.Forwards:
				if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastGeneralSettings.raycastReach, raycastGeneralSettings.layerMask))
				{
					if (debugSettings.doDrawRayGizmos == true)
					{
						Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastGeneralSettings.raycastReach);
					}
					debugSettings.averageNormalDirections.Add(hit.normal);
				}
				break;
			case RaycastTypes.Backwards:
				if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastGeneralSettings.raycastReach))
				{
					if (debugSettings.doDrawRayGizmos == true)
					{
						Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastGeneralSettings.raycastReach);
					}
					debugSettings.averageNormalDirections.Add(hit.normal);
					debugSettings.backRayNoHit = false;
				}
				else
				{
					debugSettings.backRayNoHit = true;
				}
				break;
			default:
				if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastGeneralSettings.raycastReach, raycastGeneralSettings.layerMask))
				{
					if (debugSettings.doDrawRayGizmos == true)
					{
						Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastGeneralSettings.raycastReach);
					}
					debugSettings.averageNormalDirections.Add(hit.normal);
				}
				break;
		}
	}

	private void RaycastWeightMulti(List<Vector3> listToAddTo, int amountToMultiply, Vector3 normalToAdd)
	{
		for (int i = 0; i < amountToMultiply; i++)
		{
			listToAddTo.Add(normalToAdd);
		}
	}

	private void SetPlayerUpDirection()
	{
		if (debugSettings.doRaycasts == true)
		{
			for (int i = 0; i < debugSettings.averageNormalDirections.Count; i++)
			{
				debugSettings.averageNormalDirection += debugSettings.averageNormalDirections[i];
			}
			
			debugSettings.averageNormalDirection /= debugSettings.averageNormalDirections.Count;

			if (debugSettings.averageNormalDirections.Count == 0)
			{
				debugSettings.averageNormalDirection = Vector3.up;
			}

			if (UseHookWebNormal)
			{
				debugSettings.averageNormalDirection = hookWeb.newTransformUp;
			}
			else if (UseClimbWebNormal)
			{
				debugSettings.averageNormalDirection = climbWeb.newTransformUp;
			}

			float lerpSpeed = 10f;

			myNormal = Vector3.Slerp(myNormal, debugSettings.averageNormalDirection, lerpSpeed * Time.deltaTime);
			// find forward direction with new myNormal:
			Vector3 myForward = Vector3.Cross(movementParent.transform.right, myNormal);
			// align character to the new myNormal while keeping the forward direction:
			Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
			movementParent.transform.rotation = Quaternion.Slerp(movementParent.transform.rotation, targetRot, lerpSpeed * Time.deltaTime);
		}
	}

	private void TranslateMovement()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		Vector3 movement = new Vector3(vertical, 0f, horizontal);
		if (movement.sqrMagnitude > 0f)
		{
			parentObject.transform.Translate(transform.forward * (playerSettings.normalPlayerSpeed + sprintMulti) * Time.deltaTime);
		}	
	}

	private void VacuumPullingMovement() // The Rigidbody-based movement used only for when the player is in range of being pulled with rb.AddForce towards the Robot Vacuum Cleaner - becomes most smooth this way.
	{
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

		rb.velocity = (transform.forward * vertical) * (playerSettings.velocityPlayerSpeed + sprintMulti) * Time.deltaTime;
	}

	private void SetLookDirection()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		
		if (Vector3.Distance(transform.position, lookAtTarget.position) >= 0.05f)
		{
			transform.LookAt(lookAtTarget, lookAtTarget.up);
		}
	}

	private void SpiderJump()
	{
		Vector3 movementInput = new Vector3(horizontal, 0f, vertical);
		// Jump Straight Up
		if (movementInput.sqrMagnitude <= 0f && Input.GetButtonDown("Jump") && debugSettings.isGrounded == true && StaminaBar.staminaBarInstance.currentStamina >= 0.1f && PauseMenu.isPaused == false)
		{
			spiderAnimator.SetBool("Jump", true);
			rb.AddForce(transform.up * playerSettings.jumpUpStrength);
			debugSettings.isGrounded = false;
			StaminaBar.staminaBarInstance.UseStamina(0.1f);
		}
		// Jump Forwards
		else if (movementInput.sqrMagnitude > 0f && Input.GetButtonDown("Jump") && debugSettings.isGrounded == true && StaminaBar.staminaBarInstance.currentStamina >= 0.1f && PauseMenu.isPaused == false)
		{
			spiderAnimator.SetBool("Jump", true);
			rb.AddForce((transform.up + transform.forward) * playerSettings.jumpFwdStrength);
			debugSettings.isGrounded = false;
			StaminaBar.staminaBarInstance.UseStamina(0.1f);
		}
	}
	// Binds key for player to use to increase move speed.
	private void Sprint()
	{
		Vector3 movementInput = new Vector3(horizontal, 0f, vertical);
		if ((Input.GetButton("Sprint") && movementInput.sqrMagnitude > 0f || Input.GetAxis("Sprint") < 0f) && StaminaBar.staminaBarInstance.currentStamina >= 0.0035f 
			&& PauseMenu.isPaused == false && IsUsingWeb() == false)
		{
			spiderAnimator.speed = 3f;
			if (debugSettings.isPlayerBeingVacuumed == true)
			{
				sprintMulti = playerSettings.velocitySprintMultiAmount;
			}
			else
			{
				sprintMulti = playerSettings.normalSprintMultiAmount;
			}

			StaminaBar.staminaBarInstance.UseStamina(0.0035f);
		}

		if (Input.GetButtonUp("Sprint") || movementInput.sqrMagnitude <= 0f || StaminaBar.staminaBarInstance.currentStamina < 0.0050f || Input.GetAxis("Sprint") >= 0f 
			&& Input.GetButton("Sprint") == false && IsUsingWeb() == false)
		{
			sprintMulti = 0f;
			spiderAnimator.speed = 1f;
		}
	}

	private void SpiderAnimation()
	{
		if (springJointWeb.isSwingingWeb == true)
		{
			spiderAnimator.SetBool("Web", true);
		}

		else if (springJointWeb.isSwingingWeb == false)
		{
			spiderAnimator.SetBool("Web", false);
		}

		float movementInput = Mathf.Abs(vertical + horizontal);
		if (movementInput > 0.01f && spiderAnimator.GetBool("Walk") == false && debugSettings.isGrounded == true)
		{
			spiderAnimator.SetBool("Walk", true);
		}
		else if (movementInput < 0.01f && spiderAnimator.GetBool("Walk") == true)
		{
			spiderAnimator.SetBool("Walk", false);
		}

		if(Input.GetKey(KeyCode.A) && spiderAnimator.GetBool("Walk") == false && debugSettings.isGrounded == true || Input.GetKey(KeyCode.D) && spiderAnimator.GetBool("Walk") == false && debugSettings.isGrounded == true)
        {
			spiderAnimator.SetBool("Walk", true);
		}
		else if(Input.GetKeyUp(KeyCode.A) && spiderAnimator.GetBool("Walk") == true || Input.GetKeyUp(KeyCode.D) && spiderAnimator.GetBool("Walk") == true)
		{
			spiderAnimator.SetBool("Walk", false);
		}

		randomIdleTimer += Time.deltaTime;
		if (randomIdleTimer >= 10f)
		{
			randomIdle = UnityEngine.Random.Range(0, 2);

			if (randomIdle == 0)
			{
				spiderAnimator.SetTrigger("Idle_Shake");
			}
			else if (randomIdle == 1)
			{
				spiderAnimator.SetTrigger("Idle_LookAround");
			}

			randomIdleTimer = 0f;
		}
	}

	private bool IsUsingWeb()
	{
		if (springJointWeb.isSwingingWeb == true)
		{
			return true;
		}
		else if (hookWeb.isHookWebing == true)
		{
			return true;
		}
		else if(climbWeb.isClimbWebing == true)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private void SetVisibilityOfModel(bool isActive)
	{
		foreach (GameObject item in modelChildren)
		{
			item.SetActive(isActive);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawRay(transform.position, debugSettings.averageNormalDirection * 1f);
	}
}
