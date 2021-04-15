using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumMovement : MonoBehaviour
{
    Rigidbody rb;
    Vector3 eulerAngleVelocity;

    public bool isReversing;

    public Vector3 moveForward;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        eulerAngleVelocity = new Vector3(0, -10, 0);
    }

    void FixedUpdate()
    {
        if (!isReversing)
        {
            rb.velocity = transform.forward;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Krock");

        if (other.CompareTag("Wall"))
        {
            StartCoroutine(RandomizeDirection());
        }
    }

    IEnumerator RandomizeDirection()
    {
        isReversing = true;

        float timePassed = 0;

        float reverseTime = 0.5f;
        int rotationTime = Random.Range(4, 7);

        if (Random.Range(0, 2) == 1)
        {
            eulerAngleVelocity *= -1;
        }

        while (timePassed < reverseTime)
        {
            rb.velocity = -transform.forward;

            timePassed += Time.deltaTime;

            yield return null;
        }

        while (timePassed >= reverseTime && timePassed < rotationTime)
        {
            Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);

            timePassed += Time.deltaTime;

            yield return null;
        }

        isReversing = false;
    }
}
