using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchQuest : MonoBehaviour
{
    public GameObject lightSource1;
    public GameObject lightSource2;
    public GameObject check;
    bool isFinished = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E) && isFinished == false)
            {
                Winstate.AddCompletedQuest();
                lightSource1.SetActive(false);
                lightSource2.SetActive(false);
                check.SetActive(true);
                isFinished = true;
            }
        }
    }
}
