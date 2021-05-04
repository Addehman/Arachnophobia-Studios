using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleQuest : MonoBehaviour
{
    public GameObject light;
    public GameObject check;
    bool isFinished = false;
    bool canLightCandle = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isFinished == false && canLightCandle == true)
        {
            Winstate.AddCompletedQuest();
            light.SetActive(true);
            check.SetActive(true);
            isFinished = true;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            canLightCandle = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        canLightCandle = false;
    }
}
