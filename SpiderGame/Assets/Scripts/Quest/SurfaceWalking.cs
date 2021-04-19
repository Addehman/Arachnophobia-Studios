using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SurfaceWalking : MonoBehaviour
{
    float speed = 1f;
    Rigidbody rb;

    RaycastHit hit;
    RaycastHit headHit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
      //  CheckSurfaceSteps();
    }

    void FixedUpdate()
    {
        if(rb.velocity.magnitude < speed)
        {
            float value = Input.GetAxis("Vertical");

            if(value != 0)
            {
                rb.AddForce(0, 0, value * Time.fixedDeltaTime * 1000f);
            }
        }
    }

    void CheckSurfaceSteps()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.red, 1.0f);
        Debug.DrawRay(transform.position + new Vector3(0, 0.4f, 0), transform.forward, Color.red, 1.0f);

        if(Physics.Raycast(transform.position, transform.forward, out hit, 1.0f) && !Physics.Raycast(transform.position + new Vector3(0,0.4f,0), transform.forward, out headHit, 1.0f))
        {
            Debug.Log("Surface alert");
            if(Vector3.Dot(Vector3.up, hit.normal) > 0.7f)
            {
                rb.GetComponent<Rigidbody>().AddForce(transform.up, ForceMode.VelocityChange);
            }
        }
    }
}
