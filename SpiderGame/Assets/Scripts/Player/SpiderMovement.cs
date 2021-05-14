using System.Collections.Generic;
using UnityEngine;

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
	public float normalDefaultPlayerSpeed = 0.2f;
	public float normalSprintMultiAmount = 0.2f;
	public float velocityPlayerSpeed = 30f;
	public float velocityNormalPlayerSpeed = 30f;
	public float velocitySprintMultiAmount = 60f;
	public float jumpUpStrength = 100f;
	public float jumpFwdStrength = 50f;
	public float playerToGroundRange = 0.3f;
	public float turnSmoothTime = 0.1f;
	public float turnSpeed = 90f;
	public float moveToSpeed = 1f;
}

[System.Serializable]
public class DebugSettings
{
	// [Header("Debug")]
	public bool isGrounded;
	public bool doDrawRayGizmos = true;
	public bool fwdRayNoHit = false;
	public bool backRayNoHit = false;
	public bool isFwdRayHitting;
	public Vector3 mainDownRayNormalDirection;
	public Vector3 averageNormalDirection;
	public List<Vector3> averageNormalDirections = new List<Vector3>();
	public Vector3 fwdRayHitNormalDebug;
	public bool isFpsEnabled = false;
	public bool isPlayerBeingVacuumed;
}

public class SpiderMovement : MonoBehaviour
{
	[HideInInspector] public Rigidbody rb;
	[HideInInspector] public Vector3 currentPosition;

	[SerializeField] private GameObject cmTPCamera;
	[SerializeField] private Transform movementParent;

	public MainRaycastsAdjustment mainRaycastAdjustments;
	public ForwardsRaycastsAdjustment forwardsRaycastAdjustment;
	public BackwardsRaycastsAdjustment backwardsRaycastAdjustment;
	public RaycastGeneralSettings raycastGeneralSettings;
	public PlayerSettings playerSettings;
	public DebugSettings debugSettings;

	private enum RaycastTypes {MainForwards, MainBackwards, MainDown, Forwards, Backwards, Downwards, Any}
	private RaycastTypes raycastType;
	private Animator spiderAnimator;
	private VacuumBlackhole vacuumBlackhole;
	private SpringJointWeb springJointWeb;
	private GameObject parentObject;
	private GameObject[] modelChildren;
	private Transform cam;
	private Transform lookAtTarget;
	private GameObject targetRotationObject;
	// private Transform rotateToTarget; // Probably to be removed
	private Vector3 myNormal;
	private float turnSmoothVelocity;
	private float gravityValue = -9.82f;
	private float sprintMulti;
	private int randomIdle;
	private float randomIdleTimer = 0f;
	private float rotationSlerpSpeed = 10f;
	// private float vertical;
	// private float horizontal;

	Vector3 currentForward; // Temporary
	Vector3 currentSide; // Temporary


	void Start()
	{
		spiderAnimator = GameObject.Find("Model_Character_Spider.4").GetComponent<Animator>();
		spiderAnimator.SetTrigger("Idle");

		cam = Camera.main.transform;
		Camera.main.GetComponent<ToggleCameras>().ActivationFPSCam += activateOnKeypress_ActivationFPSCam;

		vacuumBlackhole = FindObjectOfType<VacuumBlackhole>();
		vacuumBlackhole.PullingPlayer += vacuumBlackhole_PullingPlayer;

		springJointWeb = FindObjectOfType<SpringJointWeb>();

		lookAtTarget = FindObjectOfType<LookAtTargetController>().transform;
		// rotateToTarget = FindObjectOfType<RotateToTargetController>().transform;
		
		parentObject = transform.parent.gameObject;
		rb = parentObject.GetComponent<Rigidbody>();

		targetRotationObject = GameObject.Find("PlayerTargetRotation");

		// Here the reference is made for all the children of the spidermodel, used to be able to hide/show when in fpCamera-mode.
		int amountOfModelParts = 0;
		foreach (Transform item in transform)
		{
			amountOfModelParts ++;
		}
		modelChildren = new GameObject[amountOfModelParts];
		for (int i = 0; i < modelChildren.Length; i++)
		{
			modelChildren[i] = transform.GetChild(i).gameObject;
		}

		// moveToTargetController = 
	}

