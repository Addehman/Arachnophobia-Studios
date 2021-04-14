using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringJointWeb : MonoBehaviour
{
    public GameObject targetPointPrefab;
    float maxDistance = 100f;
    SpringJoint joint;

    enum State
    {
        IsGrounded,
        IsSwinging,
        IsHanging,
        IsLanding
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartWebGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopWebGrapple();
        }
    }

    void StartWebGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        {
            /*
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple keeps from the point
            joint.minDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            webRenderer.positionCount = 2;
            */

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

    void StopWebGrapple()
    {
      //  Destroy(joint);
    }
}
