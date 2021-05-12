using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FliesQuest : MonoBehaviour
{
    public GameObject flies;
    public GameObject helpText;
    public GameObject check;
    public GameObject questCircle;
    bool isFinished = false;
    bool canPickUpFlies = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isFinished == false && canPickUpFlies == true)
        {
            Winstate.AddCompletedQuest(); // Winstate needs to be fixed from 4 to 5.
            flies.SetActive(false);
            helpText.SetActive(false);
            check.SetActive(true);
            questCircle.SetActive(false);
            isFinished = true;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            helpText.SetActive(true);
            canPickUpFlies = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        helpText.SetActive(false);
        canPickUpFlies = false;
    }
}