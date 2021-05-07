using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumPlayerHitbox : MonoBehaviour
{
    VacuumMovement vacuumMove;

    private void Start()
    {
        vacuumMove = GetComponentInParent<VacuumMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            vacuumMove.playerInSight = true;
            vacuumMove.playerTransform = other.gameObject.GetComponent<Transform>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            vacuumMove.playerInSight = false;
        }
    }
}
