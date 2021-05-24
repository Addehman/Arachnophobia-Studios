using System;
using UnityEngine;

public class SpringJointWeb : MonoBehaviour
{
	[HideInInspector] public bool isSwingingWeb = false;

	public event Action ExitFPCamera;
	public event Action RecenterCamera;
	public event Action<bool> LockTPCameraRotation;

	float maxDistance = 100f;

	SpringJoint joint;
	SpiderAudio spiderAudio;
	LineRenderer lineRenderer;
	public SwingState currentState = SwingState.IsGrounded;

	public Animator spiderAnimator;
	private ToggleCameras toggleCameras;
	public DebugSettings debugSetting;

	public GameObject firstPersonCamera;
	public GameObject targetPointPrefab;
	public GameObject butt;

	public bool isReleased = true;

	private GameObject targetCloneHolder;
	

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void Start()
	{
		spiderAudio = GetComponent<SpiderAudio>();
		toggleCameras = Camera.main.GetComponent<ToggleCameras>();
	}

	private void Update()
	{

		if(toggleCameras.boosted == true)
		{
			if (Input.GetButtonDown("UseWeb") || Input.GetAxis("UseWeb") > 0f)
			{
				StartWebGrapple();
				isReleased = false;
			}
			else if ((Input.GetButtonUp("UseWeb") || Input.GetAxis("UseWeb") <= 0f) && isReleased == false)
			{
				StopWeb();
				isReleased = true;
			}
		}

		if ((Input.GetButtonUp("UseWeb") || Input.GetAxis("UseWeb") <= 0f) && isReleased == false)
		{
			StopWeb();
			isReleased = true;
		}

	}

	private void LateUpdate()
	{
		DrawString();
	}

	private void OnDisable()
	{
		StopWeb(false);
	}

	void StartWebGrapple()
	{
		RaycastHit hit;
		if (Physics.Raycast(butt.transform.position, Camera.main.transform.forward, out hit, maxDistance))
		{
			spiderAudio.WebShoot();

			isSwingingWeb = true;
			//spiderAnimator.SetBool("Web", true);
			currentState = SwingState.IsSwinging;
			GameObject targetPoint = Instantiate(targetPointPrefab, hit.point, Quaternion.identity);
			targetCloneHolder = targetPoint;
			joint = gameObject.AddComponent<SpringJoint>();
			joint.connectedBody = targetPoint.GetComponent<Rigidbody>();

			joint.spring = 120f;
			joint.damper = 75f;
			joint.massScale = 3f;
			joint.autoConfigureConnectedAnchor = false;
			joint.anchor = new Vector3(0f, 0f, 0f);
			joint.connectedAnchor = new Vector3(0f, 0f, 0f);

			if (ExitFPCamera != null)
			{
				ExitFPCamera();
			}
			if (LockTPCameraRotation != null)
			{
				LockTPCameraRotation(true);
			}
		}
	}

	void StopWeb(bool recenterCamera = true)
	{
		// spiderAnimator.SetBool("Web", false);
		debugSetting.isGrounded = true;
		currentState = SwingState.IsGrounded;
		isSwingingWeb = false;
		// GameObject currentPoint = GameObject.Find("TargetPoint(Clone)");
		Destroy(targetCloneHolder);
		Destroy(joint);
		lineRenderer.enabled = false;

		if (LockTPCameraRotation != null)
		{
			LockTPCameraRotation(false);
		}
		if (RecenterCamera != null && recenterCamera == true)
		{
			RecenterCamera();
		}
	}

	void DrawString()
	{
		if (!joint)
		{
			return;
		}

		lineRenderer.SetPosition(0, butt.transform.position);
		lineRenderer.SetPosition(1, targetCloneHolder.transform.position);
		lineRenderer.enabled = true;
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.CompareTag("Ground"))
		{
			currentState = SwingState.IsGrounded;
		}
	}
}

public enum SwingState
{
	IsGrounded,
	IsSwinging,
	IsHanging,
	IsLanding
}
