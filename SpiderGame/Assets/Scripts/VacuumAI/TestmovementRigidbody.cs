using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestmovementRigidbody : MonoBehaviour
{
    Rigidbody rb;

    public float speed;
    public float rotationSpeed = 5;

    float horizontal;
    float vertical;
    bool jump;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        jump = Input.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
        rb.velocity = (transform.forward * vertical) * speed * Time.fixedDeltaTime;
        transform.Rotate((transform.up * horizontal) * rotationSpeed * Time.fixedDeltaTime);

        //if (vertical > 0.01f)
        //{
        //    rb.AddForce(transform.forward * vertical * Time.deltaTime * speed);
        //}
        //else if (vertical < -0.01f)
        //{
        //    rb.AddForce(-transform.forward * vertical * Time.deltaTime * speed);
        //}

        //if (horizontal > 0.01f)
        //{
        //    rb.AddForce(transform.right * horizontal * Time.deltaTime * speed);
        //}
        //else if (horizontal < -0.01f)
        //{
        //    rb.AddForce(-transform.right * horizontal * Time.deltaTime * speed);
        //}

        //if (vertical == 0 || horizontal == 0)
        //{
        //    rb.velocity = Vector3.zero;
        //}
        //if (horizontal != 0f)
        //{
        //    rb.AddForce(Vector3.right * horizontal * speed * Time.deltaTime);
        //}

        //if (vertical != 0f)
        //{
        //    rb.AddForce(Vector3.forward * vertical * speed * Time.deltaTime);
        //}

        //if (jump)
        //{
        //    rb.AddForce(Vector3.up);
        //}
    }
}
