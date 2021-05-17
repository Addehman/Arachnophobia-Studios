using Cinemachine;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera cameraToZoom;
	[SerializeField] private float rotationSpeed = 1f;
	[SerializeField] private float smoothTime = 10f;
	[SerializeField] private float minZoom = 0.1f;
	[SerializeField] private float maxZoom = 0.5f;
	
	private CinemachineComponentBase componentBase;
	private Transform cameraParent;
	private Vector3 currentVelocity;
	private float mouseX;
	private float mouseY;
	private float lerpSpeed = 10f;


	private void Start()
	{
		cameraParent = transform.parent;

		componentBase = cameraToZoom.GetCinemachineComponent(CinemachineCore.Stage.Body);
		if (componentBase is CinemachineFramingTransposer)
		{
			(componentBase as CinemachineFramingTransposer).m_CameraDistance = 0.3f;
		}
	}
	
	void LateUpdate()
	{
		CamControl();
	}

	private void CamControl()
	{
		mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
		mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
		// Possibly Clamp mouseY

		// Vector3 myForward = Vector3.Cross(cameraParent.forward, followTarget.up);
		// Quaternion targetRot = Quaternion.LookRotation(myForward, followTarget.up);
		// cameraParent.rotation = Quaternion.Slerp(cameraParent.rotation, targetRot, lerpSpeed * Time.deltaTime);
		// cameraParent.rotation = Quaternion.Euler(targetRot.eulerAngles.x + mouseY, targetRot.eulerAngles.y + mouseX, 0f);

		cameraParent.localRotation = Quaternion.Euler(mouseY, mouseX, 0f);
		// cameraParent.position = Vector3.SmoothDamp(cameraParent.position, followTarget.position, ref currentVelocity, smoothTime);

		if (componentBase is CinemachineFramingTransposer)
		{
			float cameraDistance = (componentBase as CinemachineFramingTransposer).m_CameraDistance -= Input.GetAxis("Mouse ScrollWheel");
			float zoomValue = Mathf.Clamp(cameraDistance, minZoom, maxZoom);
			
			(componentBase as CinemachineFramingTransposer).m_CameraDistance = zoomValue;
			// (componentBase as CinemachineFramingTransposer).m_TrackedObjectOffset = 
		}
	}
}
