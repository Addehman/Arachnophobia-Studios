using Cinemachine;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
	[SerializeField] private Transform cameraTarget;
	[SerializeField] private Transform targetToRotate;
	[SerializeField] private float mouseRotationSpeed = 1f;
	[SerializeField] private float gamepadRotationSpeed = 10f;
	[SerializeField] private float smoothTime = 10f;
	[SerializeField] private float minZoom = 0.1f;
	[SerializeField] private float maxZoom = 0.5f;
	
	private CinemachineVirtualCamera aimCamera;
	private CinemachineVirtualCamera cameraToZoom;
	private CinemachineComponentBase zoomCameraComponentBase;
	private CinemachineComponentBase tpCameraComponentBase;
	private HookWeb hookWeb;
	private ClimbWeb climbWeb;
	private SpringJointWeb springJointWeb;
	private Transform cameraParent;
	private Vector3 currentVelocity;
	private float cameraInputX;
	private float cameraInputY;
	private float lerpSpeed = 10f;
	private bool doLockCameraInput = false;


	private void Awake()
	{
		cameraParent = transform.parent;
		hookWeb = FindObjectOfType<HookWeb>();
		hookWeb.LockTPCameraRotation += HookWebRecenterCamera;
		climbWeb = FindObjectOfType<ClimbWeb>();
		climbWeb.CameraStartRotation += BeginClimbRotation;
		climbWeb.CameraEndRotation += RecenterCamera;
		springJointWeb = FindObjectOfType<SpringJointWeb>();
		springJointWeb.RecenterCamera += RecenterCamera;
	}
	
	private void Start()
	{
		cameraToZoom = GetComponent<CinemachineVirtualCamera>();
		aimCamera = GameObject.Find("cmAimCamera").GetComponent<CinemachineVirtualCamera>();

		zoomCameraComponentBase = cameraToZoom.GetCinemachineComponent(CinemachineCore.Stage.Body);
		if (zoomCameraComponentBase is CinemachineFramingTransposer)
		{
			(zoomCameraComponentBase as CinemachineFramingTransposer).m_CameraDistance = 0.3f;
		}

		tpCameraComponentBase = aimCamera.GetCinemachineComponent(CinemachineCore.Stage.Aim);

		
	}
	
	private void LateUpdate()
	{
		if (doLockCameraInput == false)
		{
			CamControl();
		}
	}

	private void CamControl()
	{
		float mouseInput = Mathf.Abs(Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y"));
		if (mouseInput == 0f)
		{
			cameraInputX += Input.GetAxis("CameraInputX") * gamepadRotationSpeed;
			cameraInputY += Input.GetAxis("CameraInputY") * gamepadRotationSpeed;
			if (tpCameraComponentBase is CinemachinePOV)
			{
				(tpCameraComponentBase as CinemachinePOV).m_VerticalAxis.m_InvertInput = false;
			}
		}
		else
		{
			cameraInputX += Input.GetAxis("CameraInputX") * mouseRotationSpeed;
			cameraInputY -= Input.GetAxis("CameraInputY") * mouseRotationSpeed;
			if (tpCameraComponentBase is CinemachinePOV)
			{
				(tpCameraComponentBase as CinemachinePOV).m_VerticalAxis.m_InvertInput = true;
			}
		}
		
		// cameraParent.localRotation = Quaternion.Euler(cameraInputY, cameraInputX, 0f);
		// transform.localRotation = Quaternion.identity;
		cameraTarget.localRotation = Quaternion.Euler(cameraInputY, cameraInputX, 0f);
		targetToRotate.localRotation = Quaternion.Euler(0f, cameraInputX, 0f);


		if (zoomCameraComponentBase is CinemachineFramingTransposer)
		{
			float cameraDistance = (zoomCameraComponentBase as CinemachineFramingTransposer).m_CameraDistance -= Input.GetAxis("Mouse ScrollWheel") + Input.GetAxis("Zoom");
			float zoomValue = Mathf.Clamp(cameraDistance, minZoom, maxZoom);
			
			(zoomCameraComponentBase as CinemachineFramingTransposer).m_CameraDistance = zoomValue;
			// (componentBase as CinemachineFramingTransposer).m_TrackedObjectOffset = 
		}
	}

	public void HookWebRecenterCamera(bool isActive)
	{
		// Lock CameraInput:
		doLockCameraInput = isActive;

		cameraInputX = 0f;
		cameraInputY = 0f;
	}

	public void RecenterCamera()
	{
		cameraInputX = 0f;
		cameraInputY = 0f;
	}

	private void BeginClimbRotation()
	{
		cameraInputX = 0f;
		cameraInputY = climbWeb.transform.position.y + 45f;
	}

	// private void FollowMimicCamera(bool isActive)
	// {
	// 	if (isActive == false)
	// 	{
	// 		// cameraParent.localRotation = cameraToZoom.transform.rotation;
	// 		transform.localRotation = cameraToZoom.transform.rotation;
	// 	}
	// }

	private void OnDestroy()
	{
		hookWeb.LockTPCameraRotation -= HookWebRecenterCamera;
		climbWeb.CameraStartRotation -= BeginClimbRotation;
		climbWeb.CameraEndRotation -= RecenterCamera;
		springJointWeb.RecenterCamera -= RecenterCamera;
	}
}
