using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetController : MonoBehaviour
{
	[SerializeField] private float rayMaxDistance = 0.2f;
	[SerializeField] private LayerMask layerMask;

	private GameObject parentObject;
	private Transform playerTransform;
	private Transform cameraTransform;
	private Vector3 direction;
	private Vector3 fwdRayNormal;
	private Vector3 downRayNormal;
	private bool isFwdRayHitting = false;


	private void Start()
	{
		parentObject = transform.parent.gameObject;
		playerTransform = FindObjectOfType<SpiderMovement>().transform;
		cameraTransform = Camera.main.transform;
	}
	
	private void Update()
	{
		float hor = Input.GetAxis("Horizontal");
		float ver = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(hor, 0f, ver);

		transform.localPosition = movement.normalized * 0.1f;

		// direction = transform.position - parentObject.transform.position;

		// transform.LookAt(parentObject.transform, parentObject.transform.up); // Check this one out for opimization later! <------------- OBS!
		// transform.forward = -transform.forward;

		if (movement.sqrMagnitude == 0)
		{
			return;
		}
		
		// transform.forward = movement;

		// transform.rotation.SetLookRotation(-parentObject.transform.position, parentObject.transform.up);

		// transform.rotation = Quaternion.LookRotation(-parentObject.transform.forward, parentObject.transform.up);


		// SetParentPositionToPlayer();

		RaycastForward();
		RaycastDown();

		// transform.rotation = Quaternion.Euler(transform.eulerAngles.x, fwdRayNormal.y, transform.eulerAngles.z);

		if (isFwdRayHitting == true) // or maybe simply check if the magnitude of the fwdRayNormal is above 0, but in that case make sure to set the fwdRayNormal to zero if no hit.
		{
			transform.up = fwdRayNormal;
		}
		else 
		{
			transform.up = downRayNormal;
		}

		// transform.up = fwdRayNormal;
	}

	private void SetParentPositionToPlayer()
	{
		parentObject.transform.position = playerTransform.position;
		parentObject.transform.up = playerTransform.up;
		parentObject.transform.rotation = Quaternion.Euler(parentObject.transform.eulerAngles.x, cameraTransform.eulerAngles.y, parentObject.transform.eulerAngles.z); // Believe this is an issue <----- Look here!
		// There's something about the conversion on the Y-axis that doesn't follow over when on the wall.
	}

	private void RaycastForward()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, rayMaxDistance, layerMask))
		{
			// transform.up = hit.normal;
			fwdRayNormal = hit.normal;
			isFwdRayHitting = true;
		}
		else
		{
			isFwdRayHitting = false;
		}
		
		Debug.DrawRay(transform.position, transform.forward.normalized * rayMaxDistance, Color.green, 0.5f);
	}

	private void RaycastDown()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -transform.up, out hit, rayMaxDistance, layerMask))
		{
			downRayNormal = hit.normal;
		}

		Debug.DrawRay(transform.position, -transform.up.normalized * rayMaxDistance, Color.blue, 0.5f);
	}
}