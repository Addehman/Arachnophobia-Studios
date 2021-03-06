using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchQuest : MonoBehaviour
{
    public GameObject lampModelsOn;
    public GameObject lampModelsOff;
    public GameObject lightSource1;
    public GameObject lightSource2;
    public GameObject lightSource3;
    public GameObject questCircle;
    public GameObject check;
    public GameObject helpText;
    public SpiderAudio spiderAudio;

    bool isFinished = false;
    bool canSwitchLight = false;

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && isFinished == false && canSwitchLight == true)
        {
            spiderAudio.LightSwitch();
            Winstate.AddCompletedQuest();

            lampModelsOn.SetActive(false);
            lampModelsOff.SetActive(true);
            lightSource1.SetActive(false);
            lightSource2.SetActive(false);
            lightSource3.SetActive(false);
            questCircle.SetActive(false);
            helpText.SetActive(false);
            check.SetActive(true);
            isFinished = true;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            helpText.SetActive(true);
            canSwitchLight = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        helpText.SetActive(false);
        canSwitchLight = false;
    }
}
