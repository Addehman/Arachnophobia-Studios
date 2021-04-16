using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumMovement : MonoBehaviour
{
    Rigidbody rb;

    public Vector3 moveForward;
    Vector3 eulerAngleVelocity;

    int rotationSpeed = 1;

    public Transform playerTransform;

    public bool randomizeDirectionInProgress;
    public bool playerInSight;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        eulerAngleVelocity = new Vector3(0, -5, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            print("Tryck");
            StopCoroutine(RandomizeDirection());
        }
    }

    void FixedUpdate()
    {
        if (playerInSight)
        {
            ChasePlayer();
        }

        if (!randomizeDirectionInProgress && !playerInSight)
        {
            rb.velocity = transform.forward;
        }
    }

    public IEnumerator RandomizeDirection()
    {
        randomizeDirectionInProgress = true;

        float timePassed = 0;

        float reverseTime = 0.5f;
        float rotationTime = Random.Range(2f, 4.9f);

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
        randomizeDirectionInProgress = false;
    }

    private void ChasePlayer()
    {
        //TODO: Make Vacuum only rotate on Y-axis towards player. Save local variable of Y value?
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerTransform.position - transform.position), rotationSpeed * Time.deltaTime);
    }
}
