using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateButt : MonoBehaviour
{
    public GrapplingWeb grappling;
    Quaternion desiredRotation;
    float rotationSpeed = 5f;

    private void Update()
    {
        if(!grappling.IsGrappling())
        {
            desiredRotation = Quaternion.LookRotation(grappling.GetGrapplePoint() - transform.position);
            //return;
        }
        else
        {
            desiredRotation = Quaternion.LookRotation(grappling.GetGrapplePoint() - transform.position);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
        //transform.LookAt(grappling.GetGrapplePoint());
    }
}
