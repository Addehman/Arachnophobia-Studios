using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioQuest : MonoBehaviour
{
    public GameObject song;
    public GameObject check;
    bool isFinished = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E) && isFinished == false)
            {
                Winstate.AddCompletedQuest();
                song.SetActive(true);
                check.SetActive(true);
                isFinished = true;
            }
        }
    }
}
