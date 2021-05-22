using Cinemachine;
using UnityEngine;

public class CameraSoftFollow : MonoBehaviour
{
	[SerializeField] private Transform cameraToMimic;
	[SerializeField] private float positionLerpSpeed = 10f;
	[SerializeField] private float rotationLerpSpeed = 10f;
	[SerializeField] private SpiderMovement spiderMovement;

	public CinemachineCollider cmColl;

	private Vector3 currentVelocity;

	
	private void Start()
	{
		cmColl = GetComponent<CinemachineCollider>();
		spiderMovement.cameraChangeStrategy += CameraChangeStrategy;
	}

	private void LateUpdate()
	{
		transform.position = Vector3.SmoothDamp(transform.position, cameraToMimic.position, ref currentVelocity, positionLerpSpeed * Time.deltaTime);
		transform.rotation = Quaternion.Slerp(transform.rotation, cameraToMimic.rotation, rotationLerpSpeed * Time.deltaTime);
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
