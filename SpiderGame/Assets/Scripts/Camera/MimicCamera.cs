using Cinemachine;
using UnityEngine;

public class MimicCamera : MonoBehaviour
{
	[SerializeField] private float positionLerpSpeed = 10f;
	[SerializeField] private float rotationLerpSpeed = 10f;
	[SerializeField] private float activeSwingDamping = 0.1f;
	[SerializeField] private float inactiveSwingDamping = 1f;

	public bool doLockRotation = false;

	private SpiderMovement spiderMovement;
	private CinemachineCollider cmColl;
	private CinemachineComponentBase componentBase;
	private HookWeb hookWeb;
	private SpringJointWeb springJointWeb;
	private Transform cameraToMimic;
	private Vector3 currentVelocity;
	private Quaternion lockedRotation;


	private void Awake()
	{
		spiderMovement = FindObjectOfType<SpiderMovement>();
		spiderMovement.cameraChangeStrategy += CameraChangeStrategy;
		cameraToMimic = FindObjectOfType<ThirdPersonCameraController>().transform;
		cmColl = GetComponent<CinemachineCollider>();
		hookWeb = FindObjectOfType<HookWeb>();
		hookWeb.LockTPCameraRotation += LockRotation;
		springJointWeb = FindObjectOfType<SpringJointWeb>();
		// springJointWeb.SwitchToSwingCamera += LockRotation;
		// springJointWeb.SetCameraDampingForSwing += SetCameraDampingForSwing;

		transform.position = cameraToMimic.position;
		transform.rotation = cameraToMimic.rotation;
	}

	private void Start()
	{
		CinemachineVirtualCamera thisCmVirtualCamera = GetComponent<CinemachineVirtualCamera>();
		componentBase = thisCmVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
	}

	private void Update()
	{
		// transform.position = Vector3.SmoothDamp(transform.position, cameraToMimic.position, ref currentVelocity, 1f);
		// transform.position = cameraToMimic.position;

		if (doLockRotation == true)
		{
			transform.rotation = lockedRotation;
		}
		else
		{
			// transform.rotation = Quaternion.Slerp(transform.rotation, cameraToMimic.rotation, 1f);
			// transform.rotation = cameraToMimic.rotation;
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

	private void SetCameraDampingForSwing(bool isSwingActive)
	{
		if (isSwingActive == true)
		{
			if (componentBase is CinemachineHardLockToTarget)
			{
				(componentBase as CinemachineHardLockToTarget).m_Damping = activeSwingDamping;
			}
		}
		else
		{
			if (componentBase is CinemachineHardLockToTarget)
			{
				(componentBase as CinemachineHardLockToTarget).m_Damping = inactiveSwingDamping;
			}
		}
	}

	private void OnDestroy()
	{
		spiderMovement.cameraChangeStrategy -= CameraChangeStrategy;
		hookWeb.LockTPCameraRotation -= LockRotation;
		springJointWeb.SwitchToSwingCamera -= LockRotation;
		springJointWeb.SetCameraDampingForSwing -= SetCameraDampingForSwing;
	}
}
