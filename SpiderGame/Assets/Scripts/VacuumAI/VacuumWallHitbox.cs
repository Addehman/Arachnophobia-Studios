using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumWallHitbox : MonoBehaviour
{
    VacuumMovement vacuumMove;

    private void Start()
    {
        vacuumMove = GetComponentInParent<VacuumMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other);
        if (other.CompareTag("Wall"))
        {
            if (!vacuumMove.randomizeDirectionInProgress)
            {
                StartCoroutine(vacuumMove.RandomizeDirection());
            }
        }
    }
}
