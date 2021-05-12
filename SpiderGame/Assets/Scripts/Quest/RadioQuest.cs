using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioQuest : MonoBehaviour
{
    public GameObject song;
    public GameObject helpText;
    public GameObject check;
    public GameObject questCircle;
    bool isFinished = false;
    bool canPlayRadio = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isFinished == false && canPlayRadio == true)
        {
            Winstate.AddCompletedQuest();
            song.SetActive(true);
            check.SetActive(true);
            helpText.SetActive(false);
            questCircle.SetActive(false);
            isFinished = true;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            helpText.SetActive(true);
            canPlayRadio = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        helpText.SetActive(false);
        canPlayRadio = false;
    }
}
