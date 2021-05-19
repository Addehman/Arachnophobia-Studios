using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringJointWeb : MonoBehaviour
{
    float maxDistance = 100f;

    SpringJoint joint;
    SpiderAudio spiderAudio;
    LineRenderer lineRenderer;
    State currentState = State.IsGrounded;

    private ToggleCameras toggleCameras;
    public DebugSettings debugSetting;
    public Animator spiderAnimator;

    public GameObject thirdPersonCamera;
    public GameObject firstPersonCamera;
    public GameObject targetPointPrefab;
    public GameObject butt;

    public bool isSwingingWeb = false;

    enum State
    {
        IsGrounded,
        IsSwinging,
        IsHanging,
        IsLanding
    }

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
            if (Input.GetButtonDown("SwingWeb") || Input.GetAxis("SwingWeb") > 0f)
            {
                StartWebGrapple();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                StopWeb();
            }
        }

        if (Input.GetButtonUp("SwingWeb") || Input.GetAxis("SwingWeb") <= 0f)
        {
            StopWeb();
        }
    }

    private void LateUpdate()
    {
        DrawString();
    }

    void StartWebGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(butt.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        {
            spiderAudio.WebShoot();

            isSwingingWeb = true;
  //          spiderAnimator.SetBool("Web", true);
            toggleCameras.DisableFPSCamera();
            currentState = State.IsSwinging;
            GameObject targetPoint = Instantiate(targetPointPrefab, hit.point, Quaternion.identity);
            joint = gameObject.AddComponent<SpringJoint>();
            joint.connectedBody = targetPoint.GetComponent<Rigidbody>();

            joint.spring = 120f;
            joint.damper = 75f;
            joint.massScale = 3f;
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = new Vector3(0f, 0f, 0f);
            joint.connectedAnchor = new Vector3(0f, 0f, 0f);
        }
    }

    void StopWeb()
    {
        // spiderAnimator.SetBool("Web", false);
        debugSetting.isGrounded = true;
        isSwingingWeb = false;
        GameObject currentPoint = GameObject.Find("TargetPoint(Clone)");
        Destroy(currentPoint);
        Destroy(joint);
        lineRenderer.enabled = false;
    }

    void DrawString()
    {
        if (!joint)
        {
            return;
        }

        lineRenderer.SetPosition(0, butt.transform.position);
        lineRenderer.SetPosition(1, GameObject.Find("TargetPoint(Clone)").transform.position);
        lineRenderer.enabled = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Ground"))
        {
            currentState = State.IsGrounded;
        }
    }
}
