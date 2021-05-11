using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FliesQuest : MonoBehaviour
{
    public GameObject flies;
    public GameObject check;
    bool isFinished = false;
    bool canPickUpFlies = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isFinished == false && canPickUpFlies == true)
        {
            Winstate.AddCompletedQuest(); // Winstate needs to be fixed from 4 to 5.
            flies.SetActive(false);
            check.SetActive(true);
            isFinished = true;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            canPickUpFlies = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        canPickUpFlies = false;
    }
}