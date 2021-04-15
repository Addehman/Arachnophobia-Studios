using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringJointWeb : MonoBehaviour
{
    public GameObject targetPointPrefab;
    float maxDistance = 100f;
    SpringJoint joint;
    LineRenderer lineRenderer;
    State currentState = State.IsGrounded;

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
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartWebGrapple();
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            StopWeb();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            StartSwingString();
        }
    }

    private void LateUpdate()
    {
        DrawString();
    }

    void StartWebGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        {
            currentState = State.IsSwinging;
            GameObject targetPoint = Instantiate(targetPointPrefab, hit.point, Quaternion.identity);
            joint = gameObject.AddComponent<SpringJoint>();
            joint.connectedBody = targetPoint.GetComponent<Rigidbody>();

            joint.spring = 40f;
            joint.damper = 20f;
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = new Vector3(0f, 0f, 0f);
            joint.connectedAnchor = new Vector3(0f, 0f, 0f);
        }
    }

    void StartSwingString()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        {
            currentState = State.IsSwinging;
            GameObject targetPoint = Instantiate(targetPointPrefab, hit.point, Quaternion.identity);
            joint = gameObject.AddComponent<SpringJoint>();
            joint.connectedBody = targetPoint.GetComponent<Rigidbody>();

            joint.spring = 5f;
            joint.damper = 50f;
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = new Vector3(0f, 0f, 0f);
            joint.connectedAnchor = new Vector3(0f, 0f, 0f);
            joint.massScale = 100f;
        }
    }

    void StopWeb()
    {
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

        lineRenderer.SetPosition(0, gameObject.transform.position);
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
