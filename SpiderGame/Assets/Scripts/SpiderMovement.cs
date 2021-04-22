using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
	[HideInInspector] public Rigidbody rb;

	[Header("Raycast Forwards Adjustments")]
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

	[Header("Raycast Backwards Adjustments")]
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

	[Header("Player Settings")]
	[SerializeField] private float playerSpeed = 2f;
	[SerializeField] private float sprintMulti;
	[SerializeField] private float jumpStrength = 300f;
	[SerializeField] private float playerToGroundRange = 0.3f;
	public Vector3 currentPosition;
	[Space(5f)]

	[SerializeField] private bool isGrounded;

	private List<Vector3> averageNormalDirections = new List<Vector3>();
	private Vector3 averageNormalDirection;
	private Vector3 myNormal;


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
		Sprint();
		SpiderJump();

		for (int i = 0; i < averageNormalDirections.Count; i++)
			averageNormalDirection += averageNormalDirections[i];

		averageNormalDirection /= averageNormalDirections.Count;

		if (averageNormalDirections.Count == 0)
		{
			averageNormalDirection = Vector3.up;
		}

		var lerpSpeed = 10f;

		transform.Rotate(0, Input.GetAxis("Horizontal") * 90 * Time.deltaTime, 0);

		myNormal = Vector3.Slerp(myNormal, averageNormalDirection, lerpSpeed * Time.deltaTime);
		// find forward direction with new myNormal:
		Vector3 myForward = Vector3.Cross(transform.right, myNormal);
		// align character to the new myNormal while keeping the forward direction:
		Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, lerpSpeed * Time.deltaTime);

		transform.Translate(0, 0, Input.GetAxis("Vertical") * (playerSpeed + sprintMulti) * Time.deltaTime);
	}

	private void RaycastsToCast()
	{
		RaycastHelper(transform.TransformDirection(Vector3.forward), false);
		RaycastHelper(transform.TransformDirection(Vector3.back), false);
		RaycastHelper(transform.TransformDirection(Vector3.down), true);

		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod1 + transform.TransformDirection(Vector3.down) * rayBwdModDown1, false);
		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod2 + transform.TransformDirection(Vector3.down) * rayBwdModDown2, false);
		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod3 + transform.TransformDirection(Vector3.down) * rayBwdModDown3, false);
		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod4 + transform.TransformDirection(Vector3.down) * rayBwdModDown4, false);
		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod5 + transform.TransformDirection(Vector3.down) * rayBwdModDown5, false);
		RaycastHelper(transform.TransformDirection(Vector3.back) * rayBwdMod6 + transform.TransformDirection(Vector3.down) * rayBwdModDown6, false);

		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod1 + transform.TransformDirection(Vector3.down) * rayFwdModDown1, false);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod2 + transform.TransformDirection(Vector3.down) * rayFwdModDown2, false);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod3 + transform.TransformDirection(Vector3.down) * rayFwdModDown3, false);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod4 + transform.TransformDirection(Vector3.down) * rayFwdModDown4, false);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod5 + transform.TransformDirection(Vector3.down) * rayFwdModDown5, false);
		RaycastHelper(transform.TransformDirection(Vector3.forward) * rayFwdMod6 + transform.TransformDirection(Vector3.down) * rayFwdModDown6, false);

		RaycastHelper(transform.TransformDirection(Vector3.right) + transform.TransformDirection(Vector3.down), false);
		RaycastHelper(transform.TransformDirection(Vector3.left) + transform.TransformDirection(Vector3.down), false);
	}

	private void RaycastHelper(Vector3 direction, bool isDownRay)
	{
		if (isDownRay == true)
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, direction, out hit, 0.1f))
			{
				Debug.DrawRay(transform.position, direction, Color.red, 0.1f);
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
		else
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, direction, out hit, 0.1f))
			{
				Debug.DrawRay(transform.position, direction, Color.red, 0.1f);
				averageNormalDirections.Add(hit.normal);
			}
		}
	}

	private void FixedUpdate()
	{
		// apply constant weight force according to character normal:
		rb.AddForce(-9.8f * rb.mass * transform.up);
	}

	private void SpiderJump() //It's possible to spam the jump-button to get a slightly higher jump than intended, need to find a more proper way to jump
	{
		if (Input.GetButtonDown("Jump") && isGrounded == true)
		{
			rb.AddForce(transform.up * jumpStrength);
			isGrounded = false;
		}
	}

	private void Sprint()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			sprintMulti = 2f;
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
