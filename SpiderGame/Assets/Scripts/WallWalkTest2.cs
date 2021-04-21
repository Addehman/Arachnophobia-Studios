using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWalkTest2 : MonoBehaviour
{
	public float playerSpeed = 5f;
	private Rigidbody rb;
	private List<Vector3> averageNormalDirections = new List<Vector3>();
	private Vector3 averageNormalDirection;
	private Vector3 myNormal;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		averageNormalDirections.Clear();

		RayCastHelper(transform.TransformDirection(Vector3.forward));
		RayCastHelper(transform.TransformDirection(Vector3.back));
		RayCastHelper(transform.TransformDirection(Vector3.down));

		RayCastHelper(transform.TransformDirection(Vector3.back) * 0.25f + transform.TransformDirection(Vector3.down));
		RayCastHelper(transform.TransformDirection(Vector3.back) * 0.5f + transform.TransformDirection(Vector3.down));
		RayCastHelper(transform.TransformDirection(Vector3.back) * 0.75f + transform.TransformDirection(Vector3.down));

		RayCastHelper(transform.TransformDirection(Vector3.forward) * 0.25f + transform.TransformDirection(Vector3.down));
		RayCastHelper(transform.TransformDirection(Vector3.forward) * 0.5f + transform.TransformDirection(Vector3.down));
		RayCastHelper(transform.TransformDirection(Vector3.forward) * 0.75f + transform.TransformDirection(Vector3.down));

		RayCastHelper(transform.TransformDirection(Vector3.right) + transform.TransformDirection(Vector3.down));
		RayCastHelper(transform.TransformDirection(Vector3.left) + transform.TransformDirection(Vector3.down));


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

		transform.Translate(0, 0, Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime);

		//transform.Rotate(0, x * 90 * Time.deltaTime, 0);

		//v += transform.forward* z;
		////v += transform.right* x;
		//v += transform.up* y;

		//rb.velocity = v;
	}

	private void RayCastHelper(Vector3 direction)
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, direction, out hit, 2))
			averageNormalDirections.Add(hit.normal);
	}

	private void FixedUpdate()
	{
		// apply constant weight force according to character normal:
		rb.AddForce(-9.8f * rb.mass * transform.up);
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
