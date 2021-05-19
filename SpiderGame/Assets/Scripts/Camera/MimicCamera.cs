using Cinemachine;
using UnityEngine;

public class MimicCamera : MonoBehaviour
{
	[SerializeField] private Transform cameraToMimic;
	[SerializeField] private float positionLerpSpeed = 10f;
	[SerializeField] private float rotationLerpSpeed = 10f;

	public bool doLockRotation = false;

	private Vector3 currentVelocity;
	private Quaternion lockedRotation;
	private HookWeb hookWeb;


	private void Start()
	{
		hookWeb = FindObjectOfType<HookWeb>();
		hookWeb.LockTPCameraRotation += LockRotation;
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

	public void LockRotation(bool isActive)
	{
		if (isActive == true)
		{
			lockedRotation = transform.rotation;
		}

		doLockRotation = isActive;
	}
}
