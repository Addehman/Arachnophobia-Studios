using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
	public float playerSpeed = 0.2f;
	public float slowPlayerSpeed = 0.1f;
	public float normalPlayerSpeed = 0.2f;
	public float sprintMultiAmount = 0.2f;
	public float jumpUpStrength = 100f;
	public float jumpFwdStrength = 50f;
	public float playerToGroundRange = 0.3f;
	public float turnSmoothTime = 0.1f;
	public float turnSpeed = 90f;
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
}

public class SpiderMovement : MonoBehaviour
{
	[HideInInspector] public Rigidbody rb;
	[HideInInspector] public Vector3 currentPosition;

	[SerializeField] private GameObject cmFPSCamera;
	[SerializeField] private GameObject cmTPCamera;
	[SerializeField] private GameObject spiderModel;

	public MainRaycastsAdjustment mainRaycastAdjustments;
	public ForwardsRaycastsAdjustment forwardsRaycastAdjustment;
	public BackwardsRaycastsAdjustment backwardsRaycastAdjustment;
	public RaycastGeneralSettings raycastGeneralSettings;
	public PlayerSettings playerSettings;
	public DebugSettings debugSettings;

	private enum RaycastTypes {MainForwards, MainBackwards, MainDown, Forwards, Backwards, Downwards, Any}
	private RaycastTypes raycastType;
	private Animator spiderAnimator;
	private Transform cam;
	private Vector3 myNormal;
	private float turnSmoothVelocity;
	private float gravityValue = -9.82f;
	private float sprintMulti;
	private CinemachineVirtualCamera cmvCamera;
	private ActivateOnKeypress activateOnKeypress;


	void Start()
	{
		rb = GetComponent<Rigidbody>();
		cam = GameObject.Find("Main Camera").transform;
		spiderAnimator = GetComponentInChildren<Animator>();
		cmvCamera = cmFPSCamera.GetComponent<CinemachineVirtualCamera>();
		activateOnKeypress = cmFPSCamera.GetComponent<ActivateOnKeypress>();
		activateOnKeypress.ActivationFPSCam += activateOnKeypress_ActivationFPSCam;
	}

	private void activateOnKeypress_ActivationFPSCam(bool isActive)
	{
		debugSettings.isFpsEnabled = isActive;
	}

