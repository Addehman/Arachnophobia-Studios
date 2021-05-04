using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchQuest : MonoBehaviour
{
    public GameObject lightSource1;
    public GameObject lightSource2;
    public GameObject check;
    bool isFinished = false;
    bool canSwitchLight = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isFinished == false && canSwitchLight == true)
        {
            Winstate.AddCompletedQuest();
            lightSource1.SetActive(false);
            lightSource2.SetActive(false);
            check.SetActive(true);
            isFinished = true;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            canSwitchLight = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        canSwitchLight = false;
    }
}
