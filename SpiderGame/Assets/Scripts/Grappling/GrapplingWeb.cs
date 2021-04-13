using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingWeb : MonoBehaviour
{
    LineRenderer webRenderer;
    Vector3 grapplePoint;
    public LayerMask grappleable;
    public Transform webGrip, camera, player;

    float maxDistance = 100f;
    SpringJoint joint;

    private void Awake()
    {
        webRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartWebGrapple();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            StopWebGrapple();
        }
    }

    private void LateUpdate()
    {
        DrawWeb();
    }

    void StartWebGrapple()
    {
        RaycastHit hit;
        if(Physics.Raycast(camera.position, camera.forward, out hit, maxDistance))
        {
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
        }
    }

    void DrawWeb()
    {
        //if not grappling, dont draw a web
        if(!joint)
        {
            return;
        }

        webRenderer.SetPosition(0, webGrip.position);
        webRenderer.SetPosition(1, grapplePoint);
    }

    void StopWebGrapple()
    {
        webRenderer.positionCount = 0;
        Destroy(joint);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
