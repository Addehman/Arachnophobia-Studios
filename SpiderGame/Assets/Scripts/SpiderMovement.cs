using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
	[HideInInspector] public Rigidbody rb;
	[HideInInspector] public Vector3 currentPosition;

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
	[SerializeField] private float edgeRayOriginOffset = 0.2f;
	[SerializeField] private LayerMask layerMask;

	[Header("Player Settings")]
	[SerializeField] private float playerSpeed = 2f;
	[SerializeField] private float sprintMulti;
	[SerializeField] private float jumpUpStrength = 100f;
	[SerializeField] private float jumpFwdStrength = 50f;
	[SerializeField] private float playerToGroundRange = 0.3f;
	[Space(5f)]

	[Header("Debug")]
	[SerializeField] private bool isGrounded;
	[SerializeField] private bool drawRayGizmos;
	[SerializeField] private bool fwdRayNoHit = false;
	[SerializeField] private bool backRayNoHit = false;

	[SerializeField] private List<Vector3> averageNormalDirections = new List<Vector3>();
	private Vector3 averageNormalDirection;
	private Vector3 myNormal;
	private float gravityValue = -9.82f;


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
	}

	private void RaycastsToCast()
	{
		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod, 0f, false, false, false);
		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod, 0f, false, false, false);
		RaycastHelper(transform.TransformDirection(Vector3.down) * rayDownMod, rayDownOriginOffset, true, false, false);

		RaycastHelper(transform.TransformDirection(Vector3.right) + transform.TransformDirection(Vector3.down), 0f, false, false, false);
		RaycastHelper(transform.TransformDirection(Vector3.left) + transform.TransformDirection(Vector3.down), 0f, false, false, false);

		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod1 + transform.TransformDirection(Vector3.down) * rayFwdModDown1, 0f, false, true, false);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod2 + transform.TransformDirection(Vector3.down) * rayFwdModDown2, 0f, false, true, false);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod3 + transform.TransformDirection(Vector3.down) * rayFwdModDown3, 0f, false, true, false);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod4 + transform.TransformDirection(Vector3.down) * rayFwdModDown4, 0f, false, true, false);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod5 + transform.TransformDirection(Vector3.down) * rayFwdModDown5, 0f, false, true, false);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod6 + transform.TransformDirection(Vector3.down) * rayFwdModDown6, 0f, false, true, false);

		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod1 + transform.TransformDirection(Vector3.down) * rayBwdModDown1, raysBackOriginOffset, false, false, true);
		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod2 + transform.TransformDirection(Vector3.down) * rayBwdModDown2, raysBackOriginOffset, false, false, true);
		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod3 + transform.TransformDirection(Vector3.down) * rayBwdModDown3, raysBackOriginOffset, false, false, true);
		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod4 + transform.TransformDirection(Vector3.down) * rayBwdModDown4, raysBackOriginOffset, false, false, true);
		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod5 + transform.TransformDirection(Vector3.down) * rayBwdModDown5, raysBackOriginOffset, false, false, true);
		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod6 + transform.TransformDirection(Vector3.down) * rayBwdModDown6, raysBackOriginOffset, false, false, true);

		// Edge Raycasts:
		if (fwdRayNoHit == true)
		{
			EdgeRaycastHelper(transform.TransformDirection(Vector3.back) + transform.TransformDirection(Vector3.down), edgeRayOriginOffset);
		}
		// else if (backRayNoHit == true)
		// {
		// 	EdgeRaycastHelper(transform.TransformDirection(Vector3.forward) + transform.TransformDirection(Vector3.down), -edgeRayOriginOffset);
		// }
	}

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

	private void RaycastHelper(Vector3 direction, float originOffsetValue, bool isDownRay, bool isFwdRay, bool isBackRay)
	{
		Vector3 originOffset = transform.TransformDirection(Vector3.back) * originOffsetValue;
		if (isDownRay == true) // if it's the ray that is shot straight down, then it's also checking for Ground to set isGrounded.
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastReach, layerMask))
			{
				if (drawRayGizmos == true)
				{
					Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastReach);
				}
				averageNormalDirections.Add(hit.normal);

				float rbVelocity = rb.velocity.y;
				if (hit.distance < playerToGroundRange && rbVelocity < 0f)
				{
					isGrounded = true;
				}
			}
			else
			{
				isGrounded = false;
			}
		}
		else if (isFwdRay == true)
		{
			RaycastHit hit;
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
		}
		// else if (isBackRay == true)
		// {
		// 	RaycastHit hit;
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
		// }
		else
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position - originOffset, direction, out hit, raycastReach, layerMask))
			{
				if (drawRayGizmos == true)
				{
					Debug.DrawRay(transform.position - originOffset, direction, Color.red, raycastReach);
				}
				averageNormalDirections.Add(hit.normal);
			}
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
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

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

	private void SpiderJump() //It's possible to spam the jump-button to get a slightly higher jump than intended, need to find a more proper way to jump
	{
		/*if (isClimbing() == true && Input.GetButtonDown("Jump") && isGrounded == true)
		{
			rb.AddForce((transform.up + transform.forward) * 30);
			isGrounded = false;
		}
		else */
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

	// void OnDrawGizmos()
	// {
	// 	// Draws a blue line from this transform to the target
	// 	Gizmos.color = Color.blue;
	// 	Vector3 startPos = transform.position;
	// 	Vector3 endPos = transform.position + transform.TransformDirection(Vector3.forward) * 2 - transform.up * 2;
	// 	Gizmos.DrawLine(startPos, endPos);
	// 	Gizmos.DrawLine(startPos, transform.position - transform.TransformDirection(Vector3.forward) * 2 - transform.up * 2);
	// }
}
