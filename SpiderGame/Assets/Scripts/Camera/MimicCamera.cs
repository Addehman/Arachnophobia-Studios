using Cinemachine;
using UnityEngine;

public class MimicCamera : MonoBehaviour
{
	[SerializeField] private float positionLerpSpeed = 10f;
	[SerializeField] private float rotationLerpSpeed = 10f;

	public bool doLockRotation = false;

	private Transform cameraToMimic;
	private HookWeb hookWeb;
	private Vector3 currentVelocity;
	private Quaternion lockedRotation;


	private void Start()
	{
		cameraToMimic = FindObjectOfType<ThirdPersonCameraController>().transform;
		hookWeb = FindObjectOfType<HookWeb>();
	}

	private void LateUpdate()
	{
		transform.position = Vector3.SmoothDamp(transform.position, cameraToMimic.position, ref currentVelocity, positionLerpSpeed * Time.deltaTime);

		if (doLockRotation == true)
		{
			transform.rotation = lockedRotation;
		}
		else
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, cameraToMimic.rotation, rotationLerpSpeed * Time.deltaTime);
		}
	}

	// This method and what it touches in LateUpdate() might not be necessary anymore, if we keep the climbWeb(HookWeb), 
	// but might be necessary in other areas.
	public void LockRotation(bool isActive)
	{
		if (isActive == true)
		{
			lockedRotation = transform.rotation;
		}

		doLockRotation = isActive;
	}
}
