using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumBlackhole : MonoBehaviour
{
    Transform vacuumTransform;
    Transform playerTransform;
    Rigidbody playerRb;

    public float pullAmount;
    public float pullUpAmount;

    private void Start()
    {
        vacuumTransform = GetComponentInParent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Suger in playern");

            playerTransform = other.gameObject.GetComponent<Transform>();
            playerRb = other.gameObject.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BlackHole();
            Debug.Log("Suger in playern");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Slutade suga in playern");
        }
    }

    private void BlackHole()
    {
        playerRb.AddForce((vacuumTransform.position - playerTransform.position).normalized * pullAmount);
        playerRb.AddForce(Vector3.up * pullUpAmount);
    }
}
