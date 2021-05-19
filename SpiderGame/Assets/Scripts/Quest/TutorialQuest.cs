using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialQuest : MonoBehaviour
{
    public GameObject helpText;
    public GameObject check;
    public GameObject questCircle;
    public bool isFinished = false;
    bool canPress = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isFinished == false && canPress == true)
        {
       //     Winstate.AddCompletedQuest();
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
            canPress = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        helpText.SetActive(false);
        canPress = false;
    }
}
