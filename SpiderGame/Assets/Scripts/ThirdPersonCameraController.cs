using Cinemachine;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
	[SerializeField] private Transform cameraTarget;
	[SerializeField] private Transform targetToRotate;
	[SerializeField] private float mouseRotationSpeed = 2f;
	[SerializeField] private float gamepadRotationSpeed = 7f;
	[SerializeField] private float smoothTime = 10f;
	[SerializeField] private float minZoom = 0.1f;
	[SerializeField] private float maxZoom = 0.5f;
	
	private CinemachineVirtualCamera aimCamera;
	private CinemachineVirtualCamera cameraToZoom;
	private CinemachineComponentBase zoomCameraComponentBase;
	private CinemachineComponentBase aimCameraComponentBase;
	//private HookWeb hookWeb;
	//private ClimbWeb climbWeb;
	private SpiderMovement spiderMovement;
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
		spiderMovement = FindObjectOfType<SpiderMovement>();
		//hookWeb = FindObjectOfType<HookWeb>();
		//hookWeb.LockTPCameraRotation += LockCameraInput;
		//hookWeb.RecenterCamera += RecenterCamera;
		//climbWeb = FindObjectOfType<ClimbWeb>();
		//climbWeb.CameraStartRotation += BeginClimbRotation;
		//climbWeb.RecenterCamera += RecenterCamera;
		springJointWeb = FindObjectOfType<SpringJointWeb>();
		springJointWeb.RecenterCamera += RecenterCamera;
		// springJointWeb.SwitchToSwingCamera += LockCameraInput;
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

		aimCameraComponentBase = aimCamera.GetCinemachineComponent(CinemachineCore.Stage.Aim);
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
			Vector3 gamepadInput = new Vector3(Input.GetAxis("RightStickX"), 0f, Input.GetAxis("RightStickY"));
			if (gamepadInput.sqrMagnitude > 0f)
			{
				cameraInputX += Input.GetAxis("CameraInputX") * gamepadRotationSpeed * Time.deltaTime;
				cameraInputY += Input.GetAxis("CameraInputY") * gamepadRotationSpeed * Time.deltaTime;
				if (aimCameraComponentBase is CinemachinePOV)
				{
					(aimCameraComponentBase as CinemachinePOV).m_VerticalAxis.m_InvertInput = false;
				}
			}
			else
			{
				cameraInputX += Input.GetAxisRaw("CameraInputX") * mouseRotationSpeed * Time.deltaTime;
				cameraInputY -= Input.GetAxisRaw("CameraInputY") * mouseRotationSpeed * Time.deltaTime;
				if (aimCameraComponentBase is CinemachinePOV)
				{
					(aimCameraComponentBase as CinemachinePOV).m_VerticalAxis.m_InvertInput = true;
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

	//private void BeginClimbRotation()
	//{
	//	cameraInputX = 0f;
	//	cameraInputY = climbWeb.transform.position.y + 45f;
	//}

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
		//hookWeb.LockTPCameraRotation -= LockCameraInput;
		//climbWeb.CameraStartRotation -= BeginClimbRotation;
		//climbWeb.RecenterCamera -= RecenterCamera;
		springJointWeb.RecenterCamera -= RecenterCamera;
	}
}
