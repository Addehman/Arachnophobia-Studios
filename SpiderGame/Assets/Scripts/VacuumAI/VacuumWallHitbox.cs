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
        if (!other.CompareTag("Floor") && !other.CompareTag("WebQuest"))
        {
         //   print(other);
            if (!vacuumMove.randomizeDirectionInProgress)
            {
                StartCoroutine(vacuumMove.RandomizeDirection());
            }
        }
    }
}