	private void activateOnKeypress_ActivationFPSCam(bool isActive)
	{
		debugSettings.isFpsEnabled = isActive;
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


		// vertical = Input.GetAxisRaw("Vertical");
		// horizsaontal = Input.GetAxisRaw("Horizontal");

		RaycastsToCast();
		// PlayerRotation();
		Sprint();
		SpiderJump();


		PlayerAnimation();
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
			// DefaultMovement();
			// CameraDirectionMovement();

			SetPlayerLocalUpDirection();
			MoveToPointMovement();

			// RotateWithEddie();
		}
	}

	private void RaycastsToCast()
	{
		// Consider making these into For-Loops instead!
		RaycastHelper(transform.TransformDirection(Vector3.forward) * mainRaycastAdjustments.rayFwdMod, 0f, RaycastTypes.MainForwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * mainRaycastAdjustments.rayBwdMod, 0f, RaycastTypes.MainBackwards);
		RaycastHelper(transform.TransformDirection(Vector3.down) * mainRaycastAdjustments.rayDownMod, mainRaycastAdjustments.rayDownOriginOffset, RaycastTypes.MainDown);

		RaycastHelper(transform.TransformDirection(Vector3.right) + transform.TransformDirection(Vector3.down), 0f, RaycastTypes.Any);
		RaycastHelper(transform.TransformDirection(Vector3.left) + transform.TransformDirection(Vector3.down), 0f, RaycastTypes.Any);

		RaycastHelper(transform.TransformDirection(Vector3.forward) * forwardsRaycastAdjustment.rayFwdMod1 + transform.TransformDirection(Vector3.down) * forwardsRaycastAdjustment.rayFwdModDown1, 0f, RaycastTypes.Forwards);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * forwardsRaycastAdjustment.rayFwdMod2 + transform.TransformDirection(Vector3.down) * forwardsRaycastAdjustment.rayFwdModDown2, 0f, RaycastTypes.Forwards);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * forwardsRaycastAdjustment.rayFwdMod3 + transform.TransformDirection(Vector3.down) * forwardsRaycastAdjustment.rayFwdModDown3, 0f, RaycastTypes.Forwards);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * forwardsRaycastAdjustment.rayFwdMod4 + transform.TransformDirection(Vector3.down) * forwardsRaycastAdjustment.rayFwdModDown4, 0f, RaycastTypes.Forwards);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * forwardsRaycastAdjustment.rayFwdMod5 + transform.TransformDirection(Vector3.down) * forwardsRaycastAdjustment.rayFwdModDown5, 0f, RaycastTypes.Forwards);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * forwardsRaycastAdjustment.rayFwdMod6 + transform.TransformDirection(Vector3.down) * forwardsRaycastAdjustment.rayFwdModDown6, 0f, RaycastTypes.Forwards);

		RaycastHelper(transform.TransformDirection(Vector3.back) * backwardsRaycastAdjustment.rayBwdMod1 + transform.TransformDirection(Vector3.down) * backwardsRaycastAdjustment.rayBwdModDown1, mainRaycastAdjustments.raysBackOriginOffset, RaycastTypes.Backwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * backwardsRaycastAdjustment.rayBwdMod2 + transform.TransformDirection(Vector3.down) * backwardsRaycastAdjustment.rayBwdModDown2, mainRaycastAdjustments.raysBackOriginOffset, RaycastTypes.Backwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * backwardsRaycastAdjustment.rayBwdMod3 + transform.TransformDirection(Vector3.down) * backwardsRaycastAdjustment.rayBwdModDown3, mainRaycastAdjustments.raysBackOriginOffset, RaycastTypes.Backwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * backwardsRaycastAdjustment.rayBwdMod4 + transform.TransformDirection(Vector3.down) * backwardsRaycastAdjustment.rayBwdModDown4, mainRaycastAdjustments.raysBackOriginOffset, RaycastTypes.Backwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * backwardsRaycastAdjustment.rayBwdMod5 + transform.TransformDirection(Vector3.down) * backwardsRaycastAdjustment.rayBwdModDown5, mainRaycastAdjustments.raysBackOriginOffset, RaycastTypes.Backwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * backwardsRaycastAdjustment.rayBwdMod6 + transform.TransformDirection(Vector3.down) * backwardsRaycastAdjustment.rayBwdModDown6, mainRaycastAdjustments.raysBackOriginOffset, RaycastTypes.Backwards);
		// Edge Raycasts:
		if (debugSettings.fwdRayNoHit == true && Input.GetKey(KeyCode.W))
		{
			playerSettings.normalPlayerSpeed = playerSettings.normalSlowPlayerSpeed;
			EdgeRaycastHelper(transform.TransformDirection(Vector3.back) + transform.TransformDirection(Vector3.down), raycastGeneralSettings.edgeRayOriginOffset);
			EdgeRaycastHelper(transform.TransformDirection(Vector3.back) + transform.TransformDirection(Vector3.down), raycastGeneralSettings.edgeRayOriginOffset1);
		}
		else if (debugSettings.backRayNoHit == true && Input.GetKey(KeyCode.S))
		{
			playerSettings.normalPlayerSpeed = playerSettings.normalSlowPlayerSpeed;
			EdgeRaycastHelper(transform.TransformDirection(Vector3.forward) + transform.TransformDirection(Vector3.down), -raycastGeneralSettings.edgeRayOriginOffset);
			EdgeRaycastHelper(transform.TransformDirection(Vector3.forward) + transform.TransformDirection(Vector3.down), -raycastGeneralSettings.edgeRayOriginOffset1);
		}
		else 
		{
			playerSettings.normalPlayerSpeed = playerSettings.normalDefaultPlayerSpeed;
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
				if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastGeneralSettings.raycastReach, raycastGeneralSettings.layerMask) && Input.GetKey(KeyCode.W))
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
					debugSettings.mainDownRayNormalDirection = hit.normal;

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
					debugSettings.mainDownRayNormalDirection = Vector3.zero;
				}
				break;
			case RaycastTypes.MainBackwards:
				if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastGeneralSettings.raycastReach, raycastGeneralSettings.layerMask) && Input.GetKey(KeyCode.S))
				{
					if (debugSettings.doDrawRayGizmos == true)
					{
						Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastGeneralSettings.raycastReach);
					}
					RaycastWeightMulti(debugSettings.averageNormalDirections, raycastGeneralSettings.backRaycastWeightMultiplier, hit.normal);
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
					debugSettings.fwdRayNoHit = false;
				}
				else
				{
					debugSettings.fwdRayNoHit = true;
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

	private void SetPlayerLocalUpDirection()
	{
		for (int i = 0; i < debugSettings.averageNormalDirections.Count; i++)
		{
			debugSettings.averageNormalDirection += debugSettings.averageNormalDirections[i];
		}
		
		if (debugSettings.averageNormalDirections.Count == 0)
		{
			debugSettings.averageNormalDirection = Vector3.up;
		}
		else
		{
			debugSettings.averageNormalDirection /= debugSettings.averageNormalDirections.Count;
		}

		movementParent.transform.up = debugSettings.averageNormalDirection; // if I want to lerp, make sure to also lerp the other rotation(transform.LookAt), especially with the same tick/time-amount(t).
		
		/*
		myNormal = Vector3.Slerp(myNormal, debugSettings.averageNormalDirection, rotationSlerpSpeed * Time.deltaTime);
		// myNormal = debugSettings.averageNormalDirection;
		// find forward direction with new myNormal:
		Vector3 myForward = Vector3.Cross(transform.right, myNormal);
		// align character to the new myNormal while keeping the forward direction:
		// My attempt to control the direction of the player:
		// targetRotationObject.transform.LookAt(lookAtTarget, myNormal);
		// Vector3 myForward = targetRotationObject.transform.forward;
		Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
		movementParent.transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSlerpSpeed * Time.deltaTime);
		*/
	}

	private void DefaultMovement() 
	{
		float vertical = Input.GetAxisRaw("Vertical");
		float horizontal = Input.GetAxisRaw("Horizontal");

		if(currentForward == Vector3.zero)
			currentForward = transform.forward;
		if(currentSide == Vector3.zero)
			currentSide = -transform.right;

		Vector3 forwardMotion = currentForward * vertical;
		Vector3 sideMotion = currentSide * horizontal;

		Vector3 movement = (forwardMotion + sideMotion) * (playerSettings.normalPlayerSpeed + sprintMulti) * Time.deltaTime;
		transform.Translate(movement);

		if(horizontal + vertical == 0)
		{
			currentForward = Vector3.zero;
			currentSide = Vector3.zero;
		}
	}

	private void MoveToPointMovement()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");
		
		if (Vector3.Distance(transform.position, lookAtTarget.position) >= 0.05f)
		{
			// I want to do a Slerp/Lerp on this and the normal/transform.up rotation - this and the on under "SetPlayerLocalUpDirection()".
			transform.LookAt(lookAtTarget, lookAtTarget.up);
			
			/*
			Vector3 targetDirection = lookAtTarget.position - transform.position;
			Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSlerpSpeed * Time.deltaTime, 0f);
			transform.rotation = Quaternion.LookRotation(newDirection);
			*/

			// targetRotationObject.transform.LookAt(lookAtTarget, lookAtTarget.up);
			// Quaternion targetRot = targetRotationObject.transform.rotation;
			// transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSlerpSpeed * Time.deltaTime);
		}

		Vector3 movement = new Vector3(vertical, 0f, horizontal);
		if (movement.sqrMagnitude > 0f)
		{
			parentObject.transform.Translate(transform.forward * (playerSettings.normalPlayerSpeed + sprintMulti) * Time.deltaTime);
		}
		
		// parentObject.transform.Translate(movement);

		// transform.position = Vector3.Lerp(transform.position, transform.forward*0.1f, playerSettings.moveToSpeed * Time.deltaTime);
		// parentObject.transform.position = Vector3.Lerp(transform.position, moveToTarget.position, playerSettings.moveToSpeed * Time.deltaTime);
	}
	
	/// <summary>
	/// Sets the forward-direction of the player according to the camera's forward direction.
	/// </summary>
	private void CameraDirectionMovement()
	{
		float vertical = Input.GetAxisRaw("Vertical");
		float horizontal = Input.GetAxisRaw("Horizontal");

		float mouseHorizontal = Input.GetAxis("Mouse X");
		Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

		if (direction.magnitude >= 0.1f)
		{
			float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, playerSettings.turnSmoothTime);

			Vector3 moveDirection = cam.TransformDirection(direction); // trying to identify a rounded direction to set as the player's move direction.. not a longterm solution.
			// Mathf.Abs(moveDirection.x);
			// Mathf.Abs(moveDirection.y);
			// Mathf.Abs(moveDirection.z);

			// if (moveDirection.x > moveDirection.y && moveDirection.x > moveDirection.z)
			// {
			// 	moveDirection = Vector3.right;
			// }
			// else if (moveDirection.y > moveDirection.x && moveDirection.y > moveDirection.z)
			// {
			// 	moveDirection = Vector3.up;
			// }
			// else if (moveDirection.z > moveDirection.x && moveDirection.z > moveDirection.y)
			// {
			// 	moveDirection = Vector3.forward;
			// }

			// transform.rotation = Quaternion.Euler(0f, angle, 0f);
			// transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0f, transform.eulerAngles.z); // Reset the Y-axis, without changing the other ones
			
			transform.Translate(moveDirection * (playerSettings.normalPlayerSpeed + sprintMulti) * Time.deltaTime);
			// transform.Translate(moveDirection * 1 * Time.deltaTime);

			// transform.rotation = Quaternion.Euler(transform.eulerAngles.x, angle, transform.eulerAngles.z);
			transform.rotation = Quaternion.Euler(transform.eulerAngles.x, angle, transform.eulerAngles.z);
		}
		// transform.Rotate(0f, horizontal * playerSettings.turnSpeed * Time.deltaTime, 0f);
	}

	private void VacuumPullingMovement() // The Rigidbody-based movement used only for when the player is in range of being pulled with rb.AddForce towards the Robot Vacuum Cleaner - becomes most smooth this way.
	{
		float vertical = Input.GetAxisRaw("Vertical");
		float horizontal = Input.GetAxisRaw("Horizontal");

		rb.velocity = (transform.forward * vertical) * (playerSettings.velocityPlayerSpeed + sprintMulti) * Time.deltaTime;
	}

	private void PlayerRotation()
	{
		float vertical = Input.GetAxisRaw("Vertical");
		float horizontal = Input.GetAxisRaw("Horizontal");

		// float horizontalMouse = Input.GetAxis("Mouse X");
		transform.Rotate(0f, horizontal * playerSettings.turnSpeed * Time.deltaTime, 0f);
	}

	private void SpiderJump()
	{
		if (Input.GetKey(KeyCode.W) == false && Input.GetButtonDown("Jump") && debugSettings.isGrounded == true)
		{
			spiderAnimator.SetBool("Jump", true);
			rb.AddForce(transform.up * playerSettings.jumpUpStrength);
			debugSettings.isGrounded = false;
		}
		else if (Input.GetKey(KeyCode.W) && Input.GetButtonDown("Jump") && debugSettings.isGrounded == true)
		{
			spiderAnimator.SetBool("Jump", true);
			rb.AddForce((transform.up + transform.forward) * playerSettings.jumpFwdStrength);
			debugSettings.isGrounded = false;
		}
	}
	// Binds key for player to use to increase move speed.
	private void Sprint()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			if (debugSettings.isPlayerBeingVacuumed == true)
			{
				sprintMulti = playerSettings.velocitySprintMultiAmount;
			}
			else
			{
				sprintMulti = playerSettings.normalSprintMultiAmount;
			}
		}
		
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			sprintMulti = 0f;
		}
	}

	private void PlayerAnimation()
	{
		if (springJointWeb.isSwingingWeb == true)
        {
			spiderAnimator.SetBool("Web", true);
        }

		else if (springJointWeb.isSwingingWeb == false)
		{
			spiderAnimator.SetBool("Web", false);
		}


		if (((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W)) && spiderAnimator.GetBool("Walk") == false && debugSettings.isGrounded == true))
		{
			spiderAnimator.SetBool("Walk", true);
		}

		else if (((Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.W)) && spiderAnimator.GetBool("Walk") == true))
		{
			spiderAnimator.SetBool("Walk", false);
		}

		randomIdleTimer += Time.deltaTime;
		if (randomIdleTimer >= 10f)
		{
			randomIdle = Random.Range(0, 2);

			if(randomIdle == 0)
			{
				spiderAnimator.SetTrigger("Idle_Shake");
			}
			else if(randomIdle == 1)
			{
				spiderAnimator.SetTrigger("Idle_LookAround");
			}

			randomIdleTimer = 0f;
		}

		if (debugSettings.isFpsEnabled == true)
		{
			// spiderModel.SetActive(false);
			foreach (GameObject item in modelChildren)
			{
				item.SetActive(false);
			}
		}
		else
		{
			// spiderModel.SetActive(true);
			foreach (GameObject item in modelChildren)
			{
				item.SetActive(true);
			}
		}
	}

	private void RotateWithEddie()
	{
		float hor = Input.GetAxis("Horizontal");
		float ver = Input.GetAxis("Vertical");

		Vector3 input = new Vector3(hor, 0f, ver);

		Vector3 moveVector = cam.TransformDirection(input);
		moveVector.y = 0f;
		moveVector.Normalize();

		moveVector *= 0.1f * Time.deltaTime;
		parentObject.transform.Translate(moveVector);

		if (input.sqrMagnitude <= 0f)
		{
			return;
		}

		Vector3 velocity = new Vector3(moveVector.x, 0f, moveVector.z);
		Quaternion targetRotation = Quaternion.LookRotation(velocity.normalized);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 270f * Time.deltaTime);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawRay(transform.position, debugSettings.averageNormalDirection * 1f);
	}
}
