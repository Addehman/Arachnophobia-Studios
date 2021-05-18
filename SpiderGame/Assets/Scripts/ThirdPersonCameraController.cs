using Cinemachine;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
	[SerializeField] private float mouseRotationSpeed = 1f;
	[SerializeField] private float gamepadRotationSpeed = 10f;
	[SerializeField] private float smoothTime = 10f;
	[SerializeField] private float minZoom = 0.1f;
	[SerializeField] private float maxZoom = 0.5f;
	
	private CinemachineVirtualCamera aimCamera;
	private CinemachineVirtualCamera cameraToZoom;
	private CinemachineComponentBase zoomCameraComponentBase;
	private CinemachineComponentBase tpCameraComponentBase;
	private Transform cameraParent;
	private Vector3 currentVelocity;
	private float cameraInputX;
	private float cameraInputY;
	private float lerpSpeed = 10f;


	private void Start()
	{
		cameraParent = transform.parent;

		cameraToZoom = GameObject.Find("cmMimicCamera").GetComponent<CinemachineVirtualCamera>();
		aimCamera = GameObject.Find("cmAimCamera").GetComponent<CinemachineVirtualCamera>();

		zoomCameraComponentBase = cameraToZoom.GetCinemachineComponent(CinemachineCore.Stage.Body);
		if (zoomCameraComponentBase is CinemachineFramingTransposer)
		{
			(zoomCameraComponentBase as CinemachineFramingTransposer).m_CameraDistance = 0.3f;
		}

		tpCameraComponentBase = aimCamera.GetCinemachineComponent(CinemachineCore.Stage.Aim);
	}
	
	void LateUpdate()
	{
		CamControl();
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
		
		cameraParent.localRotation = Quaternion.Euler(cameraInputY, cameraInputX, 0f);

		if (zoomCameraComponentBase is CinemachineFramingTransposer)
		{
			float cameraDistance = (zoomCameraComponentBase as CinemachineFramingTransposer).m_CameraDistance -= Input.GetAxis("Mouse ScrollWheel");
			float zoomValue = Mathf.Clamp(cameraDistance, minZoom, maxZoom);
			
			(zoomCameraComponentBase as CinemachineFramingTransposer).m_CameraDistance = zoomValue;
			// (componentBase as CinemachineFramingTransposer).m_TrackedObjectOffset = 
		}
	}
}
