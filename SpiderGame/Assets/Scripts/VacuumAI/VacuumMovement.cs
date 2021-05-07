using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumMovement : MonoBehaviour
{
    Rigidbody rb;

    Vector3 eulerAngleVelocity;

    private int rotationSpeed = 2;
    private float forwardSpeedMultiplier = 0.5f;
    private float reverseSpeedMultiplier = 0.4f;


    public Transform playerTransform;

    public bool randomizeDirectionInProgress;
    public bool playerInSight;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        eulerAngleVelocity = new Vector3(0, -50, 0);
    }

    void FixedUpdate()
    {
        if (playerInSight)
        {
            ChasePlayer();
            rb.velocity = transform.forward * forwardSpeedMultiplier;
        }

        if (!randomizeDirectionInProgress && !playerInSight)
        {
            rb.velocity = transform.forward * forwardSpeedMultiplier;
        }
    }

    public IEnumerator RandomizeDirection()
    {
        randomizeDirectionInProgress = true;

        float timePassed = 0;

        float reverseTime = 0.5f;
        float rotationTime = Random.Range(2f, 4f);

        if (Random.Range(0, 2) == 1)
        {
            eulerAngleVelocity *= -1;
        }

        while (timePassed < reverseTime)
        {
            rb.velocity = -transform.forward * reverseSpeedMultiplier;

            timePassed += Time.deltaTime;

            yield return null;
        }

        while (timePassed >= reverseTime && timePassed < rotationTime)
        {
            Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);

            timePassed += Time.deltaTime;

            yield return null;
        }
        randomizeDirectionInProgress = false;
    }

    private void ChasePlayer()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, 
            Quaternion.LookRotation(new Vector3(playerTransform.position.x, 0f, playerTransform.position.z) - new Vector3(transform.position.x, 0f, transform.position.z)), 
            rotationSpeed * Time.deltaTime);
    }
}
