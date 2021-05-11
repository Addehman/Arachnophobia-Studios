using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleQuest : MonoBehaviour
{
    public GameObject light1;
    public GameObject light2;
    public GameObject check;
    public GameObject questCircle;
    bool isFinished = false;
    bool canLightCandle = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isFinished == false && canLightCandle == true)
        {
            Winstate.AddCompletedQuest();
            light1.SetActive(true);
            light2.SetActive(true);
            check.SetActive(true);
            questCircle.SetActive(false);
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
