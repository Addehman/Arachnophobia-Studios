using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
	[HideInInspector] public Rigidbody rb;
	[HideInInspector] public Vector3 currentPosition;

	[SerializeField] Transform cam;

	[Header("Main Raycasts Adjustment")]
	[SerializeField] private float rayFwdMod	= 1f;
	[SerializeField] private float rayBwdMod	= 1f;
	[SerializeField] private float rayDownMod	= 1f;
	[Space(2f)]

	[Header("Forwards-Raycasts Adjustment")]
	[SerializeField] private float rayFwdMod1 		= 0.25f;
	[SerializeField] private float rayFwdModDown1 	= 1f;
	[SerializeField] private float rayFwdMod2 		= 0.5f;
	[SerializeField] private float rayFwdModDown2	= 1f;
	[SerializeField] private float rayFwdMod3		= 0.75f;
	[SerializeField] private float rayFwdModDown3	= 1f;
	[SerializeField] private float rayFwdMod4		= 1f;
	[SerializeField] private float rayFwdModDown4	= 1f;
	[SerializeField] private float rayFwdMod5		= 1f;
	[SerializeField] private float rayFwdModDown5	= 0.63f;
	[SerializeField] private float rayFwdMod6		= 1f;
	[SerializeField] private float rayFwdModDown6	= 0.38f;
	[Space(2f)]

	[Header("Backwards-Raycasts Adjustment")]
	[SerializeField] private float rayBwdMod1		= 0.25f;
	[SerializeField] private float rayBwdModDown1	= 1f;
	[SerializeField] private float rayBwdMod2		= 0.5f;
	[SerializeField] private float rayBwdModDown2	= 1f;
	[SerializeField] private float rayBwdMod3		= 0.75f;
	[SerializeField] private float rayBwdModDown3	= 1f;
	[SerializeField] private float rayBwdMod4		= 1f;
	[SerializeField] private float rayBwdModDown4	= 1f;
	[SerializeField] private float rayBwdMod5		= 1f;
	[SerializeField] private float rayBwdModDown5	= 0.63f;
	[SerializeField] private float rayBwdMod6		= 1f;
	[SerializeField] private float rayBwdModDown6	= 0.38f;
	[Space(5f)]

	[Header("Raycast Settings")]
	[SerializeField] private float raycastReach = 0.1f;
	[SerializeField] private float rayDownOriginOffset = -0.01f;
	[SerializeField] private float raysBackOriginOffset = -0.02f;
	[SerializeField] private float raycastReachEdge = 0.1f;
	[SerializeField] private float edgeRayOriginOffset = 0.03f;
	[SerializeField] private float edgeRayOriginOffset1 = 0.04f;
	[SerializeField] private LayerMask layerMask;

	[Header("Player Settings")]
	[SerializeField] private float playerSpeed = 0.2f;
	[SerializeField] private float slowPlayerSpeed = 0.05f;
	[SerializeField] private float normalPlayerSpeed = 0.2f;
	[SerializeField] private float sprintMulti;
	[SerializeField] private float jumpUpStrength = 100f;
	[SerializeField] private float jumpFwdStrength = 50f;
	[SerializeField] private float playerToGroundRange = 0.3f;
	[SerializeField] private float turnSmoothTime = 0.1f;
	[SerializeField] private float turnSmoothVelocity;
	[Space(5f)]

	[Header("Debug")]
	[SerializeField] private bool isGrounded;
	[SerializeField] private bool drawRayGizmos;
	[SerializeField] private bool fwdRayNoHit = false;
	[SerializeField] private bool backRayNoHit = false;
	[SerializeField] private bool isFwdRayHitting;
	[SerializeField] private Vector3 mainDownRayNormalDirection;
	[SerializeField] private Vector3 averageNormalDirection;
	[SerializeField] private List<Vector3> averageNormalDirections = new List<Vector3>();

	private Vector3 myNormal;
	private float gravityValue = -9.82f;
	private enum RaycastTypes {MainForwards, MainBackwards, MainDown, Forwards, Backwards, Downwards, Any}
	private RaycastTypes raycastType;


	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		averageNormalDirections.Clear();

		currentPosition = transform.position;

		RaycastsToCast();
		Movement();
		Sprint();
		// isClimbing();
		SpiderJump();

		for (int i = 0; i < averageNormalDirections.Count; i++)
			averageNormalDirection += averageNormalDirections[i];

		averageNormalDirection /= averageNormalDirections.Count;

		if (averageNormalDirections.Count == 0)
		{
			averageNormalDirection = Vector3.up;
		}

		var lerpSpeed = 10f;

		myNormal = Vector3.Slerp(myNormal, averageNormalDirection, lerpSpeed * Time.deltaTime);
		// find forward direction with new myNormal:
		Vector3 myForward = Vector3.Cross(transform.right, myNormal);
		// align character to the new myNormal while keeping the forward direction:
		Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, lerpSpeed * Time.deltaTime);
		//try and make tha camera rotate with the player. Doesn't work as of now.
		cam.transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, lerpSpeed * Time.deltaTime);
	}

	private void RaycastsToCast()
	{
		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod, 0f, RaycastTypes.MainForwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod, 0f, RaycastTypes.MainBackwards);
		RaycastHelper(transform.TransformDirection(Vector3.down) * rayDownMod, rayDownOriginOffset, RaycastTypes.MainDown);

		RaycastHelper(transform.TransformDirection(Vector3.right) + transform.TransformDirection(Vector3.down), 0f, RaycastTypes.Any);
		RaycastHelper(transform.TransformDirection(Vector3.left) + transform.TransformDirection(Vector3.down), 0f, RaycastTypes.Any);

		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod1 + transform.TransformDirection(Vector3.down) * rayFwdModDown1, 0f, RaycastTypes.Forwards);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod2 + transform.TransformDirection(Vector3.down) * rayFwdModDown2, 0f, RaycastTypes.Forwards);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod3 + transform.TransformDirection(Vector3.down) * rayFwdModDown3, 0f, RaycastTypes.Forwards);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod4 + transform.TransformDirection(Vector3.down) * rayFwdModDown4, 0f, RaycastTypes.Forwards);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod5 + transform.TransformDirection(Vector3.down) * rayFwdModDown5, 0f, RaycastTypes.Forwards);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod6 + transform.TransformDirection(Vector3.down) * rayFwdModDown6, 0f, RaycastTypes.Forwards);

		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod1 + transform.TransformDirection(Vector3.down) * rayBwdModDown1, raysBackOriginOffset, RaycastTypes.Backwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod2 + transform.TransformDirection(Vector3.down) * rayBwdModDown2, raysBackOriginOffset, RaycastTypes.Backwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod3 + transform.TransformDirection(Vector3.down) * rayBwdModDown3, raysBackOriginOffset, RaycastTypes.Backwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod4 + transform.TransformDirection(Vector3.down) * rayBwdModDown4, raysBackOriginOffset, RaycastTypes.Backwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod5 + transform.TransformDirection(Vector3.down) * rayBwdModDown5, raysBackOriginOffset, RaycastTypes.Backwards);
		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod6 + transform.TransformDirection(Vector3.down) * rayBwdModDown6, raysBackOriginOffset, RaycastTypes.Backwards);
		// Edge Raycasts:
		if (fwdRayNoHit == true)
		{
			playerSpeed = slowPlayerSpeed;
			EdgeRaycastHelper(transform.TransformDirection(Vector3.back) + transform.TransformDirection(Vector3.down), edgeRayOriginOffset);
			EdgeRaycastHelper(transform.TransformDirection(Vector3.back) + transform.TransformDirection(Vector3.down), edgeRayOriginOffset1);
		}
		else 
		{
			playerSpeed = normalPlayerSpeed;
		}
		// else if (backRayNoHit == true)
		// {
		// 	EdgeRaycastHelper(transform.TransformDirection(Vector3.forward) + transform.TransformDirection(Vector3.down), -edgeRayOriginOffset);
		// }
	}
	// Special "Hook"- or Edge-Raycasts, used to look over edges to find footing where the other rays won't reach.
	private void EdgeRaycastHelper(Vector3 direction, float originOffsetValue)
	{
		RaycastHit hit;
		Vector3 originOffset = transform.TransformDirection(Vector3.back) * originOffsetValue;
		if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastReachEdge, layerMask))
		{
			if (drawRayGizmos == true)
			{
				Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastReachEdge);
			}
			averageNormalDirections.Add(hit.normal);
		}
	}
	// Main Raycasts function, that casts the rays that balances the player's rotation according to all the normals that these rays find.
	private void RaycastHelper(Vector3 direction, float originOffsetValue, RaycastTypes inRaycastType)
	{
		Vector3 originOffset = transform.TransformDirection(Vector3.back) * originOffsetValue;
		RaycastHit hit;
		
		switch (inRaycastType)
		{
			// case RaycastTypes.MainForwards:
			// 	if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastReach, layerMask))
			// 	{
			// 		if (drawRayGizmos == true)
			// 		{
			// 			Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastReach);
			// 		}
			// 		averageNormalDirections.Add(hit.normal);
			// 		isFwdRayHitting = true;
			// 	}
			// 	else
			// 	{
			// 		isFwdRayHitting = false;
			// 	}
			// 	break;
			case RaycastTypes.MainDown:
				if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastReach, layerMask))
				{
					if (drawRayGizmos == true)
					{
						Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastReach);
					}
					averageNormalDirections.Add(hit.normal);
					mainDownRayNormalDirection = hit.normal;

					float rbVelocity = rb.velocity.y;
					if (hit.distance < playerToGroundRange && rbVelocity < 0f)
					{
						isGrounded = true;
					}
				}
				else
				{
					isGrounded = false;
					mainDownRayNormalDirection = Vector3.zero;
				}
				break;
			case RaycastTypes.Forwards:
				if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastReach, layerMask))
				{
					if (drawRayGizmos == true)
					{
						Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastReach);
					}
					averageNormalDirections.Add(hit.normal);
					fwdRayNoHit = false;
				}
				else
				{
					fwdRayNoHit = true;
				}
				break;
			// case RaycastTypes.Backwards:
			// 	if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastReach))
			// 	{
			// 		if (drawRayGizmos == true)
			// 		{
			// 			Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastReach);
			// 		}
			// 		averageNormalDirections.Add(hit.normal);
			// 		backRayNoHit = false;
			// 	}
			// 	else
			// 	{
			// 		backRayNoHit = true;
			// 	}
			// 	break;
			default:
				if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastReach, layerMask))
				{
					if (drawRayGizmos == true)
					{
						Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastReach);
					}
					averageNormalDirections.Add(hit.normal);
				}
				break;
		// else if (isBackRay == true)
		// {
		// 	RaycastHit hit;
		// 	
		// }
		}
	}

	private void FixedUpdate()
	{
		// apply constant weight force according to character normal:
		if (isGrounded == true)
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

		// 	// Here we are determining what direction that the camera should apply it's direction to on the player.
		// 	// NOTES: This doesn't do the trick.. Doesn't seem to make any difference

		// 	// float directX = Mathf.Round(mainDownRayNormalDirection.x);
		// 	float directX = Mathf.Abs(averageNormalDirection.x);
		// 	float directY = Mathf.Abs(averageNormalDirection.y);
		// 	float directZ = Mathf.Abs(averageNormalDirection.z);
		// 	Vector3 moveDirection;

		// 	if (directX > directY && directX > directZ)
		// 	{
		// 		print ("setting to direction to X");
		// 		transform.rotation = Quaternion.Euler(angle, 0f, 0f);
		// 		moveDirection = Quaternion.Euler(targetAngle, 0f, 0f) * Vector3.forward; // should it really be forward? maybe transform.forward? I've tried Vector3.up..
		// 	}
		// 	else if (directZ > directX && directZ > directY)
		// 	{
		// 		print ("setting to direction to Z");
		// 		transform.rotation = Quaternion.Euler(0f, 0f, angle);
		// 		moveDirection = Quaternion.Euler(0f, 0f, targetAngle) * Vector3.forward;
		// 	}
		// 	else
		// 	{
		// 		print ("setting to direction to Y");
		// 		transform.rotation = Quaternion.Euler(0f, angle, 0f);
		// 		moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
		// 	}
			
		// 	transform.Translate(moveDirection.normalized * (playerSpeed + sprintMulti) * Time.deltaTime, Space.World);
		// }

		transform.Rotate(0, horizontal * 90 * Time.deltaTime, 0);
		// transform.Translate(horizontal * (playerSpeed + sprintMulti) * Time.deltaTime, 0, 0);
		transform.Translate(0, 0, vertical * (playerSpeed + sprintMulti) * Time.deltaTime);




		// if (vertical > 0.01f)
		// {
		// 	rb.AddForce(transform.forward * vertical * Time.deltaTime * playerSpeed);
		// }
		// else if (vertical < -0.01f)
		// {
		// 	rb.AddForce(-transform.forward * vertical * Time.deltaTime * playerSpeed);
		// }

		// if (horizontal > 0.01f)
		// {
		// 	rb.AddForce(transform.right * horizontal * Time.deltaTime * playerSpeed);
		// }
		// else if (horizontal < -0.01f)
		// {
		// 	rb.AddForce(-transform.right * horizontal * Time.deltaTime * playerSpeed);
		// }

		// if (vertical == 0 || horizontal == 0)
		// {
		// 	rb.velocity = Vector3.zero;
		// }
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
		if (Input.GetKey(KeyCode.W) == false && Input.GetButtonDown("Jump") && isGrounded == true)
		{
			rb.AddForce(transform.up * jumpUpStrength);
			isGrounded = false;
		}
		else if (Input.GetKey(KeyCode.W) && Input.GetButtonDown("Jump") && isGrounded == true)
		{
			rb.AddForce((transform.up + transform.forward) * jumpFwdStrength);
			isGrounded = false;
		}
	}
	// Binds key for player to use to increase move speed.
	private void Sprint()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			sprintMulti = 0.2f;
		}
		
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			sprintMulti = 0f;
		}
	}
}
