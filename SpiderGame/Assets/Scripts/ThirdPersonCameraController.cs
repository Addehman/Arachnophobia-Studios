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
	
	private CinemachineVirtualCamera aimTPCamera;
	private CinemachineVirtualCamera cameraToZoom;
	private CinemachineComponentBase zoomCameraComponentBase;
	private CinemachineComponentBase tpCameraAimComponentBase;
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
		hookWeb.LockTPCameraRotation += LockCameraInput;
		hookWeb.RecenterCamera += RecenterCamera;
		climbWeb = FindObjectOfType<ClimbWeb>();
		climbWeb.CameraStartRotation += BeginClimbRotation;
		climbWeb.RecenterCamera += RecenterCamera;
		springJointWeb = FindObjectOfType<SpringJointWeb>();
		springJointWeb.RecenterCamera += RecenterCamera;
		// springJointWeb.SwitchToSwingCamera += LockCameraInput;
	}
	
	private void Start()
	{
		cameraToZoom = GetComponent<CinemachineVirtualCamera>();
		aimTPCamera = GameObject.Find("cmAimCamera").GetComponent<CinemachineVirtualCamera>();

		zoomCameraComponentBase = cameraToZoom.GetCinemachineComponent(CinemachineCore.Stage.Body);
		if (zoomCameraComponentBase is CinemachineFramingTransposer)
		{
			(zoomCameraComponentBase as CinemachineFramingTransposer).m_CameraDistance = 0.3f;
		}

		tpCameraAimComponentBase = aimTPCamera.GetCinemachineComponent(CinemachineCore.Stage.Aim);
	}
	
	private void Update()
	{
		if (doLockCameraInput == false)
		{
			CamControl();
		}

		if (Input.GetKeyDown(KeyCode.K))
		{
			RecenterCamera();
		}
	}

	private void CamControl()
	{
		if (doLockCameraInput == false)
		{
			Vector3 mouseInput = new Vector3(Input.GetAxis("Mouse X"), 0f, Input.GetAxis("Mouse Y"));
			if (mouseInput.sqrMagnitude == 0f)
			{
				cameraInputX += Input.GetAxis("CameraInputX") * gamepadRotationSpeed;
				cameraInputY += Input.GetAxis("CameraInputY") * gamepadRotationSpeed;
				if (tpCameraAimComponentBase is CinemachinePOV)
				{
					(tpCameraAimComponentBase as CinemachinePOV).m_VerticalAxis.m_InvertInput = false;
				}
			}
			else
			{
				cameraInputX += Input.GetAxis("CameraInputX") * mouseRotationSpeed;
				cameraInputY -= Input.GetAxis("CameraInputY") * mouseRotationSpeed;
				if (tpCameraAimComponentBase is CinemachinePOV)
				{
					(tpCameraAimComponentBase as CinemachinePOV).m_VerticalAxis.m_InvertInput = true;
				}
			}
			// cameraParent.localRotation = Quaternion.Euler(cameraInputY, cameraInputX, 0f);
			// transform.localRotation = Quaternion.identity;
			cameraInputY = Mathf.Clamp(cameraInputY, -10, 80);
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
		else
		{
			print ("setting cameraTarget Up");
			cameraTarget.parent.transform.up = Vector3.up;
		}
		
	}

	public void LockCameraInput(bool isActive)
	{
		// Lock CameraInput:
		doLockCameraInput = isActive;
	}

	public void RecenterCamera()
	{
		cameraInputX = 0f;
		cameraInputY = 0f;

		// transform.rotation = Quaternion.identity;
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
		hookWeb.LockTPCameraRotation -= LockCameraInput;
		climbWeb.CameraStartRotation -= BeginClimbRotation;
		climbWeb.RecenterCamera -= RecenterCamera;
		springJointWeb.RecenterCamera -= RecenterCamera;
	}
}
