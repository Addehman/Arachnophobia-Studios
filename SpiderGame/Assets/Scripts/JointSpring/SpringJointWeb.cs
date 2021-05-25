using System;
using UnityEngine;

public class SpringJointWeb : MonoBehaviour
{
	[HideInInspector] public bool isSwingingWeb = false;

	public event Action ExitFPCamera;
	public event Action RecenterCamera;
	public event Action<bool> SwitchToSwingCamera;
	public event Action<bool> SetCameraDampingForSwing;

	public GameObject firstPersonCamera;
	public GameObject targetPointPrefab;
	public GameObject butt;
	public SwingState currentState = SwingState.IsGrounded;
	public Animator spiderAnimator;
	public SpiderMovement spiderMovement;
	public bool isReleased = true;

	private GameObject targetCloneHolder;
	private ToggleCameras toggleCameras;
	private SpringJoint joint;
	private SpiderAudio spiderAudio;
	private LineRenderer lineRenderer;
	private float maxDistance = 100f;
	private bool hasFlaggedOnce = true;
	

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void Start()
	{
		spiderMovement = FindObjectOfType<SpiderMovement>();
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

				if (SetCameraDampingForSwing != null)
				{
					SetCameraDampingForSwing(true);
				}
				hasFlaggedOnce = false;
			}
		}

		if ((Input.GetButtonUp("UseWeb") || Input.GetAxis("UseWeb") <= 0f) && isReleased == false)
		{
			StopWeb();
			isReleased = true;

			// if (SetCameraDampingForSwing != null)
			// {
			// 	SetCameraDampingForSwing(true);
			// }
			hasFlaggedOnce = false;
		}
	// Set MimicCamera's Damping back to default, and do it only one time per use of Swing.
		if (spiderMovement.debugSettings.isGrounded == true && hasFlaggedOnce == false && currentState == SwingState.IsGrounded)
		{
			if (SwitchToSwingCamera != null)
			{
				SwitchToSwingCamera(false);
			}
			hasFlaggedOnce = true;
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

	private void StartWebGrapple()
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
			if (SwitchToSwingCamera != null)
			{
				SwitchToSwingCamera(true);
			}
		}
	}

	private void StopWeb(bool recenterCamera = true)
	{
		// spiderAnimator.SetBool("Web", false);
		// debugSetting.isGrounded = true;
		currentState = SwingState.IsGrounded;
		isSwingingWeb = false;
		// GameObject currentPoint = GameObject.Find("TargetPoint(Clone)");
		Destroy(targetCloneHolder);
		Destroy(joint);
		lineRenderer.enabled = false;

		if (RecenterCamera != null && recenterCamera == true)
		{
			RecenterCamera();
		}
		// if (SetCameraDampingForSwing != null)
		// {
		// 	SetCameraDampingForSwing(false);
		// }
	}

	private void DrawString()
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
