using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioQuest : MonoBehaviour
{
    public GameObject song;
    public GameObject check;
    bool isFinished = false;
    bool canPlayRadio = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isFinished == false && canPlayRadio == true)
        {
            Winstate.AddCompletedQuest();
            song.SetActive(true);
            check.SetActive(true);
            isFinished = true;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            canPlayRadio = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        canPlayRadio = false;
    }
}