	// Update is called once per frame
	void Update()
	{
		debugSettings.averageNormalDirections.Clear();

		currentPosition = transform.position;

		RaycastsToCast();
		Movement();
		Sprint();
		// isClimbing();
		SpiderJump();

		for (int i = 0; i < debugSettings.averageNormalDirections.Count; i++)
		{
			debugSettings.averageNormalDirection += debugSettings.averageNormalDirections[i];
		}
		
		debugSettings.averageNormalDirection /= debugSettings.averageNormalDirections.Count;

		if (debugSettings.averageNormalDirections.Count == 0)
		{
			debugSettings.averageNormalDirection = Vector3.up;
		}

		var lerpSpeed = 10f;

		myNormal = Vector3.Slerp(myNormal, debugSettings.averageNormalDirection, lerpSpeed * Time.deltaTime);
		// find forward direction with new myNormal:
		Vector3 myForward = Vector3.Cross(transform.right, myNormal);
		// align character to the new myNormal while keeping the forward direction:
		Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, lerpSpeed * Time.deltaTime);
		//try and make tha camera rotate with the player. Doesn't work as of now.
		// cam.transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, lerpSpeed * Time.deltaTime);
	}

	private void RaycastsToCast()
	{
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
			playerSettings.playerSpeed = playerSettings.slowPlayerSpeed;
			EdgeRaycastHelper(transform.TransformDirection(Vector3.back) + transform.TransformDirection(Vector3.down), raycastGeneralSettings.edgeRayOriginOffset);
			EdgeRaycastHelper(transform.TransformDirection(Vector3.back) + transform.TransformDirection(Vector3.down), raycastGeneralSettings.edgeRayOriginOffset1);
		}
		else if (debugSettings.backRayNoHit == true && Input.GetKey(KeyCode.S))
		{
			playerSettings.playerSpeed = playerSettings.slowPlayerSpeed;
			EdgeRaycastHelper(transform.TransformDirection(Vector3.forward) + transform.TransformDirection(Vector3.down), -raycastGeneralSettings.edgeRayOriginOffset);
			EdgeRaycastHelper(transform.TransformDirection(Vector3.forward) + transform.TransformDirection(Vector3.down), -raycastGeneralSettings.edgeRayOriginOffset1);
		}
		else 
		{
			playerSettings.playerSpeed = playerSettings.normalPlayerSpeed;
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
						//adam titta på denna
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
		// else if (isBackRay == true)
		// {
		// 	RaycastHit hit;
		// 	
		// }
		}
	}

	private void RaycastWeightMulti(List<Vector3> listToAddTo, int amountToMultiply, Vector3 normalToAdd)
	{
		for (int i = 0; i < amountToMultiply; i++)
		{
			listToAddTo.Add(normalToAdd);
		}
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
	}

	private void Movement()
	{
		float vertical = Input.GetAxisRaw("Vertical");
		float horizontal = Input.GetAxisRaw("Horizontal");

		// Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

		// if (direction.magnitude >= 0.1f)
		// {
		// 	float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
		// 	float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

			// Here we are determining what direction that the camera should apply it's direction to on the player.
			// NOTES: This doesn't do the trick.. Doesn't seem to make any difference

			// float directX = Mathf.Round(mainDownRayNormalDirection.x);
			// float directX = Mathf.Abs(averageNormalDirection.x);
			// float directY = Mathf.Abs(averageNormalDirection.y);
			// float directZ = Mathf.Abs(averageNormalDirection.z);
			// Vector3 moveDirection;

			// if (directX > directY && directX > directZ)
			// {
			// 	print ("setting to direction to X");
			// 	transform.rotation = Quaternion.Euler(angle, 0f, 0f);
			// 	moveDirection = Quaternion.Euler(targetAngle, 0f, 0f) * Vector3.forward; // should it really be forward? maybe transform.forward? I've tried Vector3.up..
			// }
			// else if (directZ > directX && directZ > directY)
			// {
			// 	print ("setting to direction to Z");
			// 	transform.rotation = Quaternion.Euler(0f, 0f, angle);
			// 	moveDirection = Quaternion.Euler(0f, 0f, targetAngle) * Vector3.forward;
			// }
			// else
			// {
			// 	print ("setting to direction to Y");
			// 	transform.rotation = Quaternion.Euler(0f, angle, 0f);
			// 	moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
			// }

			// // transform.rotation = Quaternion.Euler(0f, angle, 0f);
			// // Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
			
			// transform.Translate(moveDirection.normalized * (playerSpeed + sprintMulti) * Time.deltaTime, Space.World);
		// }
		if (debugSettings.isFpsEnabled == true)
		{
			spiderModel.SetActive(false);
		}
		else
		{
			spiderModel.SetActive(true);
		}
		transform.Rotate(0f, horizontal * playerSettings.turnSpeed * Time.deltaTime, 0f);
		// transform.Translate(horizontal * (playerSpeed + sprintMulti) * Time.deltaTime, 0, 0);
		transform.Translate(0, 0, vertical * (playerSettings.playerSpeed + sprintMulti) * Time.deltaTime);
	}

	private bool isClimbing()
	{
		if (transform.rotation.x > 20f)
		{
			return true;
		}
		else if (transform.rotation.y > 20f)
		{
			return true;
		}
		else if (transform.rotation.z > 20f)
		{
			return true;
		}
		else
		{
			return false;
		}
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
			sprintMulti = playerSettings.sprintMultiAmount;
		}
		
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			sprintMulti = 0f;
		}
	}
}
