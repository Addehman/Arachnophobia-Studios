using System;
using System.Collections.Generic;
using UnityEngine;

public class HookWeb : MonoBehaviour
{
	[SerializeField] Transform debugHitPointTransform;
	public GameObject webStartPosition;
	public float speed = 1f;

	public event Action DisableFPSCamera;

	ThirdPersonCameraController tpcController;
	SpiderMovement spiderMovement;
	State currentState;

	Vector3 hookShotPosition;
	Vector3 newTransformUp;
	Vector3 previousTransformUp;
	Vector3 oldPosition;

	enum State
	{
		Normal,
		HookFlying,
	}
	void Start()
	{
		spiderMovement = GetComponent<SpiderMovement>();
		tpcController = FindObjectOfType<ThirdPersonCameraController>();
	}

	void Update()
	{
		//Debug.Log(Vector3.Distance(transform.position, hookShotPosition));
		if (currentState == State.Normal)
		{
			HandleHookShotStart();
		}

		if (currentState == State.HookFlying)
		{
			HandleHookShotMovement();
		}
	}

	void HandleHookShotStart()
	{
		if (Input.GetButtonDown("HookShotWeb"))
		{
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit raycastHit))
			{
				previousTransformUp = transform.up;
				newTransformUp = raycastHit.normal;

				Debug.Log(newTransformUp);

				debugHitPointTransform.position = raycastHit.point;
				hookShotPosition = raycastHit.point;
				oldPosition = transform.position;

				currentState = State.HookFlying;
				if (DisableFPSCamera != null)
				{
					DisableFPSCamera();
				}
			}
		}
	}

	void HandleHookShotMovement()
	{
		//   Vector3 hookShotDirection = (hookShotPosition - transform.position).normalized;
		oldPosition = transform.position;
		float hookShotSpeed = Vector3.Distance(oldPosition, hookShotPosition);

		spiderMovement.gravityValue = 0f;

		transform.position = Vector3.Lerp(oldPosition, hookShotPosition, speed * Time.deltaTime);

		transform.up = newTransformUp;

		tpcController.RecenterCamera();

		//transform.up = Vector3.Lerp(previousTransformUp, newTransformUp, speed);

		if (Vector3.Distance(transform.position, hookShotPosition) < 0.02f || spiderMovement.debugSettings.isGrounded == true && Vector3.Distance(transform.position, hookShotPosition) < 0.1f)
		{
			spiderMovement.gravityValue = -9.82f;
			currentState = State.Normal;
		}
	}
}
