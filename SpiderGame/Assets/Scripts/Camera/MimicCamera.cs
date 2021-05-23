using Cinemachine;
using UnityEngine;

public class MimicCamera : MonoBehaviour
{
	[SerializeField] private float positionLerpSpeed = 10f;
	[SerializeField] private float rotationLerpSpeed = 10f;

	public bool doLockRotation = false;

	private SpiderMovement spiderMovement;
	private CinemachineCollider cmColl;
	private HookWeb hookWeb;
	private SpringJointWeb springJointWeb;
	private Transform cameraToMimic;
	private Vector3 currentVelocity;
	private Quaternion lockedRotation;


	private void Start()
	{
		spiderMovement = FindObjectOfType<SpiderMovement>();
		spiderMovement.cameraChangeStrategy += CameraChangeStrategy;
		cameraToMimic = FindObjectOfType<ThirdPersonCameraController>().transform;
		cmColl = GetComponent<CinemachineCollider>();
		hookWeb = FindObjectOfType<HookWeb>();
		hookWeb.LockTPCameraRotation += LockRotation;
		springJointWeb = FindObjectOfType<SpringJointWeb>();
		springJointWeb.LockTPCameraRotation += LockRotation;

		transform.position = cameraToMimic.position;
		transform.rotation = cameraToMimic.rotation;
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

	private void CameraChangeStrategy(bool doChange)
	{
		if (doChange == true)
		{
			cmColl.m_Strategy = CinemachineCollider.ResolutionStrategy.PreserveCameraDistance;
		}
		else
		{
			cmColl.m_Strategy = CinemachineCollider.ResolutionStrategy.PullCameraForward;
		}
	}
}
