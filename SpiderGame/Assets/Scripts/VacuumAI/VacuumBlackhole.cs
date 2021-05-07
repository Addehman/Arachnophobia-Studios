using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class VacuumBlackhole : MonoBehaviour
{
    Transform vacuumTransform;
    Transform playerTransform;
    Rigidbody playerRb;

    public event Action<bool> PullingPlayer;

    public Vector3 playerDistance;
    public float pullAmount = 14f; //Alternative numbers 7.1 //14 works good with velocity movement
    public float pullUpAmount = 2f; //Alternative numbers 4 //2 works good with velocity movement

    private void Start()
    {
        vacuumTransform = GetComponentInParent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.gameObject.GetComponent<Transform>();
            playerRb = other.gameObject.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PullingPlayer != null)
            {
                PullingPlayer(true);
            }
            BlackHole();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRb.velocity = Vector3.zero;
            if (PullingPlayer != null)
            {
                PullingPlayer(false);
            }
        }
    }

    private void BlackHole()
    {
        Vector3 force = (vacuumTransform.position - playerTransform.position).normalized * pullAmount;
        playerRb.AddForce(force);
        playerRb.AddForce(Vector3.up * pullUpAmount);

        //Debug force: Debug.Log($"Force: {force}");
        //Alternative way of BlackHole effect, using sphere collider instead.
        //float gravityIntensity = Vector3.Distance(transform.position, playerTransform.position) / sphereCol.radius;
        //playerRb.AddForce((transform.position - playerTransform.position) * gravityIntensity * playerRb.mass * pullAmount * Time.smoothDeltaTime);
    }
}
